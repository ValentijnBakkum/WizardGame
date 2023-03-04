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
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(x, y);

        if(Input.GetKeyDown(KeyCode.Space)){
            Jump();
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
}
