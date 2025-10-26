using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Collider boxCol;
    Vector3 startPos, startRot;
    public Wells well;
    public ItemState itemState;
    public enum ItemState
    {
        Transporting,
        Waiting,
        None,
    }

    void Start()
    {
        startPos = transform.position;
        startRot = transform.localEulerAngles;
    }

    void Update()
    {
        if (itemState == ItemState.Transporting)
        {
            if (transform.position.y > 0f && transform.position.x == well.transform.position.x)
            {
                transform.position -= Vector3.up * Time.deltaTime;
            }
            else if (transform.position.x == well.transform.position.x)
            {
                Vector3 newPos = well.connectedWell.transform.position;
                transform.position = new Vector3(newPos.x, newPos.y - 1.0f, newPos.z);
            }

            if (transform.position.x == well.connectedWell.transform.position.x && transform.position.y < well.connectedWell.transform.position.y + 1.0f)
            {
                transform.position += Vector3.up * Time.deltaTime;
            }
            else if (transform.position.x == well.connectedWell.transform.position.x)
            {
                itemState = ItemState.Waiting;
            }
        }

        if (itemState == ItemState.Waiting)
        {
            if (well == null) return;
            if (well.connectedWell.playerDetected != null)
            {
                well.connectedWell.playerDetected.item = this;
            }
        }
    }

    public void Drop()
    {
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
