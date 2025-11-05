using UnityEngine;

public class Wells : MonoBehaviour
{
    public bool inUse;
    [SerializeField] int wellNum;
    public Wells connectedWell;
    public bool itemCollect;
    public Item collectedItem;
    public PlayerMovement playerDetected;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            // Pass reference of well to player when player enters collider 
            playerDetected = col.gameObject.GetComponent<PlayerMovement>();
            playerDetected.well = this;

            // If well has an item waiting, give player reference to that item 
            if (collectedItem != null && !playerDetected.holdingItem) playerDetected.item = collectedItem;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            // Remove references when player exits collider 
            playerDetected.well = null;

            // Only remove item reference if player isn't already holding one
            if (collectedItem != null && !playerDetected.holdingItem) playerDetected.item = null;

            playerDetected = null;
        }
    }
}
