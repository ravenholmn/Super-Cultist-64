using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectResetter : MonoBehaviour
{
    public List<SO_NPCData> NpcDatas;
    public SO_ScenesData ScenesData;

    public void ResetDatas()
    {
        foreach (var data in NpcDatas)
        {
            data.ResetIndexes();
        }

        ScenesData.activeSceneIndex = 0;
        ScenesData.nextSceneIndex = 0;
    }
}
