using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    [SerializeField] GameObject target;
    Rigidbody rb;
    float dist;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        if (dist > 0.1f)
        {
            Vector3 dir = (target.transform.position - target.GetComponent<DetectPlayer>().offset - transform.position).normalized;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, dir * 2.0f, Time.fixedDeltaTime * 5.0f);
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    void Update()
    {
        dist = Mathf.Abs(target.transform.position.x - target.GetComponent<DetectPlayer>().offset.x - this.transform.position.x);
    }

}
