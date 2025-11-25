using UnityEngine;

public class PlayerReach : MonoBehaviour
{
    HandTarget handTarget;
    void Start()
    {
        handTarget = HandTarget.target;
    }

    void Update()
    {
        transform.position = new Vector3(handTarget.transform.position.x, handTarget.yPos, handTarget.transform.position.z);
    }
}
