using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Image progressBar;
    [SerializeField] Image fade;
    bool isDone = false;
    float time = 0f;

    private void Start()
    {
        //StartCoroutine(LoadScene());
    }
    
    private void Update()
    {
        if(true)
        {
            time += Time.deltaTime;
            Color a = fade.color;
            a.a += 0.01f;
            fade.color = a;
            progressBar.fillAmount = time / 2f;
            if (time >= 2f)
            {
                SceneManager.LoadScene(nextScene);
            }
        }
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;

        while (!(op.isDone&& isDone))
        {
            yield return null;

            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    //yield break;
                }
            }
        }
    }
}
