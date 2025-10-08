using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class HandTarget : MonoBehaviour
{
    public static HandTarget target;
    public int handsIn = 0;
    [SerializeField] GameObject playerOne, playerTwo;
    PlayerMovement p1, p2;
    Vector3 center;
    Vector3 targetPosition;
    Rigidbody rb;

    void Awake()
    {
        target = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        p1 = playerOne.GetComponent<PlayerMovement>();
        p2 = playerTwo.GetComponent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(targetPosition, transform.position) > 0.2f)
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
        // Target is center of players
        center = (playerOne.transform.position + playerTwo.transform.position) / 2.0f;
        targetPosition = new Vector3(center.x, 2.5f, center.z);
        //Debug.Log(center);
    }

    void CheckHands(int howMany)
    {
        if (howMany == 2)
        {
            p1.attached = true;
            p2.attached = true;
        }
        else if (p1.attached)
        {
            p1.attached = false;
            p2.attached = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Hand")
        {
            handsIn++;
            CheckHands(handsIn);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Hand")
        {
            handsIn--;
            CheckHands(handsIn);
        }
    }
}
