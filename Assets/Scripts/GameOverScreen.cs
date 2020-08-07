using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{

    public GameObject gameOverScreen;
    public Text gameOverText;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onGameOver += GameOver;
        GameEvents.current.onWin += Win;
        gameOverScreen.SetActive(false);
    }

    void Win()
    {
        gameOverText.text = "YOU ESCAPED THE FOREST!";
        Show();
    }

    void GameOver()
    {
        gameOverText.text = "SOMETHING HAS CAUGHT YOU!";
        Show();
    }
    void Show()
    {
        Debug.Log("Show");
        gameOverScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
