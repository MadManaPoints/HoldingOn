using UnityEngine;

public class FollowHandArea : MonoBehaviour
{
    HandTarget handTarget;
    Vector3 targetPosition;
    Rigidbody rb;
    void Start()
    {
        handTarget = HandTarget.target;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(targetPosition, transform.position) > 0.1f)
        {
            Vector3 dir = (targetPosition - transform.position).normalized;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, dir * 2.0f, Time.fixedDeltaTime * 5.0f);
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 5.0f);
        }
    }

    void Update()
    {
        targetPosition = new Vector3(handTarget.transform.position.x, handTarget.transform.position.y, handTarget.transform.position.z + 0.4f);
    }
}
