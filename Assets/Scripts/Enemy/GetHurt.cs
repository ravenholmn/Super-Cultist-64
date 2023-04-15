using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHurt : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerColliderController player = other.transform.gameObject.GetComponent<PlayerColliderController>();
        
        if (player)
        {
            PlayerEntered();
        }
    }

    private void PlayerEntered()
    {
        if (PlayerController.Instance.PlayerMovement.playerState == PlayerMovement.PlayerState.Falling)
        {
            enemy.GetHit();
        }
    }
}
