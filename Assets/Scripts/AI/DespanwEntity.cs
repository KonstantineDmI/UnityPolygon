using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespanwEntity : MonoBehaviour
{
    int i = 0;
    public GameObject respawn;
    public GameObject prefab;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AIDefaultPeople>() == true)
        {
           
            Object.Destroy(other.gameObject);
            Spawn();
        }




    }
    public void Spawn()
    {
        i = 0;
        if (i < 1)
        {
            People ppl = Instantiate(prefab.GetComponent<People>());
            ppl.transform.position = respawn.transform.position;
            i++;

            
        }
    }
   
   
}

