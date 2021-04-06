using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject player;
    public Image barHeath;
    public float health;
    public float currentTimeHealth = 0;
    float startingTimeHealth = 7f;
    public float sleep;
    public float eat;
    public float damage;

    SleepBar sl = new SleepBar();
    EatBar eb = new EatBar();
      
    
    void Start()
    {
        health = 1f;
        currentTimeHealth = startingTimeHealth; 
        
    }

    // Update is called once per frame
    void Update()
    {

        sleep = GameObject.Find("Sphere").GetComponent<SleepBar>().sleep;
        eat = GameObject.Find("Sphere").GetComponent<EatBar>().eat;
        health = GameObject.FindWithTag("Enemy").GetComponent<AIEnemy>().health;
        Health();
      

    }


    void Health()
    {
        
       
        barHeath.fillAmount = health;

        currentTimeHealth -= 1 * Time.deltaTime;
        if (currentTimeHealth <= 0)
        {
            currentTimeHealth = startingTimeHealth;
        }

        if (health <= 0)
        {
            health = 0f;
            barHeath.fillAmount = health;
            //Death();
        }
        if (eat <= 0 && currentTimeHealth <= 0.01f)
        {
           
            health -= 0.01f;
            barHeath.fillAmount = health;
            
        }
        if (sleep <= 0 && currentTimeHealth <= 0.01f)
        {
            
            health -= 0.01f;
            barHeath.fillAmount = health;
            
        }  
    }

   

    

    

    //void Death()
    //{
       
    //    Destroy(player);
    //}
}
