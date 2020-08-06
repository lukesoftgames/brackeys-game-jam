using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IInteractable {
    public void interact(GameObject interactor) {
        Debug.Log("INTERACTING");
        Destroy(this.gameObject);
    }
}
