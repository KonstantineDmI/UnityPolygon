using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatBar : MonoBehaviour
{

    
    public Image barEat;

    public float eat;
   

    public float currentTimeEat = 0;
  
    float startTimeEat = 4f;
    float eatfill;

   




    // Start is called before the first frame update
    void Start()
    {
        eat = 1f;
        currentTimeEat = startTimeEat;
    }

   

    // Update is called once per frame
    void Update()
    {
        
        Eat();
       
    }

    


    public void Eat()
    {
        barEat.fillAmount = eat;

        if (eat <= 0f)
        {
           
            eat = 0f;
            barEat.fillAmount = eat;
           

        }
        


        currentTimeEat -= 1 * Time.deltaTime;
        if (currentTimeEat <= 0f)
        {
           
            currentTimeEat = startTimeEat;

        }

       
        if (currentTimeEat <= 0.01f)
        {
           
            eat -= 0.01f;
            barEat.fillAmount = eat;
          

        }
      

    }
}
