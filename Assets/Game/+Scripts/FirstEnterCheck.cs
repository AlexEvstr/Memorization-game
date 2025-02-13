using UnityEngine;

public class FirstEnterCheck : MonoBehaviour
{
    [SerializeField] private RectTransform _rulesPopup;
    private PopupEffect _popupEffect;

    private void Start()
    {
        _popupEffect = GetComponent<PopupEffect>();
        int enter = PlayerPrefs.GetInt("FirstEnteControl", 0);
        if (enter == 0) _popupEffect.OpenWindow(_rulesPopup);
    }

    public void ChangeEnterStatus()
    {
        _popupEffect.CloseWindow(_rulesPopup);
        PlayerPrefs.SetInt("FirstEnteControl", 1);
    }
}
