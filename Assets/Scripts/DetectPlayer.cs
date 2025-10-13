using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField] Transform moveableObj;

    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null)
            transform.position = new Vector3(moveableObj.position.x, transform.position.y, transform.position.z);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerMovement>().moveableObj = this;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerMovement>().moveableObj = null;
        }
    }
}
