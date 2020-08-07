using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundTimer : MonoBehaviour {
    [SerializeField] private float roundTime = 60;
    [SerializeField] private Text countdownText;
    private float timeLeft;
    private bool firstRound = true;

    bool running = true;

    private void Start() {
        Debug.Log("start");
        GameEvents.current.onRoundEnd += ResetTimer;
        GameEvents.current.onGameOver += StopTimer;
        timeLeft = roundTime;

    }

    void StopTimer()
    {
        running = false;
    }
    
    private void ResetTimer(int roundNum) {
        timeLeft = roundTime;
    }

    private void EndTimer() {
        Debug.Log("TimerDone");
        GameEvents.current.TimerEnd();
        timeLeft = roundTime;
    }

    private void Update() {
        if (running)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) {
                EndTimer();
            }
        }
        if (firstRound) {
            countdownText.enabled = false;
        } else {
            countdownText.enabled = true;
        }

        int minutes = (int)timeLeft / 60;
        int seconds = (int)timeLeft % 60;

        countdownText.text = minutes.ToString("00")+":"+seconds.ToString("00");
    }

    public void setFirstRound(bool isFirstRound) {
        firstRound = isFirstRound;
    }

}
