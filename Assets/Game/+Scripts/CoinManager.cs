using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    private int _roundCoins;
    private int _totalCoins;

    [SerializeField] private Text _roundCoinsTextWin;
    [SerializeField] private Text _roundCoinsTextLose;

    private void Start()
    {
        _roundCoins = 0;
        _totalCoins = PlayerPrefs.GetInt("TotalGameCoins", 0);
    }

    public void AddRandomCoins()
    {
        int randomCoins = Random.Range(5, 21) * 5;
        _roundCoins += randomCoins;

        _roundCoinsTextWin.text = _roundCoins.ToString();
        _roundCoinsTextLose.text = _roundCoins.ToString();
    }

    public void IncreaseAndSaveTotalCoins()
    {
        _totalCoins += _roundCoins;
        PlayerPrefs.SetInt("TotalGameCoins", _totalCoins);
    }
}