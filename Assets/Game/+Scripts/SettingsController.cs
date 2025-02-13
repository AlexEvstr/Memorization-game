using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject _onSound;
    [SerializeField] private GameObject _offSound;
    [SerializeField] private GameObject _onMusic;
    [SerializeField] private GameObject _offMusic;
    [SerializeField] private GameObject _onVibro;
    [SerializeField] private GameObject _offVibro;

    [SerializeField] private AudioSource _soundController;
    [SerializeField] private AudioSource _musicController;

    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _buySound;

    [SerializeField] private GameObject _backBtn;
    [SerializeField] private GameObject _saveBtn;

    private float _vibration;

    private void Start()
    {
        Vibration.Init();

        _soundController.volume = PlayerPrefs.GetFloat("SoundVolume", 1);
        if (_soundController.volume == 1)
        {
            _onSound.SetActive(true);
            _offSound.SetActive(false);
        }
        else
        {
            _offSound.SetActive(true);
            _onSound.SetActive(false);
        }

        _musicController.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
        if (_musicController.volume == 1)
        {
            _onMusic.SetActive(true);
            _offMusic.SetActive(false);
        }
        else
        {
            _offMusic.SetActive(true);
            _onMusic.SetActive(false);
        }

        _vibration = PlayerPrefs.GetFloat("VibroStatus", 1);

        if (_vibration == 1)
        {
            _onVibro.SetActive(true);
            _offVibro.SetActive(false);
        }
        else
        {
            _offVibro.SetActive(true);
            _onVibro.SetActive(false);
        }
    }

    public void DisableSound()
    {
        _offSound.SetActive(true);
        _onSound.SetActive(false);
        _soundController.volume = 0;
        PlayerPrefs.SetFloat("SoundVolume", 0);
        ShowSaveButton();
    }

    public void EnableSound()
    {
        _onSound.SetActive(true);
        _offSound.SetActive(false);
        _soundController.volume = 1;
        PlayerPrefs.SetFloat("SoundVolume", 1);
        ShowSaveButton();
    }

    public void DisableMusic()
    {
        _offMusic.SetActive(true);
        _onMusic.SetActive(false);
        _musicController.volume = 0;
        PlayerPrefs.SetFloat("MusicVolume", 0);
        ShowSaveButton();
    }

    public void EnableMusic()
    {
        _onMusic.SetActive(true);
        _offMusic.SetActive(false);
        _musicController.volume = 1;
        PlayerPrefs.SetFloat("MusicVolume", 1);
        ShowSaveButton();
    }

    public void DisableVibro()
    {
        _offVibro.SetActive(true);
        _onVibro.SetActive(false);
        PlayerPrefs.SetFloat("VibroStatus", 0);
        ShowSaveButton();
    }

    public void EnableVibro()
    {
        _onVibro.SetActive(true);
        _offVibro.SetActive(false);
        PlayerPrefs.SetFloat("VibroStatus", 1);
        ShowSaveButton();
    }

    public void PlayClickSound()
    {
        if (_vibration == 1) Vibration.VibratePop();
        _soundController.PlayOneShot(_clickSound);
    }

    public void PlayBuySound()
    {
        if (_vibration == 1) Vibration.VibratePeek();
        _soundController.PlayOneShot(_buySound);
    }

    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene("menu");
    }

    private void ShowSaveButton()
    {
        _backBtn.SetActive(false);
        _saveBtn.SetActive(true);
    }

    public void ShowBackButton()
    {
        _saveBtn.SetActive(false);
        _backBtn.SetActive(true);
    }
}