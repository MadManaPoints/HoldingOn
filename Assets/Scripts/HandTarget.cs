using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HandTarget : MonoBehaviour
{
    public static HandTarget target;
    public int handsIn = 0;
    public float yPos = 2.5f;
    [SerializeField] GameObject playerOne, playerTwo;
    [SerializeField] LayerMask bar, nada;
    [SerializeField] Collider boxCol;
    PlayerMovement p1, p2;
    public Vector3 center;
    Vector3 dir;
    Vector3 targetPosition;
    Rigidbody rb;
    Pairs pairs;
    public bool keepHeld;
    [SerializeField] float moveSpeed;
    List<SpringJoint> activeJoints = new List<SpringJoint>();

    void Awake()
    {
        target = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pairs = GameObject.Find("Canvas").GetComponent<Pairs>();

        //handsIn = GameManager.Instance.tutorial ? 2 : 0;
        keepHeld = GameManager.Instance.tutorial;
    }

    void FixedUpdate()
    {
        //if (Vector3.Distance(targetPosition, transform.position) < 0.1f)
        if (Vector3.Distance(targetPosition, transform.position) < 0.1f)
        {
            rb.linearVelocity = Vector3.zero; //Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 5.0f);
        }
        else if (Vector3.Distance(targetPosition, transform.position) > 0.2f)
        {
            rb.linearVelocity = dir * moveSpeed; //Vector3.Lerp(rb.linearVelocity, dir * 3.0f, Time.fixedDeltaTime * 20.0f);
        }
    }

    void Update()
    {
        CheckForPlayers();

        // Target is center of players
        center = (playerOne.transform.position + playerTwo.transform.position) / 2.0f;

        // Raise Y position along with hands
        if (p1.raiseHand && p2.raiseHand)
        {
            if (yPos < 3.1f) yPos += Time.deltaTime * 2.0f;
            else yPos = 3.1f;
        }
        else
        {
            if (yPos > 2.5f) yPos -= Time.deltaTime * 2.0f;
            else yPos = 2.5f;
        }

        targetPosition = new Vector3(0f, 2.5f, center.z);
        dir = (targetPosition - new Vector3(0f, transform.position.y, transform.position.z)).normalized;

        AddJoints();
        GameManager.Instance.playersTogether = handsIn == 2;

        //transform.position = targetPosition;
        if (!p1.attached && !p2.attached)
        {
            // Turn off collisions when players are not holding hands
            boxCol.enabled = false;
            return;
        }
        else if (!boxCol.enabled)
            boxCol.enabled = true;

        if (handsIn == 2 && yPos < 3.0f)
            // Exclude small poles when hands are raised 
            boxCol.excludeLayers = bar;
        else
            // Excluse everything except for large poles
            boxCol.excludeLayers = nada;
    }

    void CheckForPlayers()
    {
        if (playerOne == null)
        {
            playerOne = GameObject.Find("Player 1");
            p1 = playerOne.GetComponent<PlayerMovement>();
        }

        if (playerTwo == null)
        {
            playerTwo = GameObject.Find("Player 2");
            p2 = playerTwo.GetComponent<PlayerMovement>();
        }

        if (playerOne == null || playerTwo == null) return;
    }


    void AddJoints()
    {
        if (handsIn == 2 && activeJoints.Count == 0)
        {
            // Attach players to the hand target object
            for (int i = 0; i < 2; i++)
            {
                GameObject player = i == 0 ? playerOne : playerTwo;
                SpringJoint newJoint = player.AddComponent<SpringJoint>();
                newJoint.connectedBody = rb;
                newJoint.anchor = Vector3.up;
                newJoint.autoConfigureConnectedAnchor = true;
                newJoint.connectedAnchor = Vector3.up;
                newJoint.spring = 2000f;
                newJoint.damper = 0f;
                newJoint.maxDistance = 0f;
                newJoint.tolerance = 0f;
                //newJoint.connectedBody = i == 0 ? playerTwo.GetComponent<Rigidbody>() : playerOne.GetComponent<Rigidbody>();
                activeJoints.Add(newJoint);
            }
        }
        else if (handsIn == 0 && activeJoints.Count != 0)
        {
            // Remove joint components from players
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

    public void CheckHands(int howMany)
    {
        if (GameManager.Instance.bindPlayers) return;

        if (howMany == 2)
        {
            p1.attached = true;
            p2.attached = true;
        }
        else if (!p1.r && !p2.r)
        {
            p1.attached = false;
            p2.attached = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Hand")
        {
            if (handsIn == 2) return;
            handsIn++;
            CheckHands(handsIn);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Hand")
        {
            //handsIn--;
            //CheckHands(handsIn);
        }
    }
}
