using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWindows : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("game");
    }
}