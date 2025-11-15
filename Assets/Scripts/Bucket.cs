using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Bucket : Item
{
    [SerializeField] GameObject ice;
    bool onGround;
    Rigidbody rb;
    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
    }

    protected override void Update()
    {
        base.Update();

        if (rb == null) return;
        if (ice == null && !onGround) rb.isKinematic = false;
        if (onGround)
        {
            startPos = transform.position;
            Destroy(rb);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == 10 && !onGround) onGround = true;
    }
}
