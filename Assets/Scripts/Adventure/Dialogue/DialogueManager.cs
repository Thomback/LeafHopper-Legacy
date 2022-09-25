using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController thePlayerController;
    [SerializeField]
    private Transform cameraDefaultPosition;
    [SerializeField]
    private Transform cameraDialoguePosition;
    [SerializeField]
    private Transform cameraLookAt;
    [SerializeField]
    private float skipDelay = 0.25f;
    [SerializeField]
    private Canvas myCanvas;
    [SerializeField]
    private TMP_Text dialogueDisplayer;

    private Queue<string> sentences;
    private CameraLerp cameraLerp;
    private bool inConversation = false;
    private float skipDelayTimer = 0f;


    void Start()
    {
        if(thePlayerController == null)
        {
            thePlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
        if (cameraDefaultPosition == null)
        {
            cameraDefaultPosition = GameObject.Find("CameraDefaultPosition").GetComponent<Transform>();
        }
        if (cameraDialoguePosition == null)
        {
            cameraDialoguePosition = GameObject.Find("CameraDialoguePosition").GetComponent<Transform>();
        }
        if(cameraLookAt == null)
        {
            cameraLookAt = GameObject.Find("CameraLookAt").GetComponent<Transform>();
        }
        if(dialogueDisplayer == null)
        {
            dialogueDisplayer = GameObject.Find("DialogText").GetComponent<TMP_Text>();
        }
        cameraLerp = GameObject.Find("GameManager").GetComponent<CameraLerp>();


        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (inConversation)
        {
            skipDelayTimer += Time.deltaTime;
            if (Input.GetButtonUp("Fire1") && skipDelayTimer >= skipDelay)
            {
                DisplayNextSentence();
                skipDelayTimer = 0;
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (!inConversation) {
            inConversation = true;
            DialogueCamera(true);
            sentences.Clear();

            foreach(string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }
            
            DisplayNextSentence();
            myCanvas.enabled = true;
            thePlayerController.setControl(false);
        }
    }

    private void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDiaolgue();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogueDisplayer.SetText(sentence);
    }

    private void EndDiaolgue()
    {
        inConversation = false;
        dialogueDisplayer.SetText("");
        DialogueCamera(false);
        myCanvas.enabled = false;
        thePlayerController.setControl(true);
    }

    public void DialogueCamera(bool setDialogueMode)
    {
        if (setDialogueMode)
        {
            Transform newLookAt = thePlayerController.transform;
            cameraLerp.CameraLerping(cameraDialoguePosition.localPosition ,cameraLookAt);
        }
        else
        {
            cameraLerp.CameraLerping(cameraDefaultPosition.localPosition, cameraLookAt);
        }
    }
}
