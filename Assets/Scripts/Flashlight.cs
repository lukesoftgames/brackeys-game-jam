using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public bool lightOn = true;
    public Light light;

    public void SetColor(Color color)
    {
        light.color = color;
    }
    private void Start()
    {
        light.gameObject.SetActive(lightOn);        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            lightOn = !lightOn;
            light.gameObject.SetActive(lightOn);
        }   
    }
}
