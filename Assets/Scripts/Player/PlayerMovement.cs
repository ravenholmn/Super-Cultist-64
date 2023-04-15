using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : PlayerInputHandler
{
    public enum PlayerState
    {
        Default,
        Walking,
        Running,
        Crouching,
        CrouchWalking,
        Jumping,
        Falling
    }

    public PlayerState State;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform mesh;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float coyoteTime;
    private bool _canStomp;
    private bool _stompCharging;
    private bool _stomping;
    private bool _jumping;
    private bool _falling;
    private float _jumpTimer;
    private float _stompChargeTimer;
    private int _jumpCount;
    private float? _lastGroundedTime;
    private float? _lastButtonPressedTime;

    private Coroutine _jumpCountReset;
    private Vector3 _velocity;

    private void Update()
    {
        if (InputBlocker()) return;

        MoveInput();
        ChangeState();
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
        Direction = !_stompCharging ? cameraRoot.forward * VerticalInput + cameraRoot.right * HorizontalInput : Vector3.zero;
    }

    void CoyoteTime()
    {
        if (characterController.isGrounded)
        {
            _lastGroundedTime = Time.time;
            _canStomp = true;
        }

        if (Jump)
        {
            _lastButtonPressedTime = Time.time;
        }
    }

    void JumpControl()
    {
        if (_jumpTimer <= 0 || ReleasedJump || _jumpCount >= 3)
        {
            if (_jumping) _falling = true;
            _jumping = false;
            return;
        }

        if (_falling) return;

        if (Time.time - _lastButtonPressedTime <= coyoteTime)
        {
            _jumping = true;
        }

        if (HoldingJump)
        {
            _jumpTimer -= Time.deltaTime;
            _lastGroundedTime = null;
            _lastButtonPressedTime = null;
        }
    }

    void StompControl()
    {
        if ((_jumping || _falling) && Crouch && _canStomp)
        {
            _jumping = false;
            _falling = false;
            _canStomp = false;
            _stompCharging = true;
        }
        
        if (ReleasedCrouch)
        {
            _stompCharging = false;
            _falling = true;
        }

        if (_stompCharging /*&& HoldingCrouch*/)
        {
            _stompChargeTimer -= Time.deltaTime;

            if (_stompChargeTimer <= 0)
            {
                _stompCharging = false;
                _falling = true;
                _stomping = true;
            }
        }
    }

    private IEnumerator JumpCountReset()
    {
        var wait = new WaitForEndOfFrame();
        float t = 0;

        while (t < 0.25f)
        {
            t += Time.deltaTime;
            yield return wait;
        }

        _jumpCount = 0;
    }

    #endregion

    #region Action

    private void MovePlayer()
    {
        float speed = HoldingShift
            ? PlayerController.Instance.PlayerConfig.fastWalkSpeed
            : PlayerController.Instance.PlayerConfig.speed;

        characterController.Move(Direction.normalized * (speed * Time.deltaTime));
        characterController.Move(_velocity * Time.deltaTime);
    }

    private void UpdateVelocity()
    {
        if (_jumping)
        {
            if (_jumpCountReset != default)
            {
                StopCoroutine(_jumpCountReset);
            }
            
            if (HoldingJump)
            {
                _jumpTimer -= Time.deltaTime;
                _lastGroundedTime = null;
                _lastButtonPressedTime = null;
            }

            _velocity.y = PlayerController.Instance.PlayerConfig.jumpForces[_jumpCount];
        }
        else if (_falling)
        {
            if (characterController.isGrounded)
            {
                _jumpCount++;
                _jumpCountReset = StartCoroutine(JumpCountReset());
                _falling = false;
                if (_stomping)
                {
                    _stomping = false;
                    _jumpCount = 2;
                }
            }

            _velocity.y += !_stomping ? Physics.gravity.y * 7f * Time.deltaTime : Physics.gravity.y * 45f * Time.deltaTime;
        }
        else if (_stompCharging)
        {
            _velocity.y = 0;
        }
        else if (Time.time - _lastGroundedTime <= coyoteTime)
        {
            if (_jumpTimer < PlayerController.Instance.PlayerConfig.jumpDuration)
            {
                _jumpTimer = PlayerController.Instance.PlayerConfig.jumpDuration;
            }
            
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
    }

    private void RotatePlayer()
    {
        if (Direction.magnitude != 0f)
        {
            mesh.forward = Vector3.Lerp(mesh.forward, Direction,
                Time.deltaTime * PlayerController.Instance.PlayerConfig.turnSpeed);
        }
    }

    #endregion
    
    private void ChangeState()
    {
        State = PlayerState.Default;

        if (HorizontalInput != 0 || VerticalInput != 0)
        {
            State = PlayerState.Walking;
        }

        if (HoldingShift && (HorizontalInput != 0 || VerticalInput != 0))
        {
            State = PlayerState.Running;
        }

        if (HoldingCrouch)
        {
            State = PlayerState.Crouching;
        }

        if (HoldingCrouch && (HorizontalInput != 0 || VerticalInput != 0))
        {
            State = PlayerState.CrouchWalking;
        }

        if (HoldingJump)
        {
            State = PlayerState.Jumping;
        }

        if (_jumpTimer <= 0 || ReleasedJump || _jumpCount >= 3)
        {
            State = PlayerState.Falling;
        }
    }
}