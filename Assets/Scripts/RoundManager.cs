using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {
    public static RoundManager current { get; private set; }
    private int roundNum;
    public static int maxClones = 5;
    public GameObject[] targetsObjects;
    private List<GameObject> targets;
    [SerializeField] private GameObject clone;
    [SerializeField] private float border = 500f;
    private bool goToCar;
    public GameObject car;
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
        GameEvents.current.onFlash += EndRound;

        GameEvents.current.onPickup += CheckCar;
        targets = new List<GameObject>();
        //car.GetComponent<CarTarget>().enabled = false;
        int maxAttempts = 20;
        foreach (GameObject t in targetsObjects) {
            int i = 0;
            Vector3 pos;
            pos = new Vector3(Random.Range(10f, border - 10f), 2f, Random.Range(10f, border - 10f));
            while(!Physics.CheckSphere(pos, 0.25f) && i < maxAttempts)
            {
                pos = new Vector3(Random.Range(10f, border - 10f), 2f, Random.Range(10f, border - 10f));
                i++;
            }
            pos.y = 200f;
            GameObject curTarget = Instantiate(t,pos, Quaternion.identity);
            targets.Add(curTarget);
            //curTarget.transform.position = curTarget.transform.position+ new Vector3(Random.Range(0f, border), 0f, Random.Range(0f, border));
        }
    }

   
    private void CheckCar(GameObject curObj) {

        targets.Remove(curObj);
        if (targets.Count == 0) {
            goToCar = true;
        }
        
        if (goToCar == true) {
            car.AddComponent<CarTarget>();
            //car.GetComponent<CarTarget>().enabled = true;
        }
    }

    private void EndRound() {
        GameObject newClone = Instantiate(clone);
        newClone.GetComponent<CloneController>().setCloneID(roundNum);
        Debug.Log("END ROUND");
        GameEvents.current.RoundEnd(roundNum);

        roundNum += 1;
        this.GetComponent<RoundTimer>().setFirstRound(false);
    }
}
