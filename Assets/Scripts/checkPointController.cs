using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class checkPointController : MonoBehaviour
{
    public float verticalShift = 2.0f; 

    private Animator animator;
    private Vector2 spawnPoint;
    private lightController lightController;
    private playerController2 player;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spawnPoint = transform.position;
        spawnPoint.y += verticalShift;
        lightController = GetComponent<lightController>();
        lightController.push(transform.Find("light1").GetComponent<Light2D>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(tagsEnum.player.ToString()))
        {
            if(player ==null)
                 player = collision.gameObject.GetComponent<playerController2>();
            if(collision!=player.circleCollider)
            {
                lightController.turnON();
                animator.SetBool("flagged_bool", true);
                player.resetSpawn(spawnPoint);
            }
        }
    }
}
