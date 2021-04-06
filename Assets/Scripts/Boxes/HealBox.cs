using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBox : MonoBehaviour
{
    public float health;
    public float healthAvg;

    HealthBar hb = new HealthBar();

    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
       
      
        healthAvg = 1f - GameObject.Find("Sphere").GetComponent<HealthBar>().health;
        GameObject.Find("Sphere").GetComponent<HealthBar>().health += healthAvg;
        

        


    }

}
