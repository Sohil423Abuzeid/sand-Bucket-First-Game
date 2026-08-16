using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class playerController : MonoBehaviour
{

    public float actualSpeed;
    public float bulsePower = 5f;
    // boost setting 
    public float normalSpeed = 10f;
    public float cooldownTime = 10f;
    public float boostTime = 5f;
    private bool boost = true;
    public bool boosted = false;
    public float boostSpeed = 30f;


    //  jump logic
    public float jumpPower = 5;
    public int maxJumps = 2;
    public int jumpsRemaining = 0;

    // dirctions 
    private float horizontal = 0;
    public float lastGroundY = 0;

    //Components
    private Rigidbody2D rb;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpsRemaining = maxJumps;
        actualSpeed = normalSpeed;
        lastGroundY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("velocity_float",rb.velocity.y);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Jump());
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(startBoost());
        }
        horizontal = Input.GetAxisRaw(InputAxiss.Horizontal);
        dirctionAndAnimation();
    }
    private void FixedUpdate()
    {
        HandleMove();
    }
    private void LateUpdate()
    {


    }
    IEnumerator startBoost()
    {
        if (!boost) yield break;

        boost = !boost;
        boosted = !boosted;
        actualSpeed = boostSpeed;

        yield return new WaitForSeconds(boostTime);

        actualSpeed = normalSpeed;

        yield return new WaitForSeconds(cooldownTime - boostTime);

        boost = !boost;
        boosted = !boosted;
    }
    void dirctionAndAnimation()
    {
        if (horizontal != 0)
        {
            animator.SetBool("moving_bool", true);
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * horizontal;
            transform.localScale = scale;
        }
        else
            animator.SetBool("moving_bool", false);
    }
    void HandleMove()
    {
        rb.velocity = new Vector2(horizontal * actualSpeed, rb.velocity.y);
        
    }
    IEnumerator Jump()
    {
        if (jumpsRemaining == 0) yield break;
        animator.SetTrigger("jump_trigger");
        jumpsRemaining--;

        yield return new WaitForSeconds(.280f);

        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(0, rb.velocity.y));// for the second jump
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagsEnum.ground.ToString())&& collision.contacts[0].normal.y > 0.5f)
        {
            lastGroundY = transform.position.y;
            jumpsRemaining = maxJumps;
            //rb.velocity = new Vector2(rb.velocity.x,0);
            //rb.AddForce(Vector2.up * bulsePower, ForceMode2D.Impulse);
        }
    }

}
