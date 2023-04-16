using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CheckpointController : MonoBehaviour
{
    public static CheckpointController instance;

    public List<CheckpointSelector> checkpointSelectors;
    public CheckpointSelector currentCheckpoint;

    private void Awake()
    {
        instance = this;
        currentCheckpoint = checkpointSelectors[0];
    }

    public void SelectCheckpoint(CheckpointSelector selectedCheckpoint)
    {
        currentCheckpoint = selectedCheckpoint;
    }
}
