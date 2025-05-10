using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardRoomManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerDisplay;
    [SerializeField] int numberOfSeconds = 20;
    [SerializeField] DeathEnergyHolder deathEnergyHolder;
    private int secondsLeft;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        secondsLeft = numberOfSeconds;
        timerDisplay.text = secondsLeft.ToString() + "s";
        while (true)
        {
            yield return new WaitForSeconds(1);
            secondsLeft--;
            timerDisplay.text = secondsLeft.ToString() + "s";
            if(secondsLeft == 0)
            {
                break;
            }
        }
        GameManager.startDeathEnergyPercent = deathEnergyHolder.DeathEnergyPercent;
        SceneManager.LoadScene(GameManager.nextLevelName);
    }
}
