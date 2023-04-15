using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerConfig", fileName = "Player Config")]
public class SO_PlayerConfig : ScriptableObject
{
    public float speed = 3f;
    public float fastWalkSpeed = 5f;
    public float turnSpeed = 20f;
    public float mouseSensitivityX;
    public float mouseSensitivityY;
    public List<float> jumpForces;
    public float jumpDuration = 0.25f;
    public float maxFallSpeed = 40f;
}
