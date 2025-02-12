using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void LoadSceneWithFade(string sceneName)
    {
        Time.timeScale = 1;
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    IEnumerator FadeIn()
    {
        Time.timeScale = 1;
        float timer = fadeDuration;
        Color color = fadeImage.color;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            color.a = Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.raycastTarget = false;
    }

    IEnumerator FadeOutAndLoad(string sceneName)
    {
        Time.timeScale = 1;
        float timer = 0f;
        Color color = fadeImage.color;
        fadeImage.raycastTarget = true;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}