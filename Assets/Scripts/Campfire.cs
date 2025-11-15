using UnityEngine;

public class Campfire : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Marshmallow") col.gameObject.GetComponent<Marshmallow>().nearFlame = true;
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Marshmallow") col.gameObject.GetComponent<Marshmallow>().nearFlame = false;
    }
}
