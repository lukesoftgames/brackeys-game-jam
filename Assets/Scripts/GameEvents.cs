using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameEvents : MonoBehaviour {
    public static GameEvents current;

    private void Awake() {
        if (current == null) {
            current = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    public event Action onTimerEnd;
    public void TimerEnd() {
        if (onTimerEnd != null) {
            onTimerEnd();
        }
    }

    public event Action<int> onRoundEnd;
    public void RoundEnd(int roundNum) {
        if (onRoundEnd != null) {
            onRoundEnd(roundNum);
        }
    }

    public event Action<int,List<PointInTime>> onSendPointsInTime;
    public void SendPointsInTime(int cloneId, List<PointInTime> curPointsInTime) {
        if (onSendPointsInTime != null) {
            onSendPointsInTime(cloneId, curPointsInTime);
        }
    }

    public event Action onInteract;
    public void Interact() {
        if (onInteract != null) {
            onInteract();
        }
    }

}
