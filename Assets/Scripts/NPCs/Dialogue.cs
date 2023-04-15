using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public static Dialogue instance;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI nameText;
    private SO_NPCData currentNpcData;
    private int index;
    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SO_DialogueData currentDialogue = currentNpcData.dialogues[currentNpcData.dialogueIndex];
            
            if (text.text == currentDialogue.lines[currentDialogue.lineIndex].line)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = currentDialogue.lines[currentDialogue.lineIndex].line;
            }
        }
    }

    public void StartDialogue(SO_NPCData npcData)
    {
        gameObject.SetActive(true);
        currentNpcData = npcData;
        text.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        SO_DialogueData currentDialogue = currentNpcData.dialogues[currentNpcData.dialogueIndex];
        
        if (currentDialogue.lineIndex < currentDialogue.lines.Length - 1)
        {
            currentDialogue.lineIndex++;
            text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            currentNpcData.dialogueIndex++;
            currentNpcData = default;
            gameObject.SetActive(false);
        }
    }

    IEnumerator TypeLine()
    {
        var wait = new WaitForEndOfFrame();
        float t;
        float duration = currentNpcData.TextSpeed();

        ChangeName();
        SO_DialogueData currentDialogue = currentNpcData.dialogues[currentNpcData.dialogueIndex];

        foreach (char c in currentDialogue.lines[currentDialogue.lineIndex].line.ToCharArray())
        {
            t = 0;
            text.text += c;
            while (t < duration)
            {
                t += Time.deltaTime;
                yield return wait;
            }
        }
    }

    void ChangeName()
    {
        nameText.text = currentNpcData.GetName();
    }
}