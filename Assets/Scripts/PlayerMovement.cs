using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement components")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
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

        // animations
        anim.SetBool("Moving", horizInput != 0);
        anim.SetBool("Grounded", isGrounded());
    }

    private bool isGrounded() {
        RaycastHit2D ray = Physics2D.BoxCast(box.bounds.center, box.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return ray.collider != null;
    }

    private void jump() {
        if(isGrounded()) {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            SoundManager.instance.playSound(jumpSound);
        }
    }
}
