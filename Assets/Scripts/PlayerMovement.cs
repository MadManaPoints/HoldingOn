using System;
using UnityEngine;

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
    public HandHolding state;
    public enum HandHolding
    {
        holding,
    }

    Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

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
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        // Ground check 
        grounded = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Vector3.down, 0.7f, isGround);
        //Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Color.magenta);

        // Handle drag
        rb.linearDamping = (grounded) ? groundDrag : 0;

        MyInput();
    }



    void Movement()
    {
        Vector3 move = new Vector3(moveDir.x * moveSpeed, rb.linearVelocity.y, moveDir.z * moveSpeed);

        // Limit slope movement
        if (OnSlope())
            rb.linearVelocity = GetSlopeMoveDirection() * moveSpeed * 10.0f;

        // Limit movement in air
        rb.linearVelocity = (grounded) ? move : new Vector3(move.x * airMult, rb.linearVelocity.y, move.z * airMult);

        // Turn off gravity on slope
        rb.useGravity = !OnSlope();

        // Handle rotation
        if (moveDir != Vector3.zero)
        {
            float angleDiff = Vector3.SignedAngle(transform.forward, moveDir, Vector3.up);
            rb.angularVelocity = new Vector3(rb.angularVelocity.x, angleDiff * 0.2f, rb.angularVelocity.z);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }

        if (rb.linearVelocity != Vector3.zero)
            anim.SetBool("isWalking", true);
        else
            anim.SetBool("isWalking", false);
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
}
