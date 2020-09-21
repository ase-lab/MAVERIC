using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class ThrustControl : MonoBehaviour
{
    [Range(0,4530)]
    public int thrust = 1;
    public GameObject prop1, prop2, prop3, prop4;
    public float CallShitInShitOut()
    {
        //float rpm = ShitInShitMethod(thrust,10, 3);
        //Debug.Log(rpm);
        //return rpm % 6000;
        return thrust; 

    }
    public float ShitInShitMethod(float thrust, float propellorDiamete, float pitch)
    {
        // temp = Force * 4 * 1min/60sec 
        float rpm = thrust * 4 * 60;
        //d/(3.29546 * pitch)^1.5 
        float temp1 = (float) (propellorDiamete / (3.29546 * pitch));
        float finalTemp1 = (Mathf.Pow(temp1, 1.5f));
        // 1.225 * pi * (0.0254 * d) ^ 2 
        float temp2 =(float) 1.225 * Mathf.PI;
        float finalTemp2 = temp2 * (Mathf.Pow((float) (0.0254 * propellorDiamete), 2));
        float denominator = (float) (finalTemp1 * finalTemp2 * pitch * 0.0254);
        return rpm / denominator;

    }
    public void FixedUpdate()
    {
        prop1.transform.Rotate(Vector3.up, CallShitInShitOut() * Time.fixedDeltaTime);
        prop2.transform.Rotate(Vector3.up, CallShitInShitOut() * Time.fixedDeltaTime);
        prop3.transform.Rotate(Vector3.up, CallShitInShitOut() * Time.fixedDeltaTime);
        prop4.transform.Rotate(Vector3.up, CallShitInShitOut() * Time.fixedDeltaTime);
        
    }
}
