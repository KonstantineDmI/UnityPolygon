using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CurrentItem : MonoBehaviour, IPointerClickHandler
{

   
    public int index;
    GameObject inventoryObj;
    Inventory inventory;


    void Start()
    {
        inventoryObj = GameObject.FindGameObjectWithTag("InventoryManager");
        inventory = inventoryObj.GetComponent<Inventory>();
    }


   


    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //Remove();
            //Drop();
            //inventory.DisplayItemsPl();
            inventory.Menu();
        }


        if (eventData.button == PointerEventData.InputButton.Left)
        {

            //Use();
            //Remove();
            //inventory.DisplayItemsPl();
            inventory.Menu();
            

        }

    }
    public void Remove()
    {

        if (inventory.item[index].id != 0)
        {

            //if (inventory.item[index].countItem > 1)
            //{
            //    inventory.item[index].countItem--;
            //}
            //else
            //{
            //    inventory.item[index] = new Item();
            //}
            //inventory.DisplayItems();

            


        }

    }

    void Drop()
    {

        if (inventory.item[index].id != 0)
        {
            for (int i = 0; i < inventory.database.transform.childCount; i++)
            {
                Item item = inventory.database.transform.GetChild(i).GetComponent<Item>();
                if (item != null)
                {
                    if (inventory.item[index].id == item.id)
                    {
                        GameObject dropedObj = Instantiate(item.gameObject);

                        dropedObj.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
                    }
                }

            }

        }

    }

    void Use()
    {

        if (inventory.item[index].id != 0)
        {
           
                    if (inventory.item[index].isEatable && GameObject.Find("Sphere").GetComponent<EatBar>().eat + (inventory.item[index].food / 100) <= 1f)
                    {
                        Debug.Log(inventory.item[index].food);
                        GameObject.Find("Sphere").GetComponent<EatBar>().eat += (inventory.item[index].food / 100);
                        Debug.Log(inventory.item[index].food/100);


                    }
                

            

        }
        



    }

 
}

