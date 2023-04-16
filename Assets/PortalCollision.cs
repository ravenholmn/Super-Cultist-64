using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PortalCollision : MonoBehaviour
{
    public CinemachineVirtualCamera portalCamera;
    private void OnTriggerEnter(Collider other)
    {
        PlayerColliderController player = other.GetComponent<PlayerColliderController>();

        if (player)
        {
            StartCoroutine(ChangeCamera());
        }
    }

    private IEnumerator ChangeCamera()
    {
        var wait = new WaitForEndOfFrame();
        
        PlayerController.Instance.playerCamera.gameObject.SetActive(false);
        portalCamera.gameObject.SetActive(true);
        
        yield return new WaitUntil(() => PlayerController.Instance.CinemachineBrain.IsBlending);
        
        while (PlayerController.Instance.CinemachineBrain.IsBlending)
        {
            Fader.instance.FadeOut(PlayerController.Instance.CinemachineBrain.ActiveBlend.BlendWeight);
            yield return wait;
        }
        LoadingPanelController.instance.LoadNextScene();
    }
}
