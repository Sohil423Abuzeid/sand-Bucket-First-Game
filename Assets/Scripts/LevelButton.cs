using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
public class LevelButton : MonoBehaviour
{
    public int level = 0;

    private MainMenuController mainMenuController;

    private Animator animator;
    private Light2D light2D;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuController = GameObject.FindFirstObjectByType<MainMenuController>();
        animator = GetComponent<Animator>();
        light2D = transform.Find("light1").gameObject.GetComponent<Light2D>();
        light2D.intensity = 0f;

        checkForLevelDone();
    }

    // Update is called once per frame
    void Update()
    {
        checkForLevelDone();
    }
    private void checkForLevelDone()
    {
        if (PlayerPrefs.GetInt(level.ToString(), 0) == 0) return;

        light2D.intensity = 2f;
        animator.SetBool("flagged_bool", true);
    }

    private void OnMouseDown()
    {
        SceneManager.LoadSceneAsync(level + mainMenuController.firstLevelIndex-1);
    }
}
