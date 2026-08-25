using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voidController : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;
    // Start is called before the first frame update
    private void Awake()
    {
        gameObject.tag = tagsEnum.voidlimit.ToString();
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        hide();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void hide()
    {
        Color color = SpriteRenderer.color;
        color.a = 0f;
        SpriteRenderer.color = color;
    }
}
