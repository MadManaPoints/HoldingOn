using UnityEngine;

public class HandTarget : MonoBehaviour
{
    [SerializeField] Transform playerOne, playerTwo;

    void Update()
    {
        // Target is center of players
        Vector3 center = (playerOne.position + playerTwo.position) / 2.0f;

        transform.position = new Vector3(center.x, 2.3f, center.z);
        //Debug.Log(center);
    }
}
