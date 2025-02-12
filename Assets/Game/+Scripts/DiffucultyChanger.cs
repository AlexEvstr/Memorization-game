using UnityEngine;

public class DiffucultyChanger : MonoBehaviour
{
    private SceneFader _sceneFader;

    private void Start()
    {
        _sceneFader = GetComponent<SceneFader>();
    }

    public void ChooseDifficulty(int difficultyIndex)
    {
        PlayerPrefs.SetInt("DifficultyLevel", difficultyIndex);
        _sceneFader.LoadSceneWithFade("game");
    }
}