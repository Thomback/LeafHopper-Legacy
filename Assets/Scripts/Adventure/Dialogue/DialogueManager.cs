using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manager class used to control the dialog behavior and display
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController thePlayerController;       // Reference to the player controller
    [SerializeField]
    private Transform cameraDefaultPosition;            // Camera position when playing
    [SerializeField]
    private Transform cameraDialoguePosition;           // Camera position when zoomed in for dialog
    [SerializeField]
    private Transform cameraLookAt;                     // Transform to look at
    [SerializeField]
    private float skipDelay = 0.25f;                    // Little delay to skip phrase to let the text display properly
    [SerializeField]
    private Canvas myCanvas;                            // Canvas to show when displaying dialog
    [SerializeField]
    private TMP_Text dialogueDisplayer;                 // Text element to modify

    private Queue<string> sentences;                    // Queue of sentences that will be displayed
    private CameraLerp cameraLerp;                      // CameraLerp component to ease out position transition
    private bool inConversation = false;                // Is the player conversing
    private float skipDelayTimer = 0f;                  // SkipDelay timer tracker

    /// <summary>
    /// On Start, get all component that have not been initialized
    /// </summary>
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

    /// <summary>
    /// On each frame, get the Fire1 button up trigger to display the next sentence if the timeDelay
    /// has passed since last sentence skip.
    /// </summary>
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

    /// <summary>
    /// Public function called from a NPC to start a dialog
    /// </summary>
    /// <param name="dialogue">Dialog class parameter containing all sentences to display</param>
    public void StartDialogue(Dialogue dialogue)
    {
        if (!inConversation) {                  // Only triggers if not already in another conversation
            inConversation = true;
            DialogueCamera(true);               // Changes camera position
            sentences.Clear();                  // Clear old dialog just in case

            foreach(string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);    // Add all sentences stored in dialogue inside a sentence variable
            }
            
            DisplayNextSentence();              // Display the first sentence
            myCanvas.enabled = true;            // Toggle the canvas visibility
            thePlayerController.setControl(false);// Disable player movement
        }
    }

    /// <summary>
    /// Updates the displayed sentence text
    /// </summary>
    private void DisplayNextSentence()
    {
        if (sentences.Count == 0)   // If dialog end reached, terminate dialog
        {
            EndDiaolgue();
            return;
        }
        string sentence = sentences.Dequeue();  // Removes the first sentence in queue and set the current sentence to the next
        dialogueDisplayer.SetText(sentence);
    }

    /// <summary>
    /// Terminates dialog
    /// </summary>
    private void EndDiaolgue()
    {
        inConversation = false;                 // Not in conversation anymore
        dialogueDisplayer.SetText("");          // Clear displayed text
        DialogueCamera(false);                  // Set camera to default location
        myCanvas.enabled = false;               // Untoggle the canvas visibility
        thePlayerController.setControl(true);   // Enable player movement
    }

    /// <summary>
    /// Lerps the camera location to the current mode and update the lookat of the camera
    /// </summary>
    /// <param name="setDialogueMode">true if dialog mode</param>
    public void DialogueCamera(bool setDialogueMode)
    {
        if (setDialogueMode)
        {
            Transform newLookAt = thePlayerController.transform;        // Not implemented yet, might use later if needed
            cameraLerp.CameraLerping(cameraDialoguePosition.localPosition ,cameraLookAt);   // Lerp to dialog mode
        }
        else
        {
            cameraLerp.CameraLerping(cameraDefaultPosition.localPosition, cameraLookAt);    // Lerp to classic mode
        }
    }
}
