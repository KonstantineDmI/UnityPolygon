using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMessage : MonoBehaviour
{
    // Start is called before the first frame update

    public float time;
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, time);
    }
}
