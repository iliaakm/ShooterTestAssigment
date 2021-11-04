using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIScreenManager : MonoBehaviour
{
    [Inject(Id = "GameOverScreen")]
    RectTransform gameOverScreen;
    [Inject(Id = "WonScreen")]
    RectTransform wonScreen;

    public void ShowWonScreen()
    {
        wonScreen.gameObject.SetActive(true);
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.gameObject.SetActive(true);
    }
}
