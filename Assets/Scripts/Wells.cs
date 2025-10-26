using UnityEngine;

public class Wells : MonoBehaviour
{
    [SerializeField] int wellNum;
    public Wells connectedWell;
    public bool itemCollect;
    public Item collectedItem;
    public PlayerMovement playerDetected;
    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        ;
        if (col.gameObject.tag == "Player")
        {
            playerDetected = col.gameObject.GetComponent<PlayerMovement>();
            playerDetected.well = this;

            if (collectedItem != null) playerDetected.item = collectedItem;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerDetected = col.gameObject.GetComponent<PlayerMovement>();
            if (collectedItem != null) playerDetected.item = null;
            playerDetected.well = null;
        }
    }
}
