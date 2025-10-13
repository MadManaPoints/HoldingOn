using Unity.VisualScripting;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    HandTarget target;
    [SerializeField] Transform playerOne, playerTwo;
    Vector3 velocity = Vector3.zero;
    [SerializeField] Vector3 offset;
    Vector3 targetPosition;


    void Start()
    {
        target = HandTarget.target;
    }


    void Update()
    {
        // Get distance between players and clamp it
        float dist = Vector3.Distance(playerOne.transform.position, playerTwo.transform.position);
        float clampedDist = Mathf.Clamp(dist, 1.0f, 12.0f);

        // Move camera based on player distance 
        float rot = Map(clampedDist, 1.2f, 12.0f, 10.0f, 70.0f);
        float offsetY = Map(clampedDist, 1.0f, 12.0f, 2.0f, 6.5f) * 1.5f;
        float closeOffsetZ = Map(clampedDist, 1.0f, 12.0f, -2.0f, -6.0f);
        float farOffsetZ = Map(clampedDist, 1.0f, 12.0f, -4.0f, -3.0f);
        float offsetZ = (dist >= 5.0f) ? farOffsetZ : closeOffsetZ;
        //Debug.Log("OffsetY: " + offsetY + "      ||     " + "OffsetZ: " + offsetZ);

        targetPosition = new Vector3(0f, offsetY, target.center.z + offsetZ);

        // Look toward center area
        transform.localEulerAngles = new Vector3(rot, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }


    void LateUpdate()
    {
        // Ease into position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.3f);
    }


    float Map(float value, float minA, float maxA, float minB, float maxB)
    {
        float range = maxA - minA;
        float valuePercent = (value - minA) / range;

        float newRange = maxB - minB;

        return valuePercent * newRange + minB;
    }
}
