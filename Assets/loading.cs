using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class loading : MonoBehaviour
{
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine(LoadLevel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator LoadLevel()
    {
        int sceneIndex = PlayerPrefs.GetInt("next", 1);
        yield return new WaitForSeconds(3);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            text.text = $"<color=#feae34>Loading {progress}% </color>";

            yield return null;
        }
    }
}
