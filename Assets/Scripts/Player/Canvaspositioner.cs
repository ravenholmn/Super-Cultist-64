using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvaspositioner : MonoBehaviour
{
    public LayerMask ground;
    
    private void LateUpdate()
    {
        PositionCanvas();
        ScaleCanvas();
    }

    private void PositionCanvas()
    {
        transform.position = PlayerController.Instance.playerTransform.position;
        if (Physics.Raycast(PlayerController.Instance.playerTransform.position, Vector3.down, out RaycastHit hitDown, 250f, ground))
        {
            Vector3 temp = transform.position;
            temp.y = hitDown.point.y + 0.05f;
            transform.position = temp;
        }
    }

    private void ScaleCanvas()
    {
        float distance = (PlayerController.Instance.playerTransform.position.y - transform.position.y) / 10;
        float percentage = Mathf.Clamp01(1 - distance);
        transform.localScale = Vector3.one * percentage;
    }
}
