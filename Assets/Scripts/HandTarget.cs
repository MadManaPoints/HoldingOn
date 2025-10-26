using System.Collections.Generic;
using UnityEngine;

public class HandTarget : MonoBehaviour
{
    public static HandTarget target;
    public int handsIn = 0;
    float yPos = 2.5f;
    [SerializeField] GameObject playerOne, playerTwo;
    [SerializeField] LayerMask bar, nada;
    [SerializeField] Collider boxCol;
    PlayerMovement p1, p2;
    public Vector3 center;
    Vector3 targetPosition;
    Rigidbody rb;
    Pairs pairs;
    List<SpringJoint> activeJoints = new List<SpringJoint>();


    void Awake()
    {
        target = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        p1 = playerOne.GetComponent<PlayerMovement>();
        p2 = playerTwo.GetComponent<PlayerMovement>();
        pairs = GameObject.Find("Canvas").GetComponent<Pairs>();
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(targetPosition, transform.position) > 0.2f)
        {
            Vector3 dir = (targetPosition - transform.position).normalized;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, dir * 3.0f, Time.fixedDeltaTime * 5.0f);
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 5.0f);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pairs.PairGenerator();
            pairs.active = true;
        }

        // Target is center of players
        center = (playerOne.transform.position + playerTwo.transform.position) / 2.0f;
        targetPosition = new Vector3(center.x, yPos, center.z);

        // Raise Y position along with hands
        yPos = (p1.raiseHand && p2.raiseHand) ? 3.1f : 2.5f;
        //Debug.Log(center);

        if (handsIn == 2)
            boxCol.excludeLayers = bar;
        else
            boxCol.excludeLayers = nada;

        AddJoints();

    }

    void AddJoints()
    {
        if (handsIn == 2 && activeJoints.Count == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject player = i == 0 ? playerOne : playerTwo;
                SpringJoint newJoint = player.AddComponent<SpringJoint>();
                newJoint.anchor = Vector3.up;
                newJoint.autoConfigureConnectedAnchor = false;
                newJoint.connectedAnchor = Vector3.up;
                newJoint.spring = 500f;
                newJoint.maxDistance = 0.9f;
                newJoint.tolerance = 0f;
                newJoint.connectedBody = i == 0 ? playerTwo.GetComponent<Rigidbody>() : playerOne.GetComponent<Rigidbody>();
                activeJoints.Add(newJoint);
            }
        }
        else if (handsIn < 2 && activeJoints.Count != 0)
        {
            for (int i = 0; i < activeJoints.Count; i++)
            {
                Destroy(activeJoints[i]);
                activeJoints.Remove(activeJoints[i]);
            }

            foreach (SpringJoint joint in gameObject.GetComponents<SpringJoint>())
            {
                Destroy(joint);
            }
        }
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
