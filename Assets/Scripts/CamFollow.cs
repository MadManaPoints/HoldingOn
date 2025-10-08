using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] Transform playerOne, playerTwo, center;
    Vector3 velocity = Vector3.zero;
    [SerializeField] Vector3 offset;
    //Camera cam;


    void Start()
    {
        //cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Get distance between players
        float dist = Vector3.Distance(playerOne.transform.position, playerTwo.transform.position);

        // Move camera based on player distance 
        float offsetZ = Map(dist, 1.5f, 5.0f, -2.5f, -4.0f);
        offset = new Vector3(offset.x, offset.y, offsetZ);

        //print(offset);

        //Debug.Log(dist);
        transform.position = Vector3.SmoothDamp(transform.position, center.position + offset, ref velocity, 0.3f);
    }

    float Map(float value, float minA, float maxA, float minB, float maxB)
    {
        float range = maxA - minA;
        float valuePercent = (value - minA) / range;

        float newRange = maxB - minB;

        return valuePercent * newRange + minB;
    }
}
