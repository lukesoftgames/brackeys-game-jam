using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTarget : MonoBehaviour, IInteractable {
    public void interact(GameObject interactor) {
        Debug.Log("Win");
        GameEvents.current.Win();
    }
}
