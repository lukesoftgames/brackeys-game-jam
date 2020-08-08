using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceGenerator : MonoBehaviour
{
    public GameObject fence;
    public int numFence;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 cur = transform.position;
        Vector3 eulerRot = transform.rotation.eulerAngles;
        Quaternion newRot = Quaternion.Euler(0f, eulerRot.y, 0f);
        for (int i = 0; i < numFence; i++)
        {

            GameObject f = Instantiate(fence, cur, newRot);
            f.transform.parent = transform;
            cur.z += 3f;
        }
    }

    // Update is called once per frame
    void OnDrawGizmos()
    {
        Vector3 centre = transform.position;
        Vector3 size = new Vector3(1f, 4f, numFence * 3f);
        centre.z += (float)(3 * numFence / 2);
        Gizmos.DrawWireCube(centre, size);
    }
}
