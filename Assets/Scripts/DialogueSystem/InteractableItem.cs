using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e")) {
            TriggerDialogue("testStory");
        }
    }

    public void TriggerDialogue(string filename)
    {
        if (!DialogueManager.instance) return;
        
        DialogueManager.instance.StartDialogue(filename);
    }
}
