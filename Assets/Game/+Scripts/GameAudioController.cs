using UnityEngine;

public class GameAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _soundController;
    [SerializeField] private AudioSource _musicController;

    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _win;
    [SerializeField] private AudioClip _lose;
    [SerializeField] private AudioClip _time;
    [SerializeField] private AudioClip _timesUp;
    [SerializeField] private AudioClip _fail;
    [SerializeField] private AudioClip _true;
    private float _vibration;

    private void Start()
    {
        Vibration.Init();
        _soundController.volume = PlayerPrefs.GetFloat("SoundVolume", 1);
        _musicController.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
        _vibration = PlayerPrefs.GetFloat("VibroStatus", 1);
    }

    public void PlayClickSound()
    {
        _soundController.PlayOneShot(_click);
        if (_vibration == 1) Vibration.VibratePop();
    }

    public void PlayWinSound()
    {
        _soundController.PlayOneShot(_win);
        if (_vibration == 1) Vibration.Vibrate();
    }

    public void PlayLoseSound()
    {
        _soundController.PlayOneShot(_lose);
        if (_vibration == 1) Vibration.Vibrate();
    }

    public void PlayTimeSound()
    {
        _soundController.PlayOneShot(_time);
        if (_vibration == 1) Vibration.VibratePop();
    }

    public void PlayTimesUpSound()
    {
        _soundController.PlayOneShot(_timesUp);
        if (_vibration == 1) Vibration.VibratePeek();
    }

    public void PlayFailSound()
    {
        _soundController.PlayOneShot(_fail);
        if (_vibration == 1) Vibration.VibrateNope();
    }

    public void PlayTrueSound()
    {
        _soundController.PlayOneShot(_true);
        if (_vibration == 1) Vibration.VibratePeek();
    }

    public void DisableMusic()
    {
        _musicController.Stop();
    }
}
