using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    //public bool open;
    public String doorColor;
    Item item;
    //Rigidbody rb;
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.gameObject.GetComponent<PlayerMovement>().item != null &&
            col.gameObject.GetComponent<PlayerMovement>().item.key == doorColor)
            {
                col.gameObject.GetComponent<PlayerMovement>().DestroyItem();
                Destroy(gameObject);
            }

        }
    }
}
