using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : PlayerInputHandler
{
    [SerializeField] private Transform xOrientation;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (InputBlocker()) return;
        
        LookInput();
        ClampRotation();
        Look();
    }

    void ClampRotation()
    {
        XRotation = Mathf.Clamp(XRotation, -30f, 30f);
    }

    void Look()
    {
        transform.localRotation = Quaternion.Euler(0f, YRotation, 0f);
        xOrientation.transform.localRotation = Quaternion.Euler(XRotation, 0f, 0f);
    }
}
