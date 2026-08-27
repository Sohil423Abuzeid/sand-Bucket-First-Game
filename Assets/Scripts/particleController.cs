using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class particleController : MonoBehaviour
{
    private GameObject rightPostion;
    private GameObject leftPostion;

    private GameObject right;
    private GameObject left;

    public GameObject perfab;

    private GameObject player;
    private float playerPostion;

    private particleController anyOne;
    
    public bool Leader = false;

    private float width;
    // Start is called before the first frame update
    void Start()
    {
        rightPostion = transform.Find("right").gameObject;
        leftPostion = transform.Find("left").gameObject;
        player = GameObject.FindGameObjectsWithTag(tagsEnum.player.ToString()).First().gameObject;
        //width = rightPostion.GetComponent<SpriteRenderer>().bounds.size.x * .60f;
        width = 11f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Leader) return;
        handelSides();
        handelLeader();
    }
    private void handelSides()
    {
        if(left == null)
        {
            left = Instantiate(perfab, leftPostion.transform.position, leftPostion.transform.rotation);
            anyOne = left.GetComponent<particleController>();
            anyOne.right = this.gameObject;
        }

        if (right == null)
        {
            right = Instantiate(perfab, rightPostion.transform.position, rightPostion.transform.rotation);
            anyOne = right.GetComponent<particleController>();
            anyOne.left = this.gameObject;
        }
    }
    private void handelLeader()
    {
        playerPostion = player.transform.position.x;
        
        if(playerPostion >= transform.position.x + width)
        {
            Leader = false;
            anyOne = right.GetComponent<particleController>();
            anyOne.Leader = true;
            Destroy(left);
        }

        if (playerPostion <= transform.position.x - width)
        {
            Leader = false;
            anyOne = left.GetComponent<particleController>();
            anyOne.Leader = true;
            Destroy(right);
        }
    }

}
