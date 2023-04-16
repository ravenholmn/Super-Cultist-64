using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemAnimator : MonoBehaviour
{
    void Start()
    {
        Animate();
    }

    void Animate()
    {
        transform.DOLocalRotate(new Vector3(0f, 360f, 0f), 5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        transform.DOLocalMoveY(0.75f, 1.5f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }
}