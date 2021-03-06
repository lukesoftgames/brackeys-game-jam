﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public enum CloneState
{
    WANDERING,
    TURNINGTOPLAYER,
    LOOKINGATPLAYER,
    RETURNINGTOWALK,
    STOPPED
}

public class CloneController : MonoBehaviour {
    private List<PointInTime> pointsInTime;
    private int cloneID;
    private int posIndex;
    private float t;
    bool inFieldOfVision;
    bool spotted;

    float spottedTime;

    float lastLerpT;

    Animator animator;
    public float torchRange;
    public float shakeIncrease;

    public float fieldOfViewAngle;
    public LayerMask layerMask;
    [SerializeField] private float posUpdateTime = 1f;
    Transform playerPos;
    Flashlight flashlight;

    Quaternion lastRot;
    CloneState state = CloneState.WANDERING;

    AudioSource walkingSound;

    private void Awake() {
        walkingSound = GetComponent<AudioSource>();
        walkingSound.pitch = Random.Range(0.5f, 1.2f);
        animator = GetComponent<Animator>();
        posIndex = 0;
        pointsInTime = new List<PointInTime>();
        GameEvents.current.onSendPointsInTime += SetClonePositionInTime;
        GameEvents.current.onRoundEnd += ResetClone;
        GameEvents.current.onWin += StopClone;
        GameEvents.current.onGameOver += StopClone;

        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        flashlight = GameObject.Find("FirstPersonPlayer").GetComponent<Flashlight>();


    }
    void StopClone()
    {
        GameEvents.current.onWin -= StopClone;
        GameEvents.current.onGameOver -= StopClone;
        state = CloneState.STOPPED;
    }

    

    private void ResetClone(int roundNum) {
        if (cloneID < roundNum - RoundManager.maxClones)
        {
            Debug.Log("Destroy clone " + cloneID);
            Destroy(gameObject);
            GameEvents.current.onSendPointsInTime -= SetClonePositionInTime;
            GameEvents.current.onRoundEnd -= ResetClone;
            GameEvents.current.onWin -= StopClone;
            GameEvents.current.onGameOver -= StopClone;
            return;
        }
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
                    if (hit.transform.gameObject.tag == "Player" && hit.distance < torchRange)
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
        if (state == CloneState.STOPPED)
        {
            return;
        }
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
                    Vector3 move = Vector3.Lerp(pointsInTime[posIndex].position, pointsInTime[posIndex+1].position, t);
                    float diff = (move - transform.position).sqrMagnitude;
                    if (diff == 0f && walkingSound.isPlaying)
                    {
                        walkingSound.Stop();
                    } else if (diff > 0f && !walkingSound.isPlaying)
                    {
                        walkingSound.Play();
                    }
                    transform.position = move;
                    transform.rotation = Quaternion.Slerp(pointsInTime[posIndex].rotation, pointsInTime[posIndex + 1].rotation, t);
                    lastRot = transform.rotation;
                    lastLerpT = t;
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

            } else
            {
                t = lastLerpT;
                state = CloneState.WANDERING;
            }
        }
        
        if (state != CloneState.WANDERING)
        {
            walkingSound.Stop();
            animator.SetBool("isIdle", true);
        } else
        {
            animator.SetBool("isIdle", false);
        }

        if (state == CloneState.LOOKINGATPLAYER)
        {
            if (spottedTime == 0f)
            {
                GameEvents.current.PlayerSpotted();
            }
            DISystem.CreateIndicator(transform);

            if (spottedTime > 3f)
            {
                Debug.Log("Game over");
                GameEvents.current.GameOver();
                state = CloneState.STOPPED;
            }
            spottedTime += Time.deltaTime;
        } else
        {
            spottedTime = 0f;
            GameEvents.current.PlayerSafe();
        }
        

    }

    public void setCloneID(int curCloneID) {
        cloneID = curCloneID;
    }
}
