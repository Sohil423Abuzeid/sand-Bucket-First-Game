using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goup : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -80)
            transform.Translate(new Vector3(0,200f, 0));
    }
}
