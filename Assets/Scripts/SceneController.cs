using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] bool isActive = false;
    private float a = 0f;
    void Update()
    {
        if(isActive)
        {
            a += Time.deltaTime;
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Application.Quit();
                }
                else if (a > 1f)
                {
                    LoadingSceneManager.LoadScene("GameScene");
                }
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadScene(string s)
    {
        SceneManager.LoadScene(s);
    }
}
