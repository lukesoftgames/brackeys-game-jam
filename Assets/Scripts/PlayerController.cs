using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Vector3 newPos;
    private CharacterController cc;
    private List<PointInTime> pointsInTime;
    [SerializeField] private float posUpdateTime=1f;
    private float timeLeft;


    public Vector3 respawnArea;

    private void Start() {
        pointsInTime = new List<PointInTime>();
        timeLeft = 0;
        cc=this.GetComponent<CharacterController>();
        GameEvents.current.onRoundEnd += ResetPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(respawnArea / 2, respawnArea);
    }

    private void ResetPosition(int roundNum) {
        Debug.Log(pointsInTime.Count);
        pointsInTime.Add(new PointInTime(this.transform.position, this.transform.rotation));
        GameEvents.current.SendPointsInTime(roundNum, pointsInTime);
        Debug.Log("new position");
        newPos = new Vector3(Random.Range(0f, respawnArea.x), 2f, Random.Range(0f, respawnArea.z));
        cc.enabled = false;
        this.transform.localPosition= newPos;
        cc.enabled = true;
        pointsInTime = new List<PointInTime>();
    }


    private void FixedUpdate() {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) {
            pointsInTime.Add(new PointInTime(this.transform.position,this.transform.rotation));
            timeLeft = posUpdateTime;
        }
    }

}
