﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    private void Update() {
        FindClosesetTarget();
    }

    private void FindClosesetTarget() {
        float distanceToClosestTarget = Mathf.Infinity;
        GameObject closestTarget = null;
        Target[] allTargets = GameObject.FindObjectsOfType<Target>();
        if (allTargets.Length<1) {
            CarTarget car = GameObject.FindObjectOfType<CarTarget>();
            if (car == null) {
                return;
            }
            closestTarget = car.gameObject;
        } else {
            foreach (Target curTarget in allTargets) {
                float dist = (curTarget.transform.position - this.transform.position).sqrMagnitude;
                if (dist < distanceToClosestTarget) {
                    distanceToClosestTarget = dist;
                    closestTarget = curTarget.gameObject;
                }
            }
        }

        Vector3 targetPosition = closestTarget.transform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
    }
}
