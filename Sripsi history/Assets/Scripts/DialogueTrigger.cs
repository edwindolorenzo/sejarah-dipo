using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public bool finished = false;

    public void Start()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, finished);
    }

    private void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, finished);
    }
}
