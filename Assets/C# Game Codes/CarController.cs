using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

    public float verticalMovement;
    public float horizontalMovement;
    public float brakeMotion;
    public float steeringAngle;
    public float maxSteerAngle = 30;
    public float motorTorquePower = 50;
    public float brakeTorquePower = 100;
    public float brakingPower = 50f;
    public float MaxSpeed;
    public float StandardMaxSpeed = 150;

    private float originalRotX;
    private float originalRotZ;

    public float actualSpeed { get { return rb.velocity.magnitude * 2.23693629f; } }

    Rigidbody rb;
    private Vector3 wheelPos;
    private Quaternion wheelRot;

    public Transform transformWheelFrontLeft, transformWheelFrontRight;
    public Transform transformWheelRearLeft, transformWheelRearRight;

    public WheelCollider wheelFrontLeft, wheelFrontRight;
    public WheelCollider wheelRearLeft, wheelRearRight;

    private bool isBraking;
    private bool isHandBraking;

    private void Start()
    {
        rb = FindObjectOfType<Rigidbody>();

        originalRotX = transform.rotation.x;
        originalRotZ = transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        SetInput();
        SteerCar();
        AccelerateCar();
        HandBrakeCar();
        CarBraking();
        UpdateWheelMotions();
        CarRotationCheck();
    }

    void SetInput()
    {
       // initialize and set up inputs for the car... 
       // moving forwards and backwards using the WASD or Arrow Keys
       verticalMovement = Input.GetAxis("Vertical");
       horizontalMovement = Input.GetAxis("Horizontal");
    }

    void SteerCar()
    {
        // set the steering angle to the maximum steer angle times by the horizontal movement variable
        // Assign them to the left and right wheels.
        steeringAngle = maxSteerAngle * horizontalMovement;
        wheelFrontLeft.steerAngle = steeringAngle;
        wheelFrontRight.steerAngle = steeringAngle;

    }

    void AccelerateCar()
    {
        MaxSpeed = StandardMaxSpeed;

        if (actualSpeed > MaxSpeed)
        {
            verticalMovement = 0;
        }

        // set the motor torque power to the wheel colliders motorTorque member variable times by the vertical movement
        // Assign them to the left and right wheels. or vice versa doesnt matter really.
        wheelFrontLeft.motorTorque = motorTorquePower * verticalMovement;
        wheelFrontRight.motorTorque = motorTorquePower * verticalMovement;
        wheelRearLeft.motorTorque = motorTorquePower * verticalMovement;
        wheelRearRight.motorTorque = motorTorquePower * verticalMovement;

    }

    void HandBrakeCar()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("BRAKING!!");
            isHandBraking = true;
        }
        else
        {
            isHandBraking = false;
        }

        if (isHandBraking)
        {
            wheelFrontLeft.brakeTorque = brakeTorquePower;
            wheelFrontRight.brakeTorque = brakeTorquePower;

            wheelRearLeft.brakeTorque = brakeTorquePower;
            wheelRearRight.brakeTorque = brakeTorquePower;

            wheelFrontLeft.motorTorque = 0;
            wheelFrontRight.motorTorque = 0;

            wheelRearLeft.motorTorque = 0;
            wheelRearRight.motorTorque = 0;
        }
        else if (!isHandBraking && (Input.GetButton("Vertical") == true))
        {
            wheelFrontLeft.brakeTorque = 0;
            wheelFrontRight.brakeTorque = 0;
            wheelRearLeft.brakeTorque = 0;
            wheelRearRight.brakeTorque = 0;
        }
    }


    void CarBraking()
    {
        float carReverseMovement = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.S))
        {
            isBraking = true;
        }
        else
        {
            isBraking = false;
        }

        if (isBraking)
        {
            wheelFrontLeft.brakeTorque = brakingPower;
            wheelFrontRight.brakeTorque = brakingPower;
            wheelRearLeft.brakeTorque = brakingPower;
            wheelRearRight.brakeTorque = brakingPower;
        }

        if (isBraking)
        {
            wheelFrontLeft.motorTorque = carReverseMovement * motorTorquePower;
            wheelFrontRight.motorTorque = carReverseMovement * motorTorquePower;
            wheelRearLeft.motorTorque = carReverseMovement * motorTorquePower;
            wheelRearRight.motorTorque = carReverseMovement * motorTorquePower;

            wheelFrontLeft.brakeTorque = 0;
            wheelFrontRight.brakeTorque = 0;
            wheelRearLeft.brakeTorque = 0;
            wheelRearRight.brakeTorque = 0;
        }
    }


    void UpdateWheelMotions()
    {
        // This method updates all the wheel colliders and the wheel mesh in real time as they ride together.
        UpdateWheelMotion(wheelFrontLeft, transformWheelFrontLeft);
        UpdateWheelMotion(wheelFrontRight, transformWheelFrontRight);
        UpdateWheelMotion(wheelRearLeft, transformWheelRearLeft);
        UpdateWheelMotion(wheelRearRight, transformWheelRearRight);
    }

    void UpdateWheelMotion(WheelCollider wheelC, Transform wheelT)
    {
        wheelPos = transform.position;
        wheelRot = transform.rotation;

        // This gets the information of the wheel collider and transform
        // Returns the infromation with GetWorldPos
        // To align the wheels correctly to spin the right way 
        wheelC.GetWorldPose(out wheelPos, out wheelRot);

        // Then we pass the information into our method parameters...
        // Both position and rotation.
        wheelT.transform.position = wheelPos;
        wheelT.transform.rotation = wheelRot;
    }

    void CarRotationCheck()
    {
        if (Input.GetKeyDown ("t"))
        {
            ResetCar();
        }

        if (Vector3.Dot(transform.up, Vector3.down) > 0)
        {
            ResetCar();
        }
    }

    void ResetCar()
    {
        transform.Translate(0, -1, 0);

        transform.rotation = (Quaternion.Euler(new Vector3(originalRotX, transform.rotation.y, originalRotZ)));
    }
}
