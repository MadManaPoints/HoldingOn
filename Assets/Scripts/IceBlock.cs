using UnityEngine;

public class IceBlock : MonoBehaviour
{
    bool melting;
    float meltTargetTime = 5.0f;

    void Update()
    {
        if (melting)
        {
            if (meltTargetTime > 0f)
            {
                meltTargetTime -= Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Marshmallow" && col.gameObject.GetComponent<Marshmallow>().burning)
        {
            melting = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Marshmallow")
        {
            melting = false;
        }
    }
}
