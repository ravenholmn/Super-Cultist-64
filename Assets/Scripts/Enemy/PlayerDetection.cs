using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetection : MonoBehaviour
{
    public enum RangeType
    {
        Sight,
        Attack
    }

    public RangeType rangeType;
    [SerializeField] private Enemy enemy;
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerColliderController player = other.transform.gameObject.GetComponent<PlayerColliderController>();
        
        if (player)
        {
            PlayerInRange();
        }
    }

    private void PlayerInRange()
    {
        enemy.PlayerInRange(rangeType, true);
    }
    
    private void OnTriggerExit(Collider other)
    {
        PlayerColliderController player = other.transform.gameObject.GetComponent<PlayerColliderController>();

        if (player)
        {
            PlayerLeftRange();
        }
    }

    private void PlayerLeftRange()
    {
        enemy.PlayerInRange(rangeType, false);
    }
}
