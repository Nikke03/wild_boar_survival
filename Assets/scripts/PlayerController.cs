using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("Jump System")]
    [SerializeField] float JumpTime;
    [SerializeField] float jumpMoltiplier;
    public int speed;
    [SerializeField] int jumpPower;
    [SerializeField] int fallMultiplier;
    public bool Correre;
    public bool gigi;
    private Health vita;
    AudioManager audioManager;


    public Transform groundCheck;
    public LayerMask groundLayer;
    Vector2 vecGravity;
    public Vector2 vecMove;

    MovementState state;

    bool isJumping;
    bool isRunning;
    float jumpCounter;

    
    private enum MovementState { idle, run, jumping, falling, running }

    private Animator anim;

    private void Start()
    {
        vita = GetComponent<Health>(); // Ottieni il riferimento al componente PlayerController
        anim = GetComponent<Animator>();
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.linearVelocity.y < -.1f)
        {
            rb.linearVelocity -= vecGravity * fallMultiplier * Time.deltaTime;
            //state = MovementState.falling;
        }


        UpdateAnimationState();



    }

    public void Jump(InputAction.CallbackContext value)
    {
       

        if (value.started && isGrounded())
        {
            
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            //state = MovementState.jumping;
           
            isJumping = true;
            jumpCounter = 0;
            //audioManager.PlaySFX(audioManager.jump);
        }

        if (value.performed)
        {

            isJumping = false;
            //state = MovementState.falling;

        }

        //anim.SetInteger("state", (int)state);
    }

    public void Movement(InputAction.CallbackContext value)
    {
      
            //state = MovementState.running;
            vecMove = value.ReadValue<Vector2>();
            //flip();
            //anim.SetInteger("state", (int)state);
        
    }

    private void FixedUpdate()
    {
        if(Correre == false && gigi== false)
        rb.linearVelocity = new Vector2(vecMove.x * speed, rb.linearVelocity.y); // vel normale
        else if(Correre == true && gigi==false)
        rb.linearVelocity = new Vector2(vecMove.x * speed * 2, rb.linearVelocity.y); //vel doppia
        else if (gigi == true && Correre == false)
            rb.linearVelocity = new Vector2(vecMove.x * speed * 2.5f , rb.linearVelocity.y); //vel doppia
        else if (gigi == true && Correre == true)
            rb.linearVelocity = new Vector2(vecMove.x * speed * 2.5f, rb.linearVelocity.y); //vel doppia
    
}

   // void flip()
   // {
     //   if (vecMove.x < -0.01f) transform.localScale = new Vector3(-1, 1, 1);
     //   if (vecMove.x > 0.01f) transform.localScale = new Vector3(1, 1, 1);
 //   }



    bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.18f, 0.86f), CapsuleDirection2D.Vertical, 0, groundLayer);
    }

    public void Running(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            Correre = true;
        }
        else if (value.canceled || value.performed) // Controlla sia "canceled" che "performed"
        {
            Correre = false;
        }
    }

    public void Invincible(InputAction.CallbackContext value)
    {
        if (value.started && vita.immortale)
        {
            vita.AttivaImmortalita();
            gigi = true;
        }
        
    }


    private void UpdateAnimationState()
    {
        MovementState state;

        if ((vecMove.x > 0.01f) && (Correre == false) )
        {
            state = MovementState.run;
            transform.localScale = new Vector3(1, 1, 1);

        }
        else if((vecMove.x < -0.01f) && (Correre == false) )
        {
            state = MovementState.run;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if ((vecMove.x > 0.01f) && Correre)
        {
            state = MovementState.running;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if ((vecMove.x < -0.01f) && Correre)
        {
            state = MovementState.running;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        
        else
        {
            state = MovementState.idle;
        }

        if (rb.linearVelocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.linearVelocity.y < -.11f )
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

}
