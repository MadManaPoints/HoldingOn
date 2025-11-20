using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    HandTarget target;
    [SerializeField] Transform playerOne, playerTwo;
    Vector3 velocity = Vector3.zero;
    [SerializeField] Vector3 offset;
    Vector3 targetPosition;
    float minDist = 1.0f, maxDist = 14.0f;
    bool far;
    float camSpeed = 0.3f;


    void Start()
    {
        target = HandTarget.target;
    }


    void Update()
    {
        // Check for players
        if (playerOne == null) playerOne = GameObject.Find("Player 1").GetComponent<Transform>();
        if (playerTwo == null) playerTwo = GameObject.Find("Player 2").GetComponent<Transform>();

        // Get distance between players and clamp it
        float dist = Vector3.Distance(playerOne.transform.position, playerTwo.transform.position);
        float clampedDist = Mathf.Clamp(dist, minDist, maxDist);

        float distFromPlayer = !playerOne.gameObject.GetComponent<PlayerMovement>().attached ? 3.2f : 2.5f;

        GameManager.Instance.tooFar = clampedDist > 12.0f;

        // Move camera based on player distance 
        float rot = Map(clampedDist, minDist + 0.2f, maxDist, 10.0f, 65.0f);
        float offsetY = Map(clampedDist, minDist, 10.0f, 3.0f, 9.0f);

        //  MIGHT NOT NEED THIS ANYMORE
        //float closeOffsetZ = Map(clampedDist, minDist, maxDist, -2.0f, -6.0f);
        //float farOffsetZ = Map(clampedDist, minDist, maxDist, -4.0f, -3.0f);

        float offsetZ = playerOne.transform.position.z < playerTwo.transform.position.z ? playerOne.transform.position.z - distFromPlayer : playerTwo.transform.position.z - distFromPlayer;

        targetPosition = new Vector3(0f, offsetY, offsetZ);

        // Look toward center area
        transform.localEulerAngles = new Vector3(rot, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }


    void LateUpdate()
    {
        // Ease into position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, camSpeed);
    }


    float Map(float value, float minA, float maxA, float minB, float maxB)
    {
        float range = maxA - minA;
        float valuePercent = (value - minA) / range;

        float newRange = maxB - minB;

        return valuePercent * newRange + minB;
    }
}
