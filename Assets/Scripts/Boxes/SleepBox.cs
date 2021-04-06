using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepBox : MonoBehaviour
{
    public float sleep;
    public float sleepAvg;
    public void OnTriggerEnter(Collider other)
    {
        
        sleepAvg = 1f - GameObject.Find("Sphere").GetComponent<SleepBar>().sleep;
        GameObject.Find("Sphere").GetComponent<SleepBar>().sleep += sleepAvg;
    }

}
