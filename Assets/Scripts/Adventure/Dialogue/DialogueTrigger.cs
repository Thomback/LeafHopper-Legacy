using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to place on each NPC to control how they behave to player and which sentences to display in dialog
/// </summary>
public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private bool willTurnToPlayerWhenNearby = true;     // If true, npc will react to player if nearby
    [SerializeField]
    private bool willTurnToPlayerWhenTalkedTo = true;   // If true, npc will turn to player when talked to
    [SerializeField]
    private Animator animFlip;                          // Animator used to control the npc flip animation
    [SerializeField]
    private NPC_Controller myController;                // NPC movement controller reference
    [SerializeField]
    private DialogueManager myDialogueManager;          // DialogueManager reference
    [SerializeField]
    private Dialogue myDialogue;                        // Dialogue in sentences form of the NPC

    private bool readyToTalk = false;                   // Used to check if npc can be interacted with
    private Transform playerTransform;                  // Used to check current player transform
    
    /// <summary>
    /// In start, initialize some components not referenced in editor
    /// </summary>
    void Start()
    {
        if(animFlip == null)
        {
            animFlip = gameObject.GetComponentsInChildren<Animator>()[0];
        }
        if(myController == null)
        {
            myController = GetComponent<NPC_Controller>();
        }
        if (myDialogueManager == null)
        {
            myDialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        }


        playerTransform = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    /// <summary>
    /// If player collides with npc, npc is ready to talk
    /// </summary>
    /// <param name="collision">Object collision</param>
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            readyToTalk = true;
        }
    }

    /// <summary>
    /// If player exit collision with npc, npc no talking >:(
    /// </summary>
    /// <param name="collision">Object collision</param>
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            readyToTalk = false;
        }
    }

    /// <summary>
    /// On each frame, get the Fire1 buttonUp trigger.
    /// If npc is ready to talk, flip it to face player if willTurnToPlayerWhenTalkedTo is true,
    /// then start the dialogue using myDialogue variable.
    /// On each frame, also tries to turn to player if he is nearby and willTurnToPlayerWhenNearby is true.
    /// </summary>
    private void Update()
    {
        // Trigger talk
        if (readyToTalk && Input.GetButtonUp("Fire1"))
        {
            if (willTurnToPlayerWhenTalkedTo)
            {
                FlipFacingPlayer();
            }
            myDialogueManager.StartDialogue(myDialogue);

        }

        // Character flip
        if (willTurnToPlayerWhenNearby)
        {
            if (myController.isLastDirectionRight && playerTransform.position.x <= transform.position.x - 0.40 && readyToTalk)
            {
                FlipFacingPlayer();
            }
            else if (!myController.isLastDirectionRight && playerTransform.position.x >= transform.position.x + 0.40 && readyToTalk)
            {
                FlipFacingPlayer();
            }
        }

        if(!readyToTalk && !myController.canMove)
        {
            myController.canMove = true;
        }
    }

    /// <summary>
    /// Private function used to flip npc sprite to face player
    /// </summary>
    private void FlipFacingPlayer()
    {
        if(myController.isLastDirectionRight && playerTransform.position.x <= transform.position.x)
        {
            animFlip.SetTrigger("FlipLeft");
            myController.isLastDirectionRight = false;
            myController.canMove = false;
        }
        else if (!myController.isLastDirectionRight && playerTransform.position.x >= transform.position.x)
        {
            animFlip.SetTrigger("FlipRight");
            myController.isLastDirectionRight = true;
            myController.canMove = false;
        }
    }

}
