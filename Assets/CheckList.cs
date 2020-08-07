using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckList : MonoBehaviour
{
    public Color doneColor;

    public Text airFresh;
    public Text battery;
    public Text crowbar;
    public Text fuel;
    public Text wheel;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onPickup += HandlePickUp;
    }

    void HandlePickUp(GameObject g)
    {
        Debug.Log("Check list");
        Target target = g.GetComponent<Target>();
        Debug.Log(target.code);
        if (target.code == "airfresh")
        {
            airFresh.color = doneColor;
        }
        if (target.code == "battery")
        {
            battery.color = doneColor;
        }
        if (target.code == "crowbar")
        {
            crowbar.color = doneColor;
        }
        if (target.code == "fuel")
        {
            fuel.color = doneColor;
        }
        if (target.code == "wheel")
        {
            wheel.color = doneColor;
        }
    }
}
