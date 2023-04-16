using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    enum AnimationState
    {
        Idle,
        Walk,
        Run
    }

    private AnimationState _state;
    
    public void IdleAnimation()
    {
        if (_state == AnimationState.Idle) return;
        _state = AnimationState.Idle;
        
        ResetTransform();
        SoundEffectPlayer.instance.KillSounds();
        transform.DOScaleY(0.8f, 0.75f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    public void WalkAnimation()
    {
        if (_state == AnimationState.Walk) return;
        _state = AnimationState.Walk;
        
        ResetTransform();
        SoundEffectPlayer.instance.PlayWalkSfx(0.25f);
        transform.DOLocalRotate(new Vector3(0f, 0f, 8f), 0.25f, RotateMode.FastBeyond360).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOLocalRotate(new Vector3(0f, 0f, -8f), 0.25f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(200, LoopType.Yoyo);
        });
        transform.DOScaleY(0.9f, 0.125f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    public void RunAnimation()
    {
        if (_state == AnimationState.Run) return;
        _state = AnimationState.Run;
        
        ResetTransform();
        SoundEffectPlayer.instance.PlayWalkSfx(0.15f);
        transform.DOLocalRotate(new Vector3(0f, 0f, 8f), 0.15f, RotateMode.FastBeyond360).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOLocalRotate(new Vector3(0f, 0f, -8f), 0.15f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        });
        transform.DOScaleY(0.95f, 0.075f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void ResetTransform()
    {
        transform.DOKill();
        var transformRotation = transform.localRotation;
        transformRotation.eulerAngles = Vector3.zero;
        transform.localRotation = transformRotation;
        transform.localScale = Vector3.one;
    }
}
