using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    void Start()
    {
        transform.DOLocalRotate(new Vector3(6f, 0f, 0), 0f);
        transform.DOLocalRotate(new Vector3(-6f, 0f, 0), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        transform.DOScale(new Vector3(1.2f, 0.8f, 1.2f), 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }
}
