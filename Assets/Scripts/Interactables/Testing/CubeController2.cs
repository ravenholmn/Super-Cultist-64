using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CubeController2 : MonoBehaviour
{
        [SerializeField] private CinemachineVirtualCamera playerCamera;
        [SerializeField] private CinemachineVirtualCamera cameraToSwitchTo;

        public void OnPressingE()
        {
                SwitchCamera();
        }

        private void SwitchCamera()
        {
                if (playerCamera.gameObject.activeSelf)
                {
                        playerCamera.gameObject.SetActive(false);
                        cameraToSwitchTo.gameObject.SetActive(true);
                }
                else
                {
                        playerCamera.gameObject.SetActive(true);
                        cameraToSwitchTo.gameObject.SetActive(false);
                }
        }
}