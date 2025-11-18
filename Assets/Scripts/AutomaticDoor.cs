using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    [SerializeField] FloorButton button;
    [SerializeField] bool secondDoor;
    HingeJoint joint;
    JointMotor motor;

    void Start()
    {
        joint = GetComponent<HingeJoint>();
        motor = joint.motor;
    }

    void Update()
    {
        DoorMovement();
    }

    void DoorMovement()
    {
        if ((button.on && !secondDoor) || (!button.on && secondDoor))
        {
            if (motor.targetVelocity < 0f)
            {
                motor.targetVelocity *= -1;
                joint.motor = motor;
            }

            if (!joint.useMotor && joint.angle < joint.limits.max) joint.useMotor = true;
            else if (joint.useMotor && joint.angle >= joint.limits.max) joint.useMotor = false;
        }
        else
        {
            if (motor.targetVelocity > 0f)
            {
                motor.targetVelocity *= -1;
                joint.motor = motor;
            }

            if (!joint.useMotor && joint.angle > joint.limits.min) joint.useMotor = true;
            else if (joint.useMotor && joint.angle <= joint.limits.min) joint.useMotor = false;
        }
    }
}
