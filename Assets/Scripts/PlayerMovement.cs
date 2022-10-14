using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement components")]
    [Tooltip("Walking speed of the player")]
    [SerializeField] private float speed;
    [Tooltip("Amount of upward velocity applied to the player when jumping")]
    [SerializeField] private float jumpPower;
    [Tooltip("The maximum possible velocity for a player in all directions")]
    [SerializeField] private float terminalVelocity = 20.0f;
    [Tooltip("How high of a stair the player can climb")]
    [SerializeField] private float stairClimbHeight;
    [Tooltip("The layer for calculating whether or not the player is grounded or not")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip walkSound;

    private Rigidbody2D body;
    private BoxCollider2D box;
    private CapsuleCollider2D footCollider;
    private Animator anim;
    public float horizInput { get; private set; }
    private float initScaleX;
    private float scaleX;
    
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        footCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        initScaleX = transform.localScale.x;
    }

    // Update is called once per frame
    private void Update() {
        // horizontal movement
        horizInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizInput * speed, body.velocity.y);
        if(horizInput != 0 && isGrounded() && !SoundManager.instance.isPlayingSound(walkSound)) 
            SoundManager.instance.playSound(walkSound);

        // facing direction
        scaleX = horizInput != 0 ? (horizInput / Mathf.Abs(horizInput)) * initScaleX : transform.localScale.x;
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
/*
        // walking up stairs
        Vector2 stairPos = stairCheck();
        if(stairPos.y - box.bounds.min.y < stairClimbHeight && isGrounded())
            // maybe LERP this?
            transform.position = new Vector3(stairPos.x + (-Mathf.Sign(horizInput) * box.bounds.extents.x - 0.03f), 
                                             stairPos.y + box.bounds.extents.y + 0.07f, transform.position.z);
*/
        // jumping
        if(Input.GetKeyDown(KeyCode.Space))
            jump();

        // terminal velocity checks
        if(Mathf.Abs(body.velocity.x) > terminalVelocity)
            body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * terminalVelocity, body.velocity.y);
        if(Mathf.Abs(body.velocity.y) > terminalVelocity)
            body.velocity = new Vector2(body.velocity.x, Mathf.Sign(body.velocity.y) * terminalVelocity);
        
        // animations
        anim.SetBool("Moving", horizInput != 0);
        anim.SetBool("Grounded", isGrounded());
    }

    private bool isGrounded() {
        // boxcast to see if the ground is just under the player
        RaycastHit2D ray = Physics2D.BoxCast(footCollider.bounds.center, footCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return ray.collider != null;
    }

    private Vector2 stairCheck() {
        // boxcast to see if player's feet are touching the ground in the direction they're moving
        RaycastHit2D ray = Physics2D.BoxCast(new Vector2(footCollider.bounds.center.x, 
                                             footCollider.bounds.center.y + (stairClimbHeight / 2)), 
                                             footCollider.bounds.size, 0, Vector2.right * Mathf.Sign(horizInput), 0.1f, groundLayer);
        
        // if there is a collision and it has a box collider, return the closest top corner to the player
        if(ray.collider != null) {
            Collider2D stairCollider = ray.collider.GetComponent<Collider2D>();
            if(stairCollider != null) {
                float stairX = horizInput > 0 ? stairCollider.bounds.min.x : stairCollider.bounds.max.x;
                return new Vector2(stairX, stairCollider.bounds.max.y);
            }
        }
        return Vector2.positiveInfinity;
    }

    private bool downStairs() {
        // raycast to see if the ground is far enough under the player to be a possible stair
        RaycastHit2D ray = Physics2D.Raycast(footCollider.bounds.min * Mathf.Sign(horizInput), Vector2.down, 
                                             stairClimbHeight + 0.1f, groundLayer);
        return ray.collider != null;
    }

    private void jump() {
        if(isGrounded()) {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            SoundManager.instance.playSound(jumpSound);
        }
    }
/*
    private void OnDrawGizmos() {
        // draw stairCheck boxcast
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(footCollider.bounds.center + (Vector3.right * Mathf.Sign(horizInput) * 0.1f), 
                            footCollider.bounds.size);
        
    }
*/
}
