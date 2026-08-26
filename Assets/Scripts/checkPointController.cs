using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointController : MonoBehaviour
{
    public float verticalShift = 2.0f; 

    private Animator animator;
    private Vector2 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spawnPoint = transform.position;
        spawnPoint.y += verticalShift;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(tagsEnum.player.ToString()))
        {
            playerController2 player = collision.gameObject.GetComponent<playerController2>();
            if(collision!=player.circleCollider)
            {
                animator.SetBool("flagged_bool", true);
                player.resetSpawn(spawnPoint);
            }
        }
    }
}
