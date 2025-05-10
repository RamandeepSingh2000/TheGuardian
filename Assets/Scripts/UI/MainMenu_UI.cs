using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_UI : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private AudioSource _uiAudioSource;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _hoverSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Function to change scene based on the variable "_sceneName"
    public void ChangeToScene()
    {
        SceneManager.LoadScene(_sceneName);
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

    //Funciton to stop the game in the editor or in a build

    public void StopGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
}
