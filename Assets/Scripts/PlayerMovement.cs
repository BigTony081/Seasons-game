using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float ySpeed;
    [SerializeField] private float xSpeed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;


    private void Awake()
    {
        //Grab references from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        
        //Flip player when moving left or right
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(7, 7, 7);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-7, 7, 7);

        //Set animator parameters
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());

        //wall jump logic
        if (wallJumpCooldown < 0.2f)
        {
            if (Input.GetKey(KeyCode.Space) && isGrounded())
                Jump();

            body.velocity = new Vector2(horizontalInput * ySpeed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }

            else
                body.gravityScale = 3;


        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, xSpeed);
        anim.SetTrigger("Jump");

    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {  
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
