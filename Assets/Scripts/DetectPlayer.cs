using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField] Transform moveableObj;
    Vector3 startPos;
    public Vector3 offset;

    void Start()
    {
        startPos = transform.position;
        offset = startPos - moveableObj.position;
    }

    void Update()
    {
        if (transform.parent == null) // Snap back to moveable object when empty
        {
            transform.position = new Vector3(moveableObj.position.x + offset.x, startPos.y, startPos.z);
            transform.localEulerAngles = Vector3.zero;
        }
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
