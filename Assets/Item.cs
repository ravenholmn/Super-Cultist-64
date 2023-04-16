using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    public SO_NPCData _npcData;
    public UnityEvent eventToInvoke;
    public ItemAnimator ItemAnimator;

    private void OnTriggerEnter(Collider other)
    {
        PlayerColliderController player = other.transform.gameObject.GetComponent<PlayerColliderController>();

        if (player)
        {
            ItemAnimator.transform.DOKill();
            Dialogue.instance.StartDialogue(_npcData, eventToInvoke);
        }
    }
}
