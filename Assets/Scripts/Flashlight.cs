﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public bool lightOn = true;
    public Light light;
    public AudioSource click;
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
            click.Play();
            lightOn = !lightOn;
            light.gameObject.SetActive(lightOn);
        }   
    }
}
