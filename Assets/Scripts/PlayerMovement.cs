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
    [Tooltip("The layer for calculating whether or not the player is grounded or not")]
    [SerializeField] private LayerMask groundLayer;
/*    [Tooltip("The layer for calculating whether or not the player is on a slope or not")]
    [SerializeField] private LayerMask slopeLayer;
    [Tooltip("The physics material to keep the player from sliding down a slope when idle")]
    [SerializeField] private PhysicsMaterial2D fullFrictionMaterial;*/

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
    private void Update()
    {
        // horizontal movement
        horizInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizInput * speed, body.velocity.y);
        if(horizInput != 0 && isGrounded() && !SoundManager.instance.isPlayingSound(walkSound)) 
            SoundManager.instance.playSound(walkSound);

        // facing direction
        scaleX = horizInput != 0 ? (horizInput / Mathf.Abs(horizInput)) * initScaleX : transform.localScale.x;
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);

        // jumping
        if(Input.GetKeyDown(KeyCode.Space)) {
            jump();
        }

        // terminal velocity checks
        if(Mathf.Abs(body.velocity.x) > terminalVelocity)
            body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * terminalVelocity, body.velocity.y);
        if(Mathf.Abs(body.velocity.y) > terminalVelocity)
            body.velocity = new Vector2(body.velocity.x, Mathf.Sign(body.velocity.y) * terminalVelocity);
/*
        if(slopeCheck() && isIdle()) {
            body.velocity = Vector2.zero;
            body.sharedMaterial = fullFrictionMaterial;
        }
        else
            body.sharedMaterial = null;
*/
        // animations
        anim.SetBool("Moving", horizInput != 0);
        anim.SetBool("Grounded", isGrounded());
    }

    private bool isGrounded() {
        RaycastHit2D ray = Physics2D.BoxCast(box.bounds.center, box.bounds.size, 0, Vector2.down, 0.1f, groundLayer);// + slopeLayer);
        return ray.collider != null;
    }
/*
    private bool slopeCheck() {
        RaycastHit2D ray = Physics2D.Raycast(box.bounds.center, Vector2.down, 5f, slopeLayer);
        if(ray != false) {
            Vector2 temp = transform.position;
            temp.y = transform.position.y;
            transform.position = temp;
            return true;
        }
        return false;
    }

    private bool isIdle() {
        if(horizInput == 0 && isGrounded() && !Input.GetKeyDown(KeyCode.Space))
            return true;
        return false;
    }
*/
    private void jump() {
        if(isGrounded()) {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            SoundManager.instance.playSound(jumpSound);
        }
    }
}
