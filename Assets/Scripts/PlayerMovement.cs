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
    public float horizInput { get; private set; }
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

<<<<<<< Updated upstream
=======
        // walking up stairs
        Vector2 stairPos = stairCheck();
        if(stairPos.y - box.bounds.min.y < stairClimbHeight && isGrounded())
            // maybe LERP this?
            transform.position = new Vector3(stairPos.x + (-Mathf.Sign(horizInput) * box.bounds.extents.x - 0.01f), 
                                             stairPos.y + box.bounds.extents.y, transform.position.z);

>>>>>>> Stashed changes
        // jumping
        if(Input.GetKeyDown(KeyCode.Space)) {
            jump();
        }

        // animations
        anim.SetBool("Moving", horizInput != 0);
<<<<<<< Updated upstream
        anim.SetBool("Grounded", isGrounded());
=======
        anim.SetBool("Grounded", isGrounded() || downStairs());

        if(Input.GetKeyDown(KeyCode.F))
            transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
/*
    private void OnDrawGizmos() {
        // draw stairCheck boxcast
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(box.bounds.center + (Vector3.right * Mathf.Sign(horizInput) * 0.1f), 
                            box.bounds.size);
    }
*/
>>>>>>> Stashed changes
}
