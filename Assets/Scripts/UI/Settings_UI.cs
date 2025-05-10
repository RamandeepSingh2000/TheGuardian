using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings_UI : MonoBehaviour
{
    [SerializeField] private AudioSource _uiAudioSource;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _hoverSound;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(SFXManager.MUSIC_VOLUME_PROPERTY))
            _musicSlider.value = PlayerPrefs.GetFloat(SFXManager.MUSIC_VOLUME_PROPERTY);
        else PlayerPrefs.SetFloat(SFXManager.MUSIC_VOLUME_PROPERTY, 1);
        if (PlayerPrefs.HasKey(SFXManager.SFX_VOLUME_PROPERTY))
            _sfxSlider.value = PlayerPrefs.GetFloat(SFXManager.SFX_VOLUME_PROPERTY);
        else PlayerPrefs.SetFloat(SFXManager.SFX_VOLUME_PROPERTY, 1);
    }

    private void OnEnable()
    {
        _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        _sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnDisable()
    {
        _musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        _sfxSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
    }

    //Funtion to play click sound
    public void PlayClickSound()
    {
        _uiAudioSource.PlayOneShot(_clickSound);
    }
    //Funtion to play click sound
    public void PlayHoverSound()
    {
        _uiAudioSource.PlayOneShot(_hoverSound);
    }

    private void OnMusicVolumeChanged(float volume)
    {
        PlayerPrefs.SetFloat(SFXManager.MUSIC_VOLUME_PROPERTY, volume);
    }

    private void OnSFXVolumeChanged(float volume)
    {
        PlayerPrefs.SetFloat(SFXManager.SFX_VOLUME_PROPERTY, volume);
    }

}
