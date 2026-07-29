using UnityEngine;
using UnityEngine.Apple.ReplayKit;

public class playerController : MonoBehaviour
{

    // boost setting 
    public float cooldownTime = 5f;
    private bool boost = true;

    //  jump logic
    public float jumpPower = 500;
    private bool doubleJump = true;
    private bool onGround = true;


    //Components
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
        
    }
    private void LateUpdate()
    {
        handelMove();
    }
    void handelMove()
    {

    }
    void jump()
    {
        if (onGround) onGround = false;
        else if (doubleJump) doubleJump = false;
        else return;

        rb.velocity = new Vector2(rb.velocity.x, 0);// for the second jump
        rb.AddForce(Vector2.up * jumpPower ,ForceMode2D.Impulse );
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagsEnum.ground.ToString()))
        {
            onGround = true;
            doubleJump = true;
        }
    }
}
