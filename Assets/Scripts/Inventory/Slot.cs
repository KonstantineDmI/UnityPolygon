using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    public Sprite activeCell;
    Sprite cell;
    Image img;
    // Start is called before the first frame update

    void OnDisable()
    {
        img.sprite = cell;
    }
    void Start()
    {
        img = GetComponent<Image>();
        cell = img.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        img.sprite = activeCell;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        img.sprite = cell;
    }

    // Update is called once per frame

}
