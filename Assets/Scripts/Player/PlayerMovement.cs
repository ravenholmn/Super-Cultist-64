using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : PlayerInputHandler
{
    #region Variables

    public enum PlayerState
    {
        Default,
        Jumping,
        Falling
    }
    
    public enum StompState
    {
        Default,
        StompCharging,
        Stomping
    }

    public PlayerState playerState;
    public StompState stompState;

    [SerializeField] private PlayerAnimationController animationController;
    [SerializeField] private SO_ScenesData scenesData;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform mesh;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float coyoteTime;
    
    private bool _canStomp;
    private bool _gotHit;
    private float _jumpTimer;
    private float _stompChargeTimer;
    private int _jumpCount;
    private float? _lastGroundedTime;
    private float? _lastButtonPressedTime;
    private Coroutine _jumpCountReset;
    private Vector3 _velocity;
    private Vector3 _curVel;
    [SerializeField] private float friction = 1f;
    private float lastFriction;

    #endregion

    private void Start()
    {
        friction = scenesData.sceneDataList[scenesData.activeSceneIndex].friction;
    }

    private void Update()
    {
        if (InputBlocker()) return;
        
        MoveInput();
        CoyoteTime();
        JumpControl();
        StompControl();
        UpdateDirection();
        UpdateVelocity();
        MovePlayer();
        RotatePlayer();
    }

    #region Calculation

    private void UpdateDirection()
    {
        if (_gotHit) return;
        
        Direction = stompState == StompState.Default
            ? cameraRoot.forward * VerticalInput + cameraRoot.right * HorizontalInput
            : Vector3.zero;
    }

    void CoyoteTime()
    {
        if (_gotHit) return;
        
        if (characterController.isGrounded)
        {
            _lastGroundedTime = Time.time;
            if (_jumpTimer < PlayerController.Instance.PlayerConfig.jumpDuration)
                _jumpTimer = PlayerController.Instance.PlayerConfig.jumpDuration;
            _canStomp = true;
        }

        if (Jump)
        {
            _lastButtonPressedTime = Time.time;
        }
    }

    void JumpControl()
    {
        if (!(Time.time - _lastButtonPressedTime <= coyoteTime)) return;
        if (playerState == PlayerState.Falling) return;
        ChangeState(PlayerState.Jumping);
        SoundEffectPlayer.instance.PlaySfx(SoundEffectPlayer.Sfx.Jump);
    }

    void StompControl()
    {
        if (playerState != PlayerState.Default && Crouch && _canStomp)
        {
            _canStomp = false;
            ChangeState(PlayerState.Default);
            ChangeStompState(StompState.StompCharging);
        }

        if (ReleasedCrouch)
        {
            ChangeStompState(StompState.Default);
        }

        if (stompState != StompState.StompCharging) return;
        _stompChargeTimer -= Time.deltaTime;

        if (!(_stompChargeTimer <= 0)) return;
        ChangeState(PlayerState.Falling);
        ChangeStompState(StompState.Stomping);
    }

    private IEnumerator JumpCountReset()
    {
        var wait = new WaitForEndOfFrame();
        float t = 0;

        if (_jumpCount < 3)
        {
            while (t < 0.25f)
            {
                t += Time.deltaTime;
                yield return wait;
            }
        }

        _jumpCount = 0;
    }

    #endregion

    #region Action

    private void MovePlayer()
    {
        if (PlayerController.Instance.PlayerDied) return;
        float speed = PlayerController.Instance.PlayerConfig.speed;
        
        if (!_gotHit)
        {
            if (HoldingShift)
            {
                speed = PlayerController.Instance.PlayerConfig.fastWalkSpeed;
            }
            else if (HoldingCrouch)
            {
                speed = PlayerController.Instance.PlayerConfig.crouchSpeed;
            }
        }
        else
        {
            speed = PlayerController.Instance.PlayerConfig.launchSpeed;
        }
        
        _curVel = Vector3.Lerp(_curVel, Direction, friction * Time.deltaTime);
        characterController.Move(_curVel * (speed * Time.deltaTime));
        characterController.Move(_velocity * Time.deltaTime);
    }

    private void UpdateVelocity()
    {
        if (_gotHit)
        {
            animationController.IdleAnimation();
            _velocity.y += Physics.gravity.y * 5f * Time.deltaTime;
            return;
        }
        switch (playerState)
        {
            case PlayerState.Jumping:
            {
                animationController.IdleAnimation();
                if (_jumpCountReset != default)
                {
                    StopCoroutine(_jumpCountReset);
                }

                if (_jumpTimer <= 0 || !HoldingJump || _jumpCount >= 3)
                {
                    ChangeState(PlayerState.Falling);
                    return;
                }

                if (HoldingJump)
                {
                    _jumpTimer -= Time.deltaTime;
                    _lastGroundedTime = null;
                    _lastButtonPressedTime = null;
                }

                _velocity.y = PlayerController.Instance.PlayerConfig.jumpForces[_jumpCount];
                break;
            }
            case PlayerState.Falling:
            {
                if (characterController.isGrounded)
                {
                    _jumpCount++;
                    _jumpCountReset = StartCoroutine(JumpCountReset());
                    ChangeState(PlayerState.Default);
                    if (stompState == StompState.Stomping)
                    {
                        _jumpCount = 2;
                        ChangeStompState(StompState.Default);
                    }
                    
                    SoundEffectPlayer.instance.PlaySfx(SoundEffectPlayer.Sfx.Fall);
                }

                _velocity.y += stompState != StompState.Stomping
                    ? Physics.gravity.y * 7f * Time.deltaTime
                    : Physics.gravity.y * 45f * Time.deltaTime;
                break;
            }
            default:
            {
                if (stompState == StompState.StompCharging)
                {
                    _velocity.y = 0;
                }
                else if (Time.time - _lastGroundedTime <= coyoteTime)
                {
                    if (_stompChargeTimer < PlayerController.Instance.PlayerConfig.stompChargeTime)
                    {
                        _stompChargeTimer = PlayerController.Instance.PlayerConfig.stompChargeTime;
                    }

                    _velocity.y = -1f;
                }
                else
                {
                    _velocity.y += Physics.gravity.y * 5f * Time.deltaTime;
                }

                break;
            }
        }
    }

    private void RotatePlayer()
    {
        if (Direction.magnitude != 0f)
        {
            if (playerState == PlayerState.Default)
            {
                switch (HoldingShift)
                {
                    case false:
                        animationController.WalkAnimation();
                        break;
                    case true:
                        animationController.RunAnimation();
                        break;
                }
            }

            mesh.forward = Vector3.Lerp(mesh.forward, _curVel,
                Time.deltaTime * PlayerController.Instance.PlayerConfig.turnSpeed);
        }
        else
        {
            animationController.IdleAnimation();
        }
    }

    public void LaunchPlayer(Vector3 direction)
    {
        _gotHit = true;
        lastFriction = friction;
        friction = 10f;
        direction.y = 0;
        Direction = direction;
        _velocity.y = PlayerController.Instance.PlayerConfig.launchForce;
        Invoke(nameof(ResetHit), 1f);
    }

    void ResetHit()
    {
        _gotHit = false;
        friction = lastFriction;
    }

    #endregion

    private void ChangeState(PlayerState newState)
    {
        playerState = newState;
    }
    
    private void ChangeStompState(StompState newState)
    {
        stompState = newState;
    }

    public void Die()
    {
        _gotHit = false;
        playerState = PlayerState.Default;
        Direction = Vector3.zero;
        _velocity.y = 0f;
    }

    public void Respawn(Vector3 spawnPosition)
    {
        characterController.enabled = false;
        spawnPosition.y += 1;
        transform.position = spawnPosition;
        characterController.enabled = true;
    }

    public Vector3 GetDirection()
    {
        return _curVel;
    }
}