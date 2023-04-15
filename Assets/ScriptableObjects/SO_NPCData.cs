using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPCData", fileName = "NPCData")]
public class SO_NPCData : ScriptableObject
{
    public string characterName;
    public SO_DialogueData[] dialogues;
    public int dialogueIndex;
    public float defaultTextSpeed = 0.05f;

    public float TextSpeed()
    {
        SO_DialogueData currentDialogue = dialogues[dialogueIndex];
        return currentDialogue.lines[currentDialogue.lineIndex].textSpeed <= 0 ? defaultTextSpeed : currentDialogue.lines[currentDialogue.lineIndex].textSpeed;
    }

    public string GetName()
    {
        SO_DialogueData currentDialogue = dialogues[dialogueIndex];
        return currentDialogue.lines[currentDialogue.lineIndex].character == Character.Npc ? characterName : "You";
    }

    [ContextMenu("ResetIndexes")]
    public void ResetIndexes()
    {
        dialogueIndex = 0;
        foreach (var dialogue in dialogues)
        {
            dialogue.lineIndex = 0;
        }
    }
}