using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [Inject]
    UIScreenManager UIScreenManager;
    [Inject(Id = GameConfig.ZenjectConfig.loseTrigger)]
    PlayerTriggerAction playerLoseTrigger;
    [Inject(Id = GameConfig.ZenjectConfig.wonTrigger)]
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
        GameLog.Log("Won");
    }

    void Lose()
    {
        UIScreenManager.ShowGameOverScreen();
        StartCoroutine(ReloadLevel());
        GameLog.Log("Lose");
    }

    IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(levelReloadDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
