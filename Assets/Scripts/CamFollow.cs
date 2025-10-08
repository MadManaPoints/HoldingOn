using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] Transform playerOne, playerTwo, center;
    Vector3 velocity = Vector3.zero;
    [SerializeField] Vector3 offset;
    Vector3 targetPosition;


    void Update()
    {
        // Get distance between players
        float dist = Vector3.Distance(playerOne.transform.position, playerTwo.transform.position);

        // Move camera based on player distance 
        float offsetZ = Map(dist, 1.0f, 20.0f, -1.5f, -3.5f);
        float offsetY = Map(dist, 1.0f, 20.0f, 1.5f, 10.0f);
        offset = new Vector3(offset.x, offsetY, offsetZ);
        targetPosition = new Vector3(0f, center.position.y + offset.y, center.position.z + offset.z);

        // Ease into position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.3f);

        // Look toward center area
        transform.LookAt(center);

        //Debug.Log(dist + "   " + targetPosition);
    }

    float Map(float value, float minA, float maxA, float minB, float maxB)
    {
        float range = maxA - minA;
        float valuePercent = (value - minA) / range;

        float newRange = maxB - minB;

        return valuePercent * newRange + minB;
    }
}
