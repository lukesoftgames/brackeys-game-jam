using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CloneState
{
    WANDERING,
    TURNINGTOPLAYER,
    LOOKINGATPLAYER,
    RETURNINGTOWALK,
}

public class CloneController : MonoBehaviour {
    private List<PointInTime> pointsInTime;
    private int cloneID;
    private int posIndex;
    private float t;
    bool inFieldOfVision;
    bool spotted;

    Animator animator;
    public float torchlessSpotlight;

    public float fieldOfViewAngle;
    public LayerMask layerMask;
    [SerializeField] private float posUpdateTime = 1f;
    Transform playerPos;
    Flashlight flashlight;
    bool turning;
    Coroutine smoothMove = null;
    Quaternion lastRot;
    CloneState state = CloneState.WANDERING;

    private void Awake() {
        animator = GetComponent<Animator>();
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

    private bool CanSeePlayer()
    {
        Vector3 direction = playerPos.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        if (angle < fieldOfViewAngle * 0.5f)
        {
            if (!flashlight.lightOn)
            {
                RaycastHit hit;


                if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, layerMask))
                {

                    // does the clone have line of sight
                    Debug.DrawRay(transform.position, direction.normalized * hit.distance, Color.yellow);
                    if (hit.transform.gameObject.tag == "Player" && hit.distance < torchlessSpotlight)
                    {
                        return true;

                    } else
                    {
                        return false;
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position, direction.normalized * 10f, Color.yellow);
                    return false;
                }
            }
            else
            {
                // The clone can see you if your flashlight is on
                return true;
            }
        }
        return false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inFieldOfVision = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inFieldOfVision = false;
        }
    }
   
    private void Update() {
        if (inFieldOfVision)
        {
            bool nextState = CanSeePlayer();
            if (nextState == false && spotted == true)
            {
                t = 0;
                state = CloneState.RETURNINGTOWALK;
            } else if (nextState == true && spotted == false)
            {
                t = 0;
                state = CloneState.TURNINGTOPLAYER;
            }
            spotted = nextState;
        } else if (state != CloneState.WANDERING && state != CloneState.RETURNINGTOWALK)
        {
            t = 0;
            state = CloneState.RETURNINGTOWALK;
        }


        if (state == CloneState.WANDERING)
        {
            // rotate back to look
               if (pointsInTime.Count -1 > posIndex) {
                    //Debug.Log("initial" + pointsInTime[posIndex].rotation);
                    //Debug.Log("next" + pointsInTime[posIndex+1].rotation);
                    t += Time.deltaTime / posUpdateTime;
                    transform.position = Vector3.Lerp(pointsInTime[posIndex].position, pointsInTime[posIndex+1].position, t);
                    transform.rotation = Quaternion.Slerp(pointsInTime[posIndex].rotation, pointsInTime[posIndex + 1].rotation, t);
                    lastRot = transform.rotation;
                    // Debug.Log(lastPosition);
                    if (t > 1)
                    {
                        t = 0;
                        posIndex += 1;
                    }
                }
        } else if (state == CloneState.TURNINGTOPLAYER)
        {
            Quaternion currentRot = transform.rotation;
            Quaternion newRot = Quaternion.LookRotation(playerPos.position -
                transform.position, transform.TransformDirection(Vector3.up));
            if (t < 1f)
            {
                t += Time.deltaTime;
                transform.rotation =
                    Quaternion.Lerp(currentRot, newRot, t / 1f);
            } else
            {
                t = 0;
                state = CloneState.LOOKINGATPLAYER;
            }
        } else if (state == CloneState.LOOKINGATPLAYER)
        {
            t = 0;
            transform.LookAt(playerPos.position);
        } else if (state == CloneState.RETURNINGTOWALK)
        {
            Quaternion currentRot = transform.rotation;
            if (t < 1f && currentRot != lastRot)
            {
                t += Time.deltaTime;
                transform.rotation =
                    Quaternion.Slerp(currentRot, lastRot, Time.deltaTime * 5f);
                Debug.Log(transform.rotation.eulerAngles.y);
            } else
            {
                t = 0;
                state = CloneState.WANDERING;
            }
        }
        
        if (state != CloneState.WANDERING)
        {
            animator.SetBool("isIdle", true);
        } else
        {
            animator.SetBool("isIdle", false);
        }

    }

    public void setCloneID(int curCloneID) {
        cloneID = curCloneID;
    }
}
