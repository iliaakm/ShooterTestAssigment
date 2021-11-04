using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [Inject]
    UIScreenManager UIScreenManager;
    [Inject(Id = "LoseTrigger")]
    PlayerTriggerAction playerLoseTrigger;
    [Inject(Id = "WonTrigger")]
    PlayerTriggerAction playerWonTrigger;

    [SerializeField]
    float levelReloadDelay;

    private void Start()
    {
        playerLoseTrigger.TriggerEnterAction.AddListener(Lose);
        playerWonTrigger.TriggerEnterAction.AddListener(Won);
    }

    void Won()
    {
        UIScreenManager.ShowWonScreen();
        StartCoroutine(ReloadLevel());
    }

    void Lose()
    {
        UIScreenManager.ShowGameOverScreen();
        StartCoroutine(ReloadLevel());
    }

    IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(levelReloadDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
