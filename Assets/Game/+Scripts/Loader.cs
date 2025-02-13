using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(OpenMenuScene());
    }

    private IEnumerator OpenMenuScene()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("menu");
    }
}