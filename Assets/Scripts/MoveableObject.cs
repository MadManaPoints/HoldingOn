using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    [SerializeField] Transform target;
    Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        float dist = Mathf.Abs(target.position.x - this.transform.position.x);

        if (dist > 0.1f)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, dir * 2.0f, Time.fixedDeltaTime * 5.0f);
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

}
