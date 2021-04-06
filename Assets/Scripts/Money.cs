using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    // Start is called before the first frame update
    public static int money = 500;


    // Update is called once per frame
    void Update()
    {
        Text txt = gameObject.GetComponent<Text>();
        txt.text = "Деньги: " + money.ToString() + " $";
    }
}
