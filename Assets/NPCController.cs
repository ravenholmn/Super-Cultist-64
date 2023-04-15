using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NPCController : MonoBehaviour
{
    [SerializeField] private SO_NPCData npcData;

    public void TakeInput()
    {
        OpenDialogueBox();
    }

    void OpenDialogueBox()
    {
        if (npcData.dialogueIndex >= npcData.dialogues.Length)
        {
            return;
        }
        Dialogue.instance.StartDialogue(npcData);
    }
}