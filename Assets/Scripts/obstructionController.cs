using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class obstructionController : MonoBehaviour
{
    public float apearSpeed = .02f;

    private BoxCollider2D collider2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color color;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = tagsEnum.obstruction.ToString();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    } 
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagsEnum.playerzone.ToString()))
        {
            color.a = 1;
            spriteRenderer.color = color;
            animator.SetTrigger("spawn_trigger");
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        dashHitCheck(collision);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        dashHitCheck(collision);
    }
    private void dashHitCheck(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagsEnum.player.ToString()))
        {
            playerController2 player = collision.gameObject.GetComponent<playerController2>();

            if (!player.isDashing) return;

            collider2D.enabled = false;

            if (!player.isFacingRight)
                transform.Rotate(0f, 180f, 0f);

            animator.SetTrigger("break_trigger");


            Destroy(gameObject, 5f);
        }
    }
}
