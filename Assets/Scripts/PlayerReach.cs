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
        transform.position = handTarget.transform.position;
    }
}
