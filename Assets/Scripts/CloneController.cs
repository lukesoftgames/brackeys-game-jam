using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour {
    private List<PointInTime> pointsInTime;
    private int cloneID;
    private int posIndex;
    private float t;
    bool spotted;
    public LayerMask layerMask;
    [SerializeField] private float posUpdateTime = 1f;
    Transform playerPos;
    Flashlight flashlight;
    private void Awake() {
        posIndex = 0;
        pointsInTime = new List<PointInTime>();
        GameEvents.current.onSendPointsInTime += SetClonePositionInTime;
        GameEvents.current.onRoundEnd += ResetClone;
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        flashlight = GameObject.Find("FirstPersonPlayer").GetComponent<Flashlight>();

    }

    

    private void ResetClone(int cloneID) {
        posIndex = 0;
    }

    private void SetClonePositionInTime(int curCloneID, List<PointInTime> curPointInTime) {
        if (curCloneID == cloneID) {
            pointsInTime = curPointInTime;
        }
    }
    void LookAtPlayer()
    {
        transform.LookAt(playerPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            spotted = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            spotted = false;
        }
    }
    private bool CanSeeWithRays()
    {
        for (float angle = -20f; angle <= 20f; angle+=10f)
        {
            RaycastHit hit;
            Vector3 newAngle = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
            if (Physics.Raycast(transform.position, transform.TransformDirection(newAngle), out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.tag == "Player")
                {
                    return true;
                }

                Debug.DrawRay(transform.position, transform.TransformDirection(newAngle) * hit.distance, Color.yellow);
             
            } else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(newAngle) * hit.distance, Color.yellow);
            }
        }
        return false;
    }
    private void FixedUpdate() {

        CanSeeWithRays();

        if (false && spotted && flashlight.lightOn || CanSeeWithRays())
        {
            LookAtPlayer();
        } else
        {

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

    }

    public void setCloneID(int curCloneID) {
        cloneID = curCloneID;
    }
}
