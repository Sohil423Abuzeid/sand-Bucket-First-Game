using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class obstructionController : MonoBehaviour
{
    private BoxCollider2D collider2D;
    // Start is called before the first frame update
    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(tagsEnum.player.ToString()))
        {
            playerController player= collision.gameObject.GetComponent<playerController>();

            if (!player.boosted) return;

            collider2D.enabled = false;

            //animation 

            Destroy(this.gameObject);
        }
    }
}
