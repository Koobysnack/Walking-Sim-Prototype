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
    private Animator anim;
    private float horizInput;
    private float initScaleX;
    private float scaleX;
    
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
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

        // walking up stairs
        Vector2 stairPos = stairCheck();
        if(stairPos.y - box.bounds.min.y < stairClimbHeight && isGrounded())
            transform.position = new Vector3(stairPos.x + (-Mathf.Sign(horizInput) * box.bounds.extents.x - 0.01f), 
                                             stairPos.y + box.bounds.extents.y, transform.position.z);

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
        anim.SetBool("Grounded", isGrounded() || downStairs());
    }

    private bool isGrounded() {
        // boxcast to see if the ground is just under the player
        RaycastHit2D ray = Physics2D.BoxCast(box.bounds.center, box.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return ray.collider != null;
    }

    private Vector2 stairCheck() {
        // boxcast to see if player's feet are touching the ground in the direction they're moving
        RaycastHit2D ray = Physics2D.BoxCast(new Vector2(box.bounds.center.x, box.bounds.center.y + (stairClimbHeight / 2)), 
                                             box.bounds.size, 0, Vector2.right * Mathf.Sign(horizInput), 0.1f, groundLayer);
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
        RaycastHit2D ray = Physics2D.Raycast(box.bounds.min, Vector2.down, stairClimbHeight + 0.1f, groundLayer);
        return ray.collider != null;
    }

    private void jump() {
        if(isGrounded()) {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            SoundManager.instance.playSound(jumpSound);
        }
    }

    private void OnDrawGizmos() {
        // draw stairCheck boxcast
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(box.bounds.center + (Vector3.right * Mathf.Sign(horizInput) * 0.1f), 
                            box.bounds.size);
    }

}
