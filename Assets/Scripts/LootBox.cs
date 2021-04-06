using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LootBox : MonoBehaviour
{


    public bool wasPushed;
    public bool inTriggerLoot;
    public bool timerOn;

    public float currentTime = 0;
    public float startTime = 4;

    public KeyCode lootButton;
    GameObject inventoryObj;
    Inventory inventory;
    public GameObject text;
    public GameObject textForScreen;
    
    void Start()
    {
        text.SetActive(false);
        currentTime = startTime;
        inventoryObj = GameObject.FindGameObjectWithTag("InventoryManager");
        inventory = inventoryObj.GetComponent<Inventory>();
    }
    void Update()
    {
       
      
        if (Input.GetKeyDown(lootButton) && inTriggerLoot == true && currentTime == startTime)
        {
            wasPushed = true;
            timerOn = true;
        }


        if(timerOn == true && currentTime <= startTime && currentTime >=0)
        {
            currentTime -= 1 * Time.deltaTime;
        }


        else
        {
            timerOn = false;
            currentTime = startTime;
        }


    }
    

   
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            text.SetActive(true);
            textForScreen.SetActive(true);
            textForScreen.GetComponent<Text>().text = "Нажмите кнопку " + lootButton.ToString() + " чтобы облутать";
            inTriggerLoot = true;
            if (wasPushed == true)
            {
                Loot();
                wasPushed = false;
            }
        }
        
    }
    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Player>() != null)
        {
            text.SetActive(false);
            inTriggerLoot = false;
        }
        
    }

    void Loot()
    {

        int rand = Random.Range(1, 6);
        
            for (int i = 0; i < inventory.database.transform.childCount; i++)
            {
                Item currentItem = inventory.database.transform.GetChild(i).GetComponent<Item>();

                if (currentItem != null)
                {
                    if (currentItem.id == rand)
                    {
                        inventory.Message(currentItem);

                        inventory.AddToInventory(currentItem);

                    }

                }
            }
    }
   

    
}






