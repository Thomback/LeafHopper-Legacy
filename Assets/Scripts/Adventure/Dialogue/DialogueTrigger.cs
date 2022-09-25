using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private bool willTurnToPlayerWhenNearby = true;
    [SerializeField]
    private bool willTurnToPlayerWhenTalkedTo = true;
    [SerializeField]
    private Animator animFlip;
    [SerializeField]
    private NPC_Controller myController;
    [SerializeField]
    private DialogueManager myDialogueManager;
    [SerializeField]
    private Dialogue myDialogue;

    private bool readyToTalk = false;
    private Transform playerTransform;
    
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

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            readyToTalk = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            readyToTalk = false;
        }
    }

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
