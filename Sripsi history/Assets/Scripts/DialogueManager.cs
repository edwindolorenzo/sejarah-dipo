using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;
    public GameObject gamePlayUI;
    public GameObject finishUI;

    FinishGame finishGameScript;
    bool finished = true;
    bool finishGame;
    private Queue<string> sentences = new Queue<string>();
    private string sentence;

    // Start is called before the first frame update
    void Start()
    {
        finishGameScript = finishUI.GetComponent<FinishGame>();
    }

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue (Dialogue dialogue ,bool finish)
    {
        finishGame = finish;
        gamePlayUI.SetActive(false);
        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        if (finished)
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
        else
        {
            StopAllCoroutines();
            dialogueText.text = sentence;
            finished = true;
        }
        //just pop out

    }

    IEnumerator TypeSentence (string sentence)
    {
        finished = false;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
        finished = true;
        yield return null;
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        if (finishGame)
        {
            finishGameScript.GameFinished();
        }
        else
        {
            gamePlayUI.SetActive(true);
        }

    }
}
