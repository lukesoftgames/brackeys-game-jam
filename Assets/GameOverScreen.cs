using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{

    public GameObject gameOverScreen;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onGameOver += GameOver;
        gameOverScreen.SetActive(false);
    }

    void GameOver()
    {
        Debug.Log("Show");
        gameOverScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
