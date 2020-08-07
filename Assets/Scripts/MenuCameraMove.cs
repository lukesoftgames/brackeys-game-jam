using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuCameraMove : MonoBehaviour
{
    public float speed = 10f;
    public GameObject treePlate1;
    public GameObject treePlate2;
    int current = 1;

    bool moved = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs((treePlate1.transform.position.z - transform.position.z)) < 1f && current == 1)
        {
            moved = true;
            current = 2;
            treePlate2.transform.position = new Vector3(50f, 0f, treePlate2.transform.position.z + 200f);
        }
        if (Mathf.Abs((treePlate2.transform.position.z - transform.position.z)) < 1f && current == 2)
        {
            moved = true;
            current = 1;
            treePlate1.transform.position = new Vector3(50f, 0f, treePlate1.transform.position.z + 200f);
        }
        transform.Translate(transform.forward * Time.deltaTime * speed);
    }
}
