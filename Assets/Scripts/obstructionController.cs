using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using System;

public class obstructionController : MonoBehaviour
{
    public float apearSpeed = .02f;
    public float minIntensity = .75f;
    public float maxIntensity = 1.5f;
    public float lightChangeSpeed = .05f;

    private BoxCollider2D collider2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color color;
    private Light2D light2D;
    private int lighting = 0;
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

        light2D = transform.Find("light").GetComponent<Light2D>();
        light2D.intensity = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        lightHandeller();
    } 

    private void lightHandeller()
    {
        if (lighting != 1) return;

        if (light2D.intensity > maxIntensity&& lightChangeSpeed>0)
            lightChangeSpeed *= -1;
        
        if (light2D.intensity < minIntensity&& lightChangeSpeed<0)
            lightChangeSpeed *= -1;

        light2D.intensity += lightChangeSpeed;
    }
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagsEnum.playerzone.ToString()))
        {
            color.a = 1;
            if (lighting == 0)
            {
                lighting++;
            }
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
            lighting++;
            light2D.intensity = 0f;

            if (!player.isFacingRight)
                transform.Rotate(0f, 180f, 0f);

            animator.SetTrigger("break_trigger");


            Destroy(gameObject, 5f);
        }
    }
}
