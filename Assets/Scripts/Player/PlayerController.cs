using System;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
        public static PlayerController Instance;

        public PlayerMovement PlayerMovement;
        public PlayerHealth PlayerHealth;
        public SO_PlayerConfig PlayerConfig;
        public Transform playerTransform;
        public Transform playerCamera;
        public CinemachineBrain CinemachineBrain;
        public bool PlayerDied;
        private Vector3 launchDirection;
        
        private void Awake()
        {
                Instance = this;
                CinemachineBrain = FindObjectOfType<CinemachineBrain>();
        }

        public void GotHit(Vector3 direction)
        {
                PlayerHealth.TakeDamage();
                HeartUI.instance.UpdateUI(PlayerHealth.health);
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
                SoundEffectPlayer.instance.PlaySfx(SoundEffectPlayer.Sfx.Death);
                PlayerHealth.Die();
                PlayerMovement.Die();
                HeartUI.instance.UpdateUI(PlayerHealth.health);
                PlayerDied = true;
                Invoke(nameof(SpawnCharacter), 0.2f);
        }

        private void SpawnCharacter()
        {
                PlayerHealth.Respawn();
                PlayerMovement.Respawn(CheckpointController.instance.currentCheckpoint.transform.position);
                HeartUI.instance.UpdateUI(PlayerHealth.health);
                PlayerDied = false;
        }
}