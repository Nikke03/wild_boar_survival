using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovements : MonoBehaviour
{

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    //private InputProviders _inputProviders;

    [SerializeField] private LayerMask jumpableGround;

    private float driX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private enum MovementState { idle, running, jumping, falling }


    [SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }



    // Update is called once per frame
    private void Update()
    {
        driX = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(driX * moveSpeed, rb.linearVelocity.y);


        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSoundEffect.Play();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        UpdateAnimationState();
    }



    private void UpdateAnimationState()
    {
        MovementState state;

        if (driX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (driX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.linearVelocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if(rb.linearVelocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }


    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    
}
