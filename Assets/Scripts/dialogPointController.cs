using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class dialogPointController : MonoBehaviour
{
    public List<string> dialogs = new List<string>();
    
    private playerController2 playerController;

    private bool Working= false;
    private int pageIndex = 0;

    private GameObject banel;
    private Text main;
    private Text right;
    private Text left;

    private string leftMid = "<color=#feae34>Next (D)</color>";
    private string leftEnd = "<color=#feae34>Exit (D)</color>";

    private string rightMid = "<color=#feae34>Back (A)</color>";

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindAnyObjectByType<playerController2>();
        
        var texts = GameObject.FindObjectsByType<Text>(FindObjectsInactive.Include,FindObjectsSortMode.None).ToList();
        foreach(Text text in texts)
        {
            if(text.text== "<color=#feae34>next (D)</color>")
            {
                left = text;
            }
            else if (text.text == "<color=#feae34>back (A)</color>")
            {
                right = text;
            }
            else
            {
                main = text;
            }
        }
        banel = main.transform.parent.gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        updateDialogs();
    }
    private void updateDialogs ()
    {
        if (!Working) return;
        
        if (Input.GetKeyDown(KeyCode.A))
            pageIndex = Mathf.Max(0, pageIndex - 1);
        else if (Input.GetKeyDown(KeyCode.D))
            pageIndex++;

        if (pageIndex >= dialogs.Count()) Destroy(gameObject);

        if (pageIndex == 0)
            right.text = "";
        else
            right.text = rightMid;

        if (pageIndex == dialogs.Count() - 1)
            left.text = leftEnd;
        else
            left.text = leftMid;

        main.text = dialogs[pageIndex];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagsEnum.player.ToString()))
        {
            if (collision != playerController.circleCollider)
            {
                playerController.dialog = true;
                Working = true;
                banel.SetActive(true);
            }
        }
    }
    private void OnDestroy()
    {
        playerController.dialog = false;
        banel.SetActive(false);
    }
}
