using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTree : MonoBehaviour
{
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up * -1), out hit, Mathf.Infinity, layerMask))
        {
            transform.position = hit.point;
            transform.Rotate(new Vector3(0f, Random.Range(0f, 360f)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
