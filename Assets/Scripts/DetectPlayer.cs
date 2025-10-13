using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField] Transform moveableObj;

    void Update()
    {
        if (transform.parent == null) // Snap back to moveable object when empty
            transform.position = new Vector3(moveableObj.position.x, transform.position.y, transform.position.z);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            // Link script to player that enters collider
            col.gameObject.GetComponent<PlayerMovement>().moveableObj = this;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            // Unlink script from player
            col.gameObject.GetComponent<PlayerMovement>().moveableObj = null;
        }
    }
}
