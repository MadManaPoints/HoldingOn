using Unity.VisualScripting;
using UnityEngine;

public class Bush : MonoBehaviour
{
    [SerializeField] Bunny bunny;
    bool carrotNearby;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player") bunny.hiding = true;
        if (col.gameObject.tag == "Carrot")
        {
            carrotNearby = true;
            bunny.GetComponent<Bunny>().carrot = col.gameObject.GetComponent<Carrot>();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player") bunny.hiding = false;
        if (col.gameObject.tag == "Carrot")
        {
            carrotNearby = false;
            bunny.GetComponent<Bunny>().carrot = null;
        }
    }
}
