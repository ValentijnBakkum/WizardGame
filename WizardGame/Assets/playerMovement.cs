using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public float speed = 10;

    [Range(1,20)]
    public float jumpVelocity = 10;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private bool onGround;
    public Vector2 bottomOffset, rightOffset, leftOffset;
    public float collisionRadius = 0.25f;

    [Header("Layers")]
    public LayerMask groundLayer;

    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;
    public float slideSpeed = 5;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer) || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);    

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Debug.Log(x);
        Vector2 dir = new Vector2(x, y);

        if(Input.GetKeyDown(KeyCode.Space) && onGround){
            Jump();
        }

        if(onWall && !onGround && rb.velocity.y < 0.1)
        {
            if (x != 0)
            {
                WallSlide();
            }
        }

        if(rb.velocity.y < 0){
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)){
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        Walk(dir);
    }

    private void Walk(Vector2 dir){
        rb.velocity = (new Vector2(dir.x * speed, rb.velocity.y));

        if(dir != Vector2.zero){
            animator.SetFloat("moveX", dir.x);
            animator.SetFloat("moveY", dir.y);
            animator.SetBool("moving", true);
        }else{
            animator.SetBool("moving", false);
        }
            
    }

    private void Jump(){
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += Vector2.up * jumpVelocity;
    }

    private void WallSlide()
    {
        rb.velocity = new Vector2(0, -slideSpeed);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}
