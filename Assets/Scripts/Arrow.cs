using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    private void Update() {
        FindClosesetTarget();
    }

    private void FindClosesetTarget() {
        float distanceToClosestTarget = Mathf.Infinity;
        Target closestTarget = null;
        Target[] allTargets = GameObject.FindObjectsOfType<Target>();
        
        foreach (Target curTarget in allTargets) {
            float dist = (curTarget.transform.position - this.transform.position).sqrMagnitude;
            if (dist < distanceToClosestTarget) {
                distanceToClosestTarget = dist;
                closestTarget = curTarget;
            }
        }

        Vector3 targetPosition = closestTarget.transform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
    }
}
