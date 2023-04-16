using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "SceneData", fileName = "SceneData")]
public class SO_SceneData : ScriptableObject
{
    public float friction = 10f;
}
