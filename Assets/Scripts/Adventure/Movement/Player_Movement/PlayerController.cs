using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody theRB;
    [SerializeField]
    private float moveSpeed, jumpForce;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private Transform groundPoint;
    [SerializeField]
    private Animator animSprite;
    [SerializeField]
    private Animator animFlip;
    [SerializeField]
    private float jumpBufferTime = 0.1f;
    [SerializeField]
    private ParticleSystem footDust;
    [SerializeField]
    private float footDustMaxRate = 10;

    private ParticleSystem.EmissionModule footDustEmission;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool wasGrounded = true;
    private bool isLastDirectionRight = true;
    private float jumpBuffer = -1;
    private int jumpDirection = 0;
    private bool canMove = true;

    private void Awake()
    {
        Physics.gravity *= 3;
        if(footDust)
            footDustEmission = footDust.emission;
    }


    void Update()
    {
        if (canMove)
        {
            // Input getters
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();
        }


        // Ground Checker
        RaycastHit hit;
        if (Physics.Raycast(groundPoint.position, Vector3.down, out hit, .25f, whatIsGround))
        {
            isGrounded = true;
            animSprite.SetBool("isGrounded", true);
        }
        else
        {
            isGrounded = false;
            animSprite.SetBool("isGrounded", false);
        }

        // Jump direction checker
        if(theRB.velocity.y>0.1f && jumpDirection!= 1 && !isGrounded)
        {
            jumpDirection = 1;
            animSprite.SetTrigger("jumpStart");
        }else if(theRB.velocity.y<-0.1f && jumpDirection != -1 && !isGrounded)
        {
            jumpDirection = -1;
            animSprite.SetTrigger("jumpEnd");
        }
        else if(theRB.velocity.y < 0.1f && theRB.velocity.y > -0.1f)
        {
            jumpDirection = 0;
        }

        // -- Jump getter --
        if (Input.GetButtonDown("Jump"))    // Reset buffer on key down
        {
            jumpBuffer = jumpBufferTime;
        }
        else if(jumpBuffer > 0)
        {
            jumpBuffer -= Time.deltaTime;
        }
        if (Input.GetButtonUp("Jump") && theRB.velocity.y > 0)   // Jump shortener
        {
            theRB.velocity += new Vector3(0f, -theRB.velocity.y / 2, 0f);
        }

        // Foot dust manager
        if (Input.GetAxisRaw("Horizontal") != 0 && isGrounded == true)
        {
            // calcul de la valeur absolue de l'input pour générer des particules
            footDustEmission.rateOverTime = Mathf.Abs(footDustMaxRate * Input.GetAxisRaw("Horizontal"));
        }
        else
        {
            footDustEmission.rateOverTime = 0;
        }
        if(!wasGrounded && isGrounded)  // Emit dust on landing
        {
            footDust.Emit(50);
        }

        // Character flip
        if(isLastDirectionRight && moveInput.x < 0)
        {
            animFlip.SetTrigger("FlipLeft");
            isLastDirectionRight = false;
        }else if(!isLastDirectionRight && moveInput.x > 0)
        {
            animFlip.SetTrigger("FlipRight");
            isLastDirectionRight = true;
        }

        wasGrounded = isGrounded;
    }

    private void FixedUpdate()
    {
        // Horizontal movement
        theRB.velocity = new Vector3(moveInput.x * moveSpeed, theRB.velocity.y, moveInput.y * moveSpeed);
        animSprite.SetFloat("moveSpeed", theRB.velocity.magnitude);

        // Jump movement
        if (jumpBuffer >= 0 && isGrounded)
        {
            theRB.velocity = new Vector3(theRB.velocity.x, jumpForce, theRB.velocity.z);
            jumpBuffer = -1;
        }
    }

    public void setControl(bool status)
    {
        canMove = status;
        if (!status) {
            moveInput = Vector2.zero;
        }
    }
}
