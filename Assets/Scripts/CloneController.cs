using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour {
    private List<PointInTime> pointsInTime;
    private int cloneID;
    private int posIndex;
    private float t;
    [SerializeField] private float posUpdateTime = 1f;

    private void Awake() {
        posIndex = 0;
        pointsInTime = new List<PointInTime>();
        GameEvents.current.onSendPointsInTime += SetClonePositionInTime;
        GameEvents.current.onRoundEnd += ResetClone;
    }

    private void ResetClone(int cloneID) {
        posIndex = 0;
    }

    private void SetClonePositionInTime(int curCloneID, List<PointInTime> curPointInTime) {
        if (curCloneID == cloneID) {
            pointsInTime = curPointInTime;
        }
    }

    private void FixedUpdate() {
        if (pointsInTime.Count -1 > posIndex) {
            Debug.Log("initial" + pointsInTime[posIndex].rotation);
            Debug.Log("next" + pointsInTime[posIndex+1].rotation);
            t += Time.deltaTime / posUpdateTime;
            transform.position = Vector3.Lerp(pointsInTime[posIndex].position, pointsInTime[posIndex+1].position, t);
            transform.rotation = Quaternion.Lerp(pointsInTime[posIndex].rotation, pointsInTime[posIndex + 1].rotation, t);
            if (t > 1) {
                t = 0;
                posIndex += 1;
            }

        }
    }

    public void setCloneID(int curCloneID) {
        cloneID = curCloneID;
    }
}
