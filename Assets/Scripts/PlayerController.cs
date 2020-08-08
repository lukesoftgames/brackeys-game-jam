using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;

public class PlayerController : MonoBehaviour {
    private Vector3 newPos;
    private CharacterController cc;
    private List<PointInTime> pointsInTime;
    [SerializeField] private float posUpdateTime=1f;
    [SerializeField] private float shakeIncrease = 0.1f;
    [SerializeField] private float shakeMax = 1f;
    [SerializeField] private float shakeDecrease = 0.3f;

    [SerializeField] private float volumeIncrease = 0.1f;
    public AudioSource dong;

    private float timeLeft;
    Coroutine spotted;
    int spottedCount = 0;

    public AudioSource rumble;
    public AudioSource thump;
    public Vector3 respawnAreaStart;
    public Vector3 respawnAreaEnd;


    public LayerMask layerMask;
    private void Start() {
        rumble.volume = 0f;
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
            thump.Play();
            rumble.Stop();
        };
        GameEvents.current.onPlayerSpotted += PlayerSpotted;
        GameEvents.current.onPlayerSafe += PlayerSafe;

    }

    IEnumerator Spotted()
    {
       
        while (CameraShake.current.magnitude < shakeMax)
        {
            if (rumble.volume < 1f)
            {
                rumble.volume += Time.deltaTime * volumeIncrease;
            }
           
           
           rumble.pitch += Time.deltaTime * volumeIncrease;
            
            CameraShake.current.magnitude += Time.deltaTime * shakeIncrease;
            yield return null;
        }
    }

    IEnumerator Safe()
    {
        while (CameraShake.current.magnitude > 0f)
        {
            if (rumble.volume > 0f)
            {
                rumble.volume -= Time.deltaTime * volumeIncrease * 3f;
            }
            if (rumble.pitch > 1f)
            {
                rumble.pitch -= Time.deltaTime * volumeIncrease;
            }
            CameraShake.current.magnitude -= Time.deltaTime * shakeDecrease;
            yield return null;
        }
        CameraShake.current.StopShake();
        spotted = null;
    }
    void PlayerSafe()
    {
       
        if (spotted != null)
        {
            StopCoroutine(spotted);
        }

        spotted = StartCoroutine(Safe());
    }

    void PlayerSpotted()
    {
        dong.Play();
        if (!rumble.isPlaying)
        {
            rumble.Play();
        }
        if (spotted != null)
        {
            StopCoroutine(spotted);
        }
        CameraShake.current.StartShake();
        spotted = StartCoroutine(Spotted());
    }

    private void OnDrawGizmos()
    {
        Vector3 respawnArea = respawnAreaEnd - respawnAreaStart;
        Gizmos.DrawWireCube(respawnAreaStart + respawnArea / 2, respawnArea);
    }

    private void ResetPosition(int roundNum) {
        Debug.Log(pointsInTime.Count);
        pointsInTime.Add(new PointInTime(this.transform.position, this.transform.rotation));
        GameEvents.current.SendPointsInTime(roundNum, pointsInTime);
        Debug.Log("new position");
        
        newPos = new Vector3(Random.Range(respawnAreaStart.x, respawnAreaEnd.x), 2000f, Random.Range(respawnAreaStart.z, respawnAreaEnd.z));

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            newPos.y = hit.point.y + 5f;
        } else
        {
            newPos.y = 2f;
        }
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
