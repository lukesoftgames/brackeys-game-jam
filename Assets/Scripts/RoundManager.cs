using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {
    public static RoundManager current { get; private set; }
    private int roundNum;
    [SerializeField] private GameObject clone;
    private void Awake() {
        if (current == null) {
            current = this;
            DontDestroyOnLoad(gameObject);

        } else {
            Destroy(gameObject);
            return;
        }
    }

    private void Start() {
        roundNum = 0;
        GameEvents.current.onTimerEnd += EndRound;

    }

    private void EndRound() {
        GameObject newClone = Instantiate(clone);
        newClone.GetComponent<CloneController>().setCloneID(roundNum);
        Debug.Log("END ROUND");
        GameEvents.current.RoundEnd(roundNum);

        roundNum += 1;
    }
}
