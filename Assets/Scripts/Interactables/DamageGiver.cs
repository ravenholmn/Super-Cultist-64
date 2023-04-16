using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageGiver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerColliderController player = other.GetComponent<PlayerColliderController>();

        if (player)
        {
            PlayerController.Instance.GotHit(-PlayerController.Instance.PlayerMovement.GetDirection());
        }
    }
}
