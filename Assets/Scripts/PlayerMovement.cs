using System;
using NUnit.Framework;
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


    [Header("Ground Check")]
    [SerializeField] LayerMask isGround;
    [SerializeField] bool grounded = true;
    [SerializeField] float groundDrag;

    [Header("Slope Handling")]
    [SerializeField] float maxSlopeAngle;
    RaycastHit slopeHit;


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
    public SpringJoint joint;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<SpringJoint>();
        anim = GetComponentInChildren<Animator>();
        hand = GetComponentInChildren<TwoBoneIKConstraint>();

        playerNum = (!isPlayerTwo) ? "1" : "2"; // Choose control scheme for each player
    }

    void MyInput()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal" + playerNum), 0f, -Input.GetAxis("Vertical" + playerNum));

        // Listen for Jump input
        if (Input.GetButtonDown("Jump" + playerNum) && canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown); // Wait before reseting Jump
        }

        if (Input.GetAxisRaw("Reach" + playerNum) > 0f)
        {
            hand.weight = 1.0f;
        }
        else
        {
            hand.weight = 0f;
        }

        if (attached)
        {
            state = PlayerState.holding;
            float raise = Input.GetAxisRaw("Raise" + playerNum);
            raiseHand = (raise > 0f) ? true : false;
        }
        else if (Input.GetButton("Action" + playerNum) && moveableObj != null)
        {
            state = PlayerState.pulling;
        }
        else
        {
            state = PlayerState.running;
        }
    }


    void FixedUpdate()
    {
        Movement();
    }


    void Update()
    {
        HoldHands();
        GroundCheck();
        MyInput();
        StateHandler();
        Animations();
    }


    void Movement()
    {
        Vector3 move = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.z * moveSpeed);
        if (state == PlayerState.pulling) move.z = 0f;

        // Limit slope movement
        if (OnSlope())
            rb.linearVelocity = GetSlopeMoveDirection() * moveSpeed * 10.0f;

        // Limit movement in air
        rb.linearVelocity = (grounded) ? move : new Vector3(move.x * airMult, rb.linearVelocity.y, move.z * airMult);

        // Turn off gravity on slope
        rb.useGravity = !OnSlope();

        // Handle rotation
        if (moveDir != Vector3.zero && !Input.GetButton("Action" + playerNum))
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
            moveSpeed = 0.5f;
            if (moveableObj != null) moveableObj.transform.parent = this.transform;
            anim.SetBool("isPulling", true);
        }
        else if (state == PlayerState.running)
        {
            moveSpeed = 3.5f;
            if (moveableObj != null) moveableObj.transform.parent = null;
            anim.SetBool("isPulling", false);
        }
        else
        {
            moveSpeed = 2.0f;
        }
    }


    void HoldHands()
    {
        if (!attached && joint.maxDistance != 10.0f)
        {
            // Increase joint strength to lock hands
            joint.tolerance = 1000;
            joint.maxDistance = 20.0f;
        }

        if (attached && joint.maxDistance != 0f)
        {
            // Decrease joint strength to unlock hands
            joint.tolerance = 0f;
            joint.maxDistance = 0f;
        }
    }


    void GroundCheck()
    {
        // Ground check 
        grounded = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Vector3.down, 0.7f, isGround);
        //Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Color.magenta);

        // Handle drag
        rb.linearDamping = (grounded) ? groundDrag : 0;
    }


    void Jump()
    {
        // Always start with Y Vel at 0
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.linearVelocity += Vector3.up * jumpForce;
    }


    void ResetJump()
    {
        canJump = true;
    }


    bool OnSlope()
    {
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Vector3.down, out slopeHit, 0.8f))
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


    void Animations()
    {
        if (moveDir != Vector3.zero)
            anim.SetBool("isRunning", true);
        else
            anim.SetBool("isRunning", false);

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
}
