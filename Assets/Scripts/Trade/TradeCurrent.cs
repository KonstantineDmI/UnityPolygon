using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TradeCurrent : MonoBehaviour, IPointerClickHandler
{
    public int index;
    GameObject inventoryObj;
    Inventory inventory;

    GameObject shopObj;

    public GameObject playerSelectedCell;
    public GameObject traderSelectedCell;
    Trade trade;
    public static Item currentItem;
    public GameObject text;

    void Start()
    {
        inventoryObj = GameObject.FindGameObjectWithTag("InventoryManager");
        inventory = inventoryObj.GetComponent<Inventory>();

        


    }
    void TradeCells()
    {

        if (inventory.item[index].id != 0)
        {
            DisplayForPlayer();
        }
        
    }


    public void OnPointerClick(PointerEventData eventData)
    {


        if (eventData.button == PointerEventData.InputButton.Left)
        {

            TradeCells();

        }

    }
    
    void DisplayForPlayer()
    {
        currentItem = inventory.item[index];
        Transform cell = playerSelectedCell.transform.GetChild(0);
        Transform icon = cell.GetChild(0);
        Transform count = icon.GetChild(0);
        Text txt = count.GetComponent<Text>();
        Image img = icon.GetComponent<Image>();
        if (currentItem.id != 0)
        {

            img.enabled = true;
            img.sprite = inventory.item[index].icon;
            if (currentItem.countItem > 1)
            {


                txt.text = currentItem.countItem.ToString();
            }
            else
            {
                txt.text = null;
            }
        }
        else
        {
            img.enabled = false;
            img.sprite = null;
        }


        text.GetComponent<Text>().text = "Название: " + currentItem.nameItem + "\n" +
                  "Цена: " + (currentItem.cost - 5).ToString();



    }

    
}
