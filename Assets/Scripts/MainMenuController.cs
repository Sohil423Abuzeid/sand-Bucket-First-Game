using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    public int firstLevelIndex = 4;
    //test
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void levels()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void setting()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void credits()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void exit()
    {
        Application.Quit();
    }
}
