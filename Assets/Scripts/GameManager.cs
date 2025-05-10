using Player;
using Player.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{    
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private HUDView hudView;
    public PlayerController Player => _playerController;

    [SerializeField] private string _winSceneName;
    [SerializeField] private string _gameOverSceneName;
    [SerializeField] private string _rewardSceneName;
    [SerializeField] private bool _shouldTransitionToRewardScene;
    public bool IsGamePaused { get; private set; } = false;
    public Action OnGamePaused;
    public Action OnGameResumed;

    public static float startDeathEnergyPercent;
    public static string nextLevelName;
    private void Start()
    {
        var portalAttack = _playerController.GetComponent<AttackController>().GetAttack(AttackName.Portal);
        portalAttack.SetStartAmmunition((int)(startDeathEnergyPercent * 100));
        portalAttack.SetAmmo(startDeathEnergyPercent * 100);
        hudView.UpdateDeathEnergyPercentage(startDeathEnergyPercent);
        startDeathEnergyPercent = 0;
        var enemySpawnerManager = FindObjectOfType<EnemySpawnerManager>();
        if (enemySpawnerManager)
        {
            enemySpawnerManager.OnSpawnerDestroyed += () =>
            {
                if (enemySpawnerManager.NumberOfActivePortals == 0)
                {
                    if (_shouldTransitionToRewardScene)
                    {
                        nextLevelName = _winSceneName;
                        ChangeScene(_rewardSceneName, 2);
                    }
                    else
                    {
                        ChangeScene(_winSceneName, 2);
                    }
                }                    
            };
        }
        _playerController.GetCustomComponent<PlayerHealthController>().OnDie += () => ChangeScene(_gameOverSceneName, 2);

        SFXManager.Instance.Play2DSound(SFXCategory.Music, "GameplayMusic");
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }

    /// <summary>
    /// When changing scenes, it will reset the timeScale to its original value 
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        Time.timeScale = 1.0f;
    }

    public void PauseGame(bool pause)
    {
        IsGamePaused = pause;
        Time.timeScale = pause ? 0.0f : 1.0f;
        if (pause) OnGamePaused?.Invoke();
        else OnGameResumed?.Invoke();
    }

    public void ChangeScene(string name, float delay = 0)
    {
        if (delay == 0)
            SceneManager.LoadScene(name);
        else
            StartCoroutine(ChangeSceneCoroutine(name, delay));
    }

    private IEnumerator ChangeSceneCoroutine(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(name);
    }
}
