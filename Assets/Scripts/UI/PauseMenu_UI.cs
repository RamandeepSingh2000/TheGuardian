using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_UI : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private GameObject _ui;
    private MenuInteractionActions _inputActions;
    private bool _isActive = false;

    [SerializeField] private AudioSource _uiAudioSource;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _hoverSound;

    private void Awake()
    {
        _inputActions = new MenuInteractionActions();        
    }

    private void OnEnable()
    {
        _inputActions.Player.Menu.performed += Menu_performed;
        _inputActions.Enable();
    }

    private void OnDisable()
    {        
        _inputActions.Disable();
    }

    private void Menu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _isActive = !_isActive;
        Pause(_isActive);
    }

    public void Pause(bool pause)
    {
        _isActive = pause;
        _ui.SetActive(_isActive);
        GameManager.Instance.PauseGame(_isActive);
    }

    //Function to change scene based on the variable "_sceneName"
    public void ChangeToScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_sceneName);
    }

    public void ChangeToScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeToScene(int index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
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
}
