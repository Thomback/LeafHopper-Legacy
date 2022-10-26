using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC Movement and sprite flip
/// </summary>
public class NPC_Controller : MonoBehaviour
{
    private enum MovementType                   // Enum to quickly select the movement """IA""" of the npc
    {
        Immobile,
        ImmobileCurious,
        YoyoHorizontal,
        YoyoVertical
    }

    [SerializeField]
    private float moveSpeed = 5;                // Npc movement speed, default to 5
    [SerializeField]
    private MovementType myMovementType = MovementType.ImmobileCurious; // Movement type used
    [SerializeField]
    private Animator animSprite;                // General npc Animator
    [SerializeField]
    private Animator animFlip;                  // npc flip only Animator
    [SerializeField]
    private Rigidbody theRB;                    // npc's rigidbody reference

    [HideInInspector]
    public bool isLastDirectionRight = true;    // Check which direction faces ma boy
    [HideInInspector]
    public bool canMove = true;                 // Used to lock npc movement
    private Vector2 moveInput;
    private float patternTimer, patternAleaModifier;
    private int patternStep = 0;
    private Transform initialTransform;


    /// <summary>
    /// On Start, get some component references user forgot to link in editor
    /// </summary>
    void Start()
    {
        if(theRB == null)
            theRB = GetComponent<Rigidbody>();


        initialTransform = transform;
        isLastDirectionRight = true;
    }

    /// <summary>
    /// On each frame, move the npc according to its movementType strategy
    /// </summary>
    void Update()
    {
        // Pattern manager
        if (canMove)
        {
            patternTimer += Time.deltaTime;
            switch (myMovementType)
            {
                case MovementType.Immobile:
                    break;

                case MovementType.ImmobileCurious:
                    if (patternStep == 0 && patternTimer >= 0) {
                        AdvancePattern(-2f, 2f);
                        FlipSprite();
                    }
                    else if(patternStep == 1 && patternTimer+patternAleaModifier >= 5f){
                        AdvancePattern(-2f, 2f);
                        FlipSprite();
                    }else if(patternStep == 2 && patternTimer + patternAleaModifier >= 10f)
                    {
                        ResetPattern(-2f, 2f);
                    }
                    break;

                case MovementType.YoyoHorizontal:
                    if(patternStep == 0 && patternTimer >= 0)
                    {
                        AdvancePattern(-3f, 3f);
                    }else if(patternStep == 1 && patternTimer + patternAleaModifier >= 7)
                    {
                        AdvancePattern();
                        if (Random.Range(0, 2) == 0)
                            moveInput.x = 0.5f;
                        else
                            moveInput.x = -0.5f;
                       
                    }else if (patternStep == 2 && patternTimer + patternAleaModifier >= 11 || patternStep == 2 && transform.position.x >= initialTransform.position.x + 10)
                    {
                        break;//finish movement
                    }
                    break;

                case MovementType.YoyoVertical:
                    break;
                default:
                    break;
            }
        }

        moveInput.Normalize();                          // Normalize the movement so the movement is not faster in diagonal

        // Character flip
        if (isLastDirectionRight && moveInput.x < 0)
        {
            FlipSprite();
        }
        else if (!isLastDirectionRight && moveInput.x > 0)
        {
            FlipSprite();
        }


    }

    private void FixedUpdate()
    {
        // Horizontal movement
        theRB.velocity = new Vector3(moveInput.x * moveSpeed, theRB.velocity.y, moveInput.y * moveSpeed);
        animSprite.SetFloat("moveSpeed", theRB.velocity.magnitude);
    }


    #region private functions
    /// <summary>
    /// Character sprite flip depending on previous flip duration
    /// </summary>
    private void FlipSprite()
    {
        if (isLastDirectionRight)
        {
            isLastDirectionRight = false;
            animFlip.SetTrigger("FlipLeft");
        }
        else if (!isLastDirectionRight)
        {
            isLastDirectionRight = true;
            animFlip.SetTrigger("FlipRight");
        }
    }
    /// <summary>
    /// Force character sprite flip in a direction
    /// </summary>
    /// <param name="isFlippingLeft">If true, will flip left. If False, will flip right.</param>
    private void FlipSprite(bool isFlippingLeft)
    {
        if (isFlippingLeft)
        {
            isLastDirectionRight = false;
            animFlip.SetTrigger("FlipLeft");
        }
        else if (!isFlippingLeft)
        {
            isLastDirectionRight = true;
            animFlip.SetTrigger("FlipRight");
        }
    }

    /// <summary>
    /// Advance a step in NPC's pattern and set a new random alea modifier
    /// </summary>
    /// <param name="aleaMinValue">Pattern time modifier minimum value</param>
    /// <param name="aleaMaxValue">Pattern time modifier maximum value</param>
    private void AdvancePattern(float aleaMinValue,float aleaMaxValue)
    {
        patternStep++;
        patternAleaModifier = Random.Range(aleaMinValue, aleaMaxValue);
    }
    /// <summary>
    /// Advance a step in NPC's pattern and set the alea modifier to 0
    /// </summary>
    private void AdvancePattern()
    {
        patternStep++;
        patternAleaModifier = 0f;
    }

    /// <summary>
    /// Reset the step in NPC's pattern and set a new random alea modifier
    /// </summary>
    /// <param name="aleaMinValue">Pattern time modifier minimum value</param>
    /// <param name="aleaMaxValue">Pattern time modifier maximum value</param>
    private void ResetPattern(float aleaMinValue, float aleaMaxValue)
    {
        patternStep = 0;
        patternTimer = 0;
        patternAleaModifier = Random.Range(aleaMinValue, aleaMaxValue);
    }
    /// <summary>
    /// Reset the step in NPC's pattern and set the alea modifier to 0
    /// </summary>
    private void ResetPattern()
    {
        patternStep = 0;
        patternTimer = 0;
        patternAleaModifier = 0f;
    }
    #endregion

}
