using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IInteractable {
    public bool pickedup = false;
    public string code;
    public void interact(GameObject interactor) {
        Debug.Log("INTERACTING");
        pickedup = true;
        GameEvents.current.Pickup(this.gameObject);
        this.gameObject.SetActive(false);
        //Destroy(this);
    }
}
