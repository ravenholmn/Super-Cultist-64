using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "ItemData", fileName = "ItemData")]
public class SO_ItemData : ScriptableObject
{
    public float friction = 10f;
}
