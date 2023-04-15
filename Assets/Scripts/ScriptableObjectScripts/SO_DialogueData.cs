using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DialogueData", fileName = "DialogueData")]
public class SO_DialogueData : ScriptableObject
{
    public Line[] lines;
    public int lineIndex;
}

[Serializable]

public class Line
{
    public Character character;
    public string line;
    public float textSpeed;
}

public enum Character
{
    Npc,
    Player
}
