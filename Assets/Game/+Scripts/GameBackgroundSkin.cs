using UnityEngine;
using UnityEngine.UI;

public class GameBackgroundSkin : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private Sprite[] _bgSprites;

    private void Awake()
    {
        int spriteIndex = PlayerPrefs.GetInt("SelectedSkin", 6);
        _background.sprite = _bgSprites[spriteIndex];
    }
}