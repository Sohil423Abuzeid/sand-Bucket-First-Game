using UnityEngine;
using UnityEngine.Apple.ReplayKit;
using UnityEngine.Assertions.Must;

public class playerController : MonoBehaviour
{

    public float speed = 10f;

    // boost setting 
    public float cooldownTime = 5f;
    private bool boost = true;

    //  jump logic
    public float jumpPower = 500;
    public int maxJumps = 2;
    int jumpsRemaining;

    // dirctions 
    private float horizontal = 0;


    //Components
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsRemaining = maxJumps;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        horizontal = Input.GetAxisRaw(InputAxiss.Horizontal);
    }
    private void FixedUpdate()
    {
        HandleMove();
    }
    private void LateUpdate()
    {

    }
    void HandleMove()
    {
        rb.velocity = new Vector2( horizontal*speed,rb.velocity.y);
    }
    void Jump()
    {
        if (jumpsRemaining == 0) return;

        jumpsRemaining--;

        rb.velocity = new Vector2(rb.velocity.x, 0);// for the second jump
        rb.AddForce(Vector2.up * jumpPower ,ForceMode2D.Impulse );
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagsEnum.ground.ToString()))
        {
            jumpsRemaining = maxJumps;
        }
    }
}
