using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{   [HideInInspector]
    public List<Item> item;
    public static Inventory inventory;
    public GameObject database;

    public GameObject cellContainer;
    
    public KeyCode showInventory;
    public KeyCode takeButton;
    public Player player;
    [Header ("Message")]
    public GameObject messageManager;
    public GameObject message;
    public GameObject point;
    public GameObject playerCellContainer;
    public GameObject shopInventory;
    public GameObject menuPanel;

   
    
    
    void Start()
    {
        item = new List<Item>();
        cellContainer.SetActive(false);

        for (int i = 0; i < cellContainer.transform.childCount; i++)
        {
            cellContainer.transform.GetChild(i).GetComponent<CurrentItem>().index = i;
          
        }

        for (int i = 0; i < cellContainer.transform.childCount; i++)
        {
            item.Add(new Item());
        }

        



    }

    // Update is called once per frame
    void Update()
    {
        
        ToggleInventory();
        if (Input.GetKeyDown(takeButton))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider.GetComponent<Item>())
                {
                    Item currentItem = hit.collider.GetComponent<Item>();
                    Message(currentItem);
                    AddToInventory(hit.collider.GetComponent<Item>());
                }
            }
            
        }
      

       



    }

    public void Menu()
    {
        menuPanel.SetActive(true);
        menuPanel.transform.position = Input.mousePosition;
    }

    public void Message(Item currentItem)
    {
        GameObject msgObj = Instantiate(message);
        msgObj.transform.SetParent(messageManager.transform);

        Image msgImg = msgObj.transform.GetChild(0).GetComponent<Image>();

        msgImg.sprite = currentItem.icon;

        Text msgTxt = msgObj.transform.GetChild(1).GetComponent<Text>();
        msgTxt.text = currentItem.nameItem;


    }
   
    public void AddToInventory(Item currentItem)
    {
       if(currentItem.isStackable == true)
        {
            AddStackableItem(currentItem);
        }
        else
        {
            AddUnStackableItem(currentItem);
        }


       
       

        
    }

    

    public void AddStackableItem(Item currentItem)
    {
        for (int i = 0; i < item.Count; i++)
        {
            if(item[i].id == currentItem.id)
            {
                item[i].countItem++;
                DisplayItems();
                DisplayItemsPl();
                return;
            }
        }
        AddUnStackableItem(currentItem);
    }

    public void AddUnStackableItem(Item currentItem)
    {
        for (int i = 0; i < item.Count; i++)
        {
            if (item[i].id == 0)
            {
                item[i] = currentItem;
                item[i].countItem = 1;
                DisplayItems();
                DisplayItemsPl();


                break;
            }
        }
    }
    
    

    
    void ToggleInventory()
    {

        if (Input.GetKeyDown(showInventory))
        {

            shopInventory.SetActive(false);

                if (cellContainer.activeSelf)
                {
                    cellContainer.SetActive(false);
                    player.enabled = true;

                    point.SetActive(true);
                }

                else
                 {
                    cellContainer.SetActive(true);
                    player.enabled = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    point.SetActive(false);
                  }

        }
       
    }
        
        
    
    
   public void DisplayItems()
    {
        for (int i = 0; i < item.Count; i++)
        {

                Transform cell = cellContainer.transform.GetChild(i);
                Transform icon = cell.GetChild(0);
                Transform count = icon.GetChild(0);
                Text txt = count.GetComponent<Text>();
                Image img = icon.GetComponent<Image>();
                if (item[i].id != 0)
                {

                    img.enabled = true;
                    img.sprite = item[i].icon;
                    if (item[i].countItem > 1)
                    {


                    txt.text = item[i].countItem.ToString();
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
            
        }
    }


    public void DisplayItemsPl()
    {
        for (int i = 0; i < playerCellContainer.transform.childCount; i++)
        {

            Transform cell = playerCellContainer.transform.GetChild(i);
            Transform icon = cell.GetChild(0);
            Transform count = icon.GetChild(0);
            Text txt = count.GetComponent<Text>();
            Image img = icon.GetComponent<Image>();
            if (item[i].id != 0)
            {

                img.enabled = true;
                img.sprite = item[i].icon;
                if (item[i].countItem > 1)
                {


                    txt.text = item[i].countItem.ToString();
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

        }
    }

    
    }


