using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win_UI : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private AudioSource _uiAudioSource;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _hoverSound;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void PlayClickSound()
    {
        _uiAudioSource.PlayOneShot(_clickSound);
    }
    //Funtion to play click sound
    public void PlayHoverSound()
    {
        _uiAudioSource.PlayOneShot(_hoverSound);
    }

    //Function to change scene based on the variable "_sceneName"
    public void ChangeToScene()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
