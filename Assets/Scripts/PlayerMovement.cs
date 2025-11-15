using System;
using System.Xml;
using NUnit.Framework;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [Space(5)]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMult;
    bool canJump = true;

    [Header("Pickup")]
    public bool canTransfer;
    public bool holdingItem;
    public Item item;
    public Wells well;
    [SerializeField] Transform handToHoldItem;



    [Header("Ground Check")]
    [SerializeField] LayerMask isGround;
    [SerializeField] bool grounded = true;
    [SerializeField] float groundDrag;

    [Header("Slope Handling")]
    [SerializeField] float maxSlopeAngle;
    RaycastHit slopeHit;
    bool exitingSlope;


    [Space(15)]
    public Transform orientation;

    [SerializeField] bool isPlayerTwo;
    String playerNum;

    Vector3 moveDir;

    Rigidbody rb;
    public bool attached;
    public bool raiseHand;
    bool dpad;
    public PlayerState state;
    public enum PlayerState
    {
        holding,
        running,
        pulling,
    }
    public DetectPlayer moveableObj;

    Animator anim;
    TwoBoneIKConstraint hand;
    //public SpringJoint joint;
    RaycastHit floorHit;
    bool onIce;
    Vector3 move;

    public Vector3 actualMove;
    PlayerMovement partner;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //joint = GetComponent<SpringJoint>();
        anim = GetComponentInChildren<Animator>();
        hand = GetComponentInChildren<TwoBoneIKConstraint>();

        playerNum = (!isPlayerTwo) ? "1" : "2"; // Choose control scheme for each player
    }

    void MyInput()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0f, -Input.GetAxis("Vertical"));

        // Listen for Jump input
        if (Input.GetButtonDown("Jump" + playerNum) && canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); // Wait before reseting Jump
        }

        if (Input.GetAxisRaw("Reach" + playerNum) > 0f)
        {
            hand.weight = 1.0f; // Blend animation with hand rig
        }
        else
        {
            hand.weight = 0f;
        }

        if (attached)
        {
            state = PlayerState.holding;
            float raise = Input.GetAxisRaw("Raise" + playerNum);
            raiseHand = raise > 0f ? true : false;
        }
        else if ((Input.GetButton("Action" + playerNum) || Input.GetMouseButton(1)) && moveableObj != null)
        {
            state = PlayerState.pulling;
        }
        else
        {
            state = PlayerState.running;
        }

        // Item management 
        if (holdingItem && (Input.GetButtonDown("Action" + playerNum) || Input.GetMouseButtonDown(0)))
        {
            // Allow only one transport at a time
            if (well != null && (well.inUse || well.connectedWell.inUse)) return;
            if (well != null && well.collectedItem != null) return;

            holdingItem = false;
            item.transform.SetParent(null); // Unparent from player 

            // If near well, place item inside 
            if (well != null)
            {
                well.inUse = true;
                item.well = well; // Pass reference of well script to item script
                item.transform.position = new Vector3(well.transform.position.x, well.transform.position.y + 1.0f, well.transform.position.z);
                item.TransportItem(); // Change item state 
                this.item = null; // Remove item reference from player 
            }
            else
            {
                // Drop item if not near well 
                item.Drop();
            }
        }
        else if (item != null && (Input.GetButtonDown("Action" + playerNum) || Input.GetMouseButtonDown(0)) && !holdingItem)
        {
            // Remove collected item reference from well 
            if (well != null && well.collectedItem != null)
            {
                well.collectedItem = null;
                well.inUse = false;
                well.connectedWell.inUse = false;
            }

            holdingItem = true;
            item.boxCol.enabled = false; // Make item unable to collide with objects 

            // Place item in player hand
            item.transform.position = handToHoldItem.position + item.offset;
            item.transform.SetParent(handToHoldItem);
        }
    }


    void FixedUpdate()
    {
        Movement();
    }


    void Update()
    {
        if (partner == null) AddPartner();
        GroundCheck();
        MyInput();
        StateHandler();
        Animations();
    }

    void Movement()
    {
        // Update move based on player input
        move = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.z * moveSpeed);

        // Add move to Actual as long as player's on regular ground
        if (!onIce) actualMove = move;

        // Match move for player on ice to player on regular ground while hand holding
        if (attached && onIce) actualMove = partner.actualMove;

        // Prevent forward/back movement while pulling
        if (state == PlayerState.pulling) move.z = 0f;

        if (OnSlope() && !exitingSlope)
        {   // Limit slope movement
            rb.linearVelocity = GetSlopeMoveDirection() * moveSpeed;

            // Prevent bump effect when running upward
            if (rb.linearVelocity.y > 0f) rb.AddForce(Vector3.down * 80.0f, ForceMode.Force);
        }
        else
        {
            // Limit movement in air
            if (!onIce) rb.linearVelocity = grounded ? actualMove : new Vector3(move.x * airMult, rb.linearVelocity.y, move.z * airMult);

            // Use force when sliding on ice
            else rb.AddForce(move);

            // Clamp magnitude when sliding on ice
            if (onIce && rb.linearVelocity.magnitude > 5.0f) rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 5.0f);
        }

        // Turn off gravity on slope
        rb.useGravity = !OnSlope();

        // Handle rotation
        if (moveDir != Vector3.zero && !Input.GetButton("Action" + playerNum) && state != PlayerState.pulling)
        {
            float angleDiff = Vector3.SignedAngle(transform.forward, moveDir, Vector3.up);
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, angleDiff * 0.2f, rb.angularVelocity.z);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    void StateHandler()
    {
        if (state == PlayerState.pulling)
        {
            moveSpeed = 0.3f; // Pull speed

            // Parent moveable object collider to player
            if (moveableObj != null) moveableObj.transform.parent = this.transform;
            anim.SetBool("isPulling", true);
        }
        else if (state == PlayerState.running)
        {
            moveSpeed = 3.5f; // Default speed

            // Unparent moveable object
            if (moveableObj != null) moveableObj.transform.parent = null;
            anim.SetBool("isPulling", false);
        }
        else
        {
            moveSpeed = 2.0f; // Speed while holding hands
        }
    }

    void GroundCheck()
    {
        // Ground check 
        grounded = Physics.BoxCast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.localScale * 0.25f, Vector3.down, out floorHit, transform.rotation, 0.7f, isGround);

        //grounded = Physics.Raycast(new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z), Vector3.down, 0.7f, isGround);
        //Debug.DrawLine(this.transform.position, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Color.magenta);

        // Ice check
        onIce = grounded && floorHit.transform.tag == "Ice";

        // Handle drag
        rb.linearDamping = grounded && !onIce ? groundDrag : 0;
    }


    void Jump()
    {
        exitingSlope = true;

        // Always start with Y Vel at 0
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.linearVelocity += Vector3.up * jumpForce;
    }


    void ResetJump()
    {
        canJump = true;
        exitingSlope = false;
    }


    bool OnSlope()
    {
        if (Physics.Raycast(new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z), Vector3.down, out slopeHit, 0.7f, isGround))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal); // Calculate slope steepness
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }


    Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }

    public void DestroyItem()
    {
        if (item == null) return;
        holdingItem = false;
        Destroy(item.gameObject);
    }


    void Animations()
    {
        if (moveDir != Vector3.zero || rb.angularVelocity != Vector3.zero)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }


        bool hasJumped = !grounded;
        anim.SetBool("isJumping", hasJumped);

        if (attached)
        {
            anim.SetBool("isHolding", true);
        }
        else
        {
            anim.SetBool("isHolding", false);
        }
    }

    void AddPartner()
    {
        partner = isPlayerTwo ? GameObject.Find("Player 1").GetComponent<PlayerMovement>() :
                                GameObject.Find("Player 2").GetComponent<PlayerMovement>();
    }
}
