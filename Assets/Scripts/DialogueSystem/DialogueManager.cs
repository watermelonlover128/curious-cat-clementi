using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;  
using System.IO;  
using System.Text;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance; 

    [SerializeField]
    private float textDelay = 0.05f;

    private Queue<Dialogue> sentences = new Queue<Dialogue>();
    private bool currentlyInDialogue = false;
    private bool currentlyTyping = false;
    private Dialogue currentLine;

    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI speechText;

    // sets a global static instance of dialogue manager so other scripts can access it
    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        dialoguePanel.SetActive(false); // ensure the dialogue UI isnt visible initially
    }

    void Update()
    {
        if (currentlyInDialogue && Input.anyKeyDown)
        {
            if (currentlyTyping) 
            {
                // skips typewriting animation
                StopAllCoroutines(); 
                speechText.text = currentLine.sentence;
                currentlyTyping = false;
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(string filename)
    {
        if (currentlyInDialogue) return;

        // reads the corresponding file for the dialogue
        sentences.Clear();
        ReadFile(filename);

        dialoguePanel.SetActive(true);
        currentlyInDialogue = true;

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0) // end of dialogue
        {
            EndDialogue();
            return;
        }

        // grabs next line, sets corresponding speaker
        currentLine = sentences.Dequeue();
        speakerText.text = currentLine.speaker;

        currentlyInDialogue = true;

        // ensure previous typewriter coroutine is stopped before starting a new one
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine.sentence));
    }

    // creates the typing animation
    IEnumerator TypeSentence(string sentence)
    {
        speechText.text = "";
        currentlyTyping = true;

        foreach(char letter in sentence.ToCharArray())
        {
            speechText.text += letter;
            yield return new WaitForSeconds(textDelay);
        }

        currentlyTyping = false;
    }

    // resets everything
    public void EndDialogue()
    {
        currentlyInDialogue = false;
        dialoguePanel.SetActive(false);

        speakerText.text = "";
        speechText.text = "";
    }

    // story texts are stored in txt files under resources (can change the location of txt file storage later)
    // txt files follow a format of first line speaker, second line dialogue text, and repeat
    public void ReadFile(string filename)
    {
        TextAsset file = (TextAsset)Resources.Load("Story/" + filename);

        using (StringReader sr = new StringReader(file.text))
        {
            string line;
            int counter = 1;
            Dialogue temp = new Dialogue();

            while ((line = sr.ReadLine()) != null)
            {
                if (counter == 1)
                {
                    temp.speaker = line;
                }
                else if (counter == 2)
                {
                    temp.sentence = line;
                    sentences.Enqueue(temp);
                    temp = new Dialogue();
                    counter = 0;
                }
                counter++;
            }
        } 
    }
}
