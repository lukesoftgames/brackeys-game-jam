using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Vector3 newPos;
    private CharacterController cc;
    private List<PointInTime> pointsInTime;
    [SerializeField] private float posUpdateTime=1f;
    [SerializeField] private float shakeIncrease = 0.1f;
    [SerializeField] private float shakeMax = 1f;
    [SerializeField] private float shakeDecrease = 0.3f;

    private float timeLeft;
    Coroutine spotted;

    public Vector3 respawnArea;

    private void Start() {
        pointsInTime = new List<PointInTime>();
        timeLeft = 0;
        cc=this.GetComponent<CharacterController>();
        GameEvents.current.onRoundEnd += ResetPosition;
        GameEvents.current.onGameOver += () =>
        {
            CameraShake.current.StopShake();
            if (spotted != null)
            {
                StopCoroutine(spotted);
            }
        };
        GameEvents.current.onPlayerSpotted += PlayerSpotted;
        GameEvents.current.onPlayerSafe += PlayerSafe;

    }

    IEnumerator Spotted()
    {
        while (CameraShake.current.magnitude < shakeMax)
        {
            CameraShake.current.magnitude += Time.deltaTime * shakeIncrease;
            yield return null;
        }
    }

    IEnumerator Safe()
    {
        while (CameraShake.current.magnitude > 0f)
        {
            CameraShake.current.magnitude -= Time.deltaTime * shakeDecrease;
            yield return null;
        }
        CameraShake.current.StopShake();
    }
    void PlayerSafe()
    {
        StartCoroutine(Safe());
    }

    void PlayerSpotted()
    {
        CameraShake.current.StartShake();
        StartCoroutine(Spotted());
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
