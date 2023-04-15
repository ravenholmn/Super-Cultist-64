using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSelector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerColliderController player = other.GetComponent<PlayerColliderController>();
        
        if (player)
        {
            CheckpointController.instance.SelectCheckpoint(this);
        }
    }
}
