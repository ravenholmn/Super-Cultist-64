using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScenesData", fileName = "ScenesData")]
public class SO_ScenesData : ScriptableObject
{
    public List<SO_SceneData> sceneDataList;
    public int activeSceneIndex;
    public int nextSceneIndex;
}
