using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
        public static PlayerController Instance;

        public PlayerMovement PlayerMovement;
        public PlayerHealth PlayerHealth;
        public HeartUI HeartUI;
        public SO_PlayerConfig PlayerConfig;
        public Transform playerTransform;
        public bool PlayerDied;
        private Vector3 launchDirection;

        private void Awake()
        {
                Instance = this;
        }

        public void GotHit(Vector3 direction)
        {
                PlayerHealth.TakeDamage();
                HeartUI.UpdateUI(PlayerHealth.health);
                launchDirection = direction;
                Invoke(nameof(LaunchPlayer), 0.15f);
        }

        private void LaunchPlayer()
        {
                if (PlayerHealth.health > 0)
                {
                        PlayerMovement.LaunchPlayer(launchDirection);
                }
        }

        public void Die()
        {
                PlayerHealth.Die();
                PlayerMovement.Die();
                HeartUI.UpdateUI(PlayerHealth.health);
                PlayerDied = true;
                Debug.Log("playerDied");
                Invoke(nameof(SpawnCharacter), 0.2f);
        }

        private void SpawnCharacter()
        {
                PlayerHealth.Respawn();
                PlayerMovement.Respawn(CheckpointController.instance.currentCheckpoint.transform.position);
                HeartUI.UpdateUI(PlayerHealth.health);
                PlayerDied = false;
        }
}