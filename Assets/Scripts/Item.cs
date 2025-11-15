using System;
using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Collider boxCol;
    protected Vector3 startPos, startRot;
    public Wells well;
    public ItemState itemState;
    public String key;
    public Vector3 offset;
    public enum ItemState
    {
        Transporting,
        Waiting,
        None,
    }

    protected virtual void Start()
    {
        startPos = transform.position;
        startRot = transform.localEulerAngles;
    }

    protected virtual void Update()
    {
        if (itemState == ItemState.Transporting)
        {
            Debug.Log("YERRR");
            // Move down first well 
            if (transform.position.y > 0f && transform.position.x == well.transform.position.x)
            {
                transform.position -= Vector3.up * Time.deltaTime;
            }
            // Snap to below second well
            else if (transform.position.x == well.transform.position.x)
            {
                Vector3 newPos = well.connectedWell.transform.position;
                transform.position = new Vector3(newPos.x, newPos.y - 1.0f, newPos.z);
            }

            // Move up second well 
            if (transform.position.x == well.connectedWell.transform.position.x && transform.position.y < well.connectedWell.transform.position.y + 1.0f)
            {
                transform.position += Vector3.up * Time.deltaTime;
            }
            // Item has arrived
            else if (transform.position.x == well.connectedWell.transform.position.x)
            {
                well = well.connectedWell; // Switch reference to new well 
                well.collectedItem = this; // Give reference of this script to new well 
                itemState = ItemState.Waiting;
            }
        }

        if (itemState == ItemState.Waiting)
        {
            if (well == null) return;

            if (well.playerDetected != null && !well.playerDetected.holdingItem)
            {
                // In update in case player is already waiting inside collider
                well.playerDetected.item = this;
            }
        }
    }

    public void Drop()
    {
        //float offset = transform.position.x + 0.3f;

        // Place item on ground and fix rotation
        transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
        transform.localEulerAngles = startRot;
        boxCol.enabled = true;
    }

    public void TransportItem()
    {
        itemState = ItemState.Transporting;
    }



    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerMovement>().item == null)
        {
            col.gameObject.GetComponent<PlayerMovement>().item = this;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player" && col.gameObject.GetComponent<PlayerMovement>().item == this)
        {
            col.gameObject.GetComponent<PlayerMovement>().item = null;
        }
    }
}
