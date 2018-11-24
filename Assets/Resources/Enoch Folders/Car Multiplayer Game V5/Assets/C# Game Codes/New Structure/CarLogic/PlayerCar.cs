using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCar : Photon.MonoBehaviour, IDamageable
{
    [Header("Car Mathematics")]
    public Rigidbody rb;
    public Transform transformWheelFrontLeft, transformWheelFrontRight;
    public Transform transformWheelRearLeft, transformWheelRearRight;
    public WheelCollider wheelFrontLeft, wheelFrontRight;
    public WheelCollider wheelRearLeft, wheelRearRight;
    private Vector3 wheelPos;
    private Quaternion wheelRot;

    [Header("Car Mechanics")]
    private float verticalMovement; // moving forward
    private float horizontalMovement; // steering wheels
    private float steeringAngle; // steering angle of the wheels mesh
    private float maxSteerAngle = 30; // max steering for the wheels
    private float maxSpeed;
    public float setMaxSpeed;
    public float actualSpeed { get { return rb.velocity.magnitude * 2.23693629f; } }

    [Header("Torque")]
    public float motorTorquePower;
    public float brakeTorquePower;
    private float newTorquePower = 1000;
    private float oldTorquePower = 500;
    public float brakingPower = 50f;

    [Header("Nitrous")]
    public float maxNitroSpeed; // maximum speed the car go in nitrous mode
    private float nitroRandNumber;
    public float changeInNitro = 5; // for the infinitro powerup
    public float currentNitro; // will be the max nitro
    public float maxNitro = 100;// how much nitrous do we have.
    private bool isHandBraking; // are we handbraking
    private bool isBraking; // are we braking
    private bool isNitrousOn = false; //Nitrous system pressing E and out
  
    [Header("Player Lives")]
    public float currentLives;
    public float maxLives = 3;

    [Header ("Player Health Bar")]
    public Image healthbar;
    public float maxCarHealth = 100; // the maximum amount of health the car can have
    public float currentCarHealth; // current car health thats in game

    [Header("Player Damage System")]
    public float collisionSpeed; // a variable to swap with the current speed
    public float currentSpeed; // the current speed of the car 
    public float changeInSpeed; // the speed that the car changes by
    public float damageRate; // multiplier that alters how much damage the car can do to another car
    public float changeInDamage; //TEST FOR THE SHIELD POWER UP... MODIFIED LATER ON WITH COLLISIONS....
    float collisionTime; // time.deltatime variable helper
    public float minCollisionSpeed = 1; // the variable that helps define if the current speed of the car is greater than 1mph

    // Player UI
    [Header("Player UI")]
    public Text speedText;
    public Text nitroText;
    public Text carLivesText;
    public Text score;
    public Text rankingText;
    public Text timerText;

    [Header("Timer")]
    private float startTime;
    private bool finished = false;
    private bool timerPause = true;

    [Header("Score")]
    public static int scoreValue = 0;
    public static int maxScore = 50;
    private int randomScore;



    void Start()
    {
        OnPlayerStart();
    }

    void Update()
    {
        //if (photonView.isMine)
        {
            CarMechanics();
            ShowAllPlayerUI();
            CarWreaked(); // TESTTTINNGGG
            PressKeyToDamage();
        }

    }

    IEnumerator HoldTimer(float time)
    {
        yield return new WaitForSeconds(5);
        startTime = Time.time;
        timerPause = false;
    }

    void OnPlayerStart()
    {
        // Initialize everything the player needs to be ready to start the game

        if (!photonView.isMine)
        {
            //return;
        }

        rb = GetComponent<Rigidbody>(); // get the rigidbody thats attached to the car..
        StartCoroutine(HoldTimer(5)); // wait 5 seconds before starting the timer
        currentNitro = maxNitro; // current nitro is now equal to the max nitro (100 at the start)
        currentCarHealth = maxCarHealth;
        currentLives = maxLives; // current player lives is 3
    }

    #region ("Car Movement Mechanics")
    private void CarMechanics()
    {
        SetInput();
        SteerCar();
        AccelerateCar();
        NewNitroSystem();
        HandBrakeCar();
        CarBraking();
        UpdateNitroValue();
        UpdateWheelMotions();
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
        maxSpeed = setMaxSpeed;

        if (actualSpeed >= setMaxSpeed)
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
    void NewNitroSystem()
    {
        // pass the max speed assigned and set it in another variable.
        maxSpeed = setMaxSpeed;

        // if the 'E' key is PRESSED
        // and the nitrous has not been activated
        // turn the nitrous on and increase the torque power of the car.
        // with nitrous on the car has the ability to reach the max nitro speed
        // else if 'E' key is RELEASED then revert back to the normal torque power of the car (no nitrous)
        if (Input.GetKey(KeyCode.E) && !isNitrousOn)
        {
            isNitrousOn = true;
            motorTorquePower = newTorquePower;
            setMaxSpeed = maxNitroSpeed;
            //Debug.Log("Nitro activated");
        }
        else if (Input.GetKeyUp(KeyCode.E) && isNitrousOn)
        {
            isNitrousOn = false;
            motorTorquePower = oldTorquePower;
            //Debug.Log("nitrous deactivated");
        }

        // if the nitro value is equal to zero OR is equal to 5
        // dont boost...
        if (Input.GetKey(KeyCode.E) && isNitrousOn && (currentNitro <= 0) || (currentNitro <= 5))
        {
            isNitrousOn = false;
            motorTorquePower = oldTorquePower;
        }
    }
    void UpdateNitroRate(float downRate, float upRate)
    {
        // Base function
        // this function allows the ability to
        // either..
        // decrease the nitro value when pressed
        // increase the nitro value when released

        currentNitro -= downRate * Time.deltaTime;
        currentNitro += upRate;
    }
    void UpdateNitroValue()
    {
        // This function updates the nitro value based on whether the E key is pressed or not
        // If the key is pressed, decrease the nitro value

        if (Input.GetKey(KeyCode.E))
        {
            UpdateNitroRate(changeInNitro, 0);
            //Debug.Log("Decreased Nitro Value");
        }

        // Safety Checks
        // Making sure the values are set to 0 and 100
        // If these numbers are ever reached.
        if (currentNitro <= 0)
        {
            currentNitro = 0;
        }

        if (currentNitro > 100)
        {
            currentNitro = 100;
        }
    }
    void HandBrakeCar()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Debug.Log("BRAKING!!");
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
    #endregion 

    #region ("Damage System Interface")
    float GetCurrentSpeed()
    {
        return rb.velocity.magnitude * 2.23693629f;
    }
    void CarCollision(Collision collision)
    {
        //Reference to hit object
        GameObject hitObject = collision.gameObject;
        IDamageable damgeObject = hitObject.GetComponent<IDamageable>();

        // If it is network then the photon view will work
        // If it is not networked it will still run the code..
        if (!photonView.isMine || PhotonNetwork.connected == false)
        {
            //Reference to attached Damageable interface (if there is one)
            //If the car that we are hitting has the IDamageable interface attached they will take damage.
            if (hitObject.GetComponent<IDamageable>() != null)
            {
                collisionSpeed = GetCurrentSpeed(); // get the speed of the car

                if (Time.time > collisionTime)
                {
                    if (collisionSpeed > minCollisionSpeed)
                    {
                        nitroRandNumber = Random.Range(5.0f, 10.0f);
                        randomScore = Random.Range(1, 11);
                        damgeObject.Damage(collisionSpeed * damageRate); // apply damage to other car
                        UpdateNitroRate(0, nitroRandNumber); // replenish player nitro if used 
                        AddScore(randomScore); // add score 
                        Debug.Log(collisionSpeed * damageRate);
                    }
                }
            }
        }

        // When its networked
        //if (photonView.isMine) //|| PhotonNetwork.connected == false)
        //{
        //    //Reference to attached Damageable interface (if there is one)
        //    //If the car that we are hitting has the IDamageable interface attached they will take damage.
        //    if (hitObject.GetComponent<IDamageable>() != null)
        //    {
        //        collisionSpeed = GetCurrentSpeed(); // get the speed of the car

        //        if (Time.time > collisionTime)
        //        {
        //            if (collisionSpeed > minCollisionSpeed)
        //            {
        //                nitroRandNumber = Random.Range(5.0f, 10.0f);
        //                Debug.Log(collisionSpeed * damageRate);
        //                damgeObject.Damage(collisionSpeed * damageRate);
        //                UpdateNitroRate(0, nitroRandNumber); //Update Nitro 
        //                AddScore(10);
        //            }
        //        }
        //    }
        //}
    }

    void OnCollisionEnter(Collision collision)
    {
        CarCollision(collision);
    }
    #endregion

    // UI FUNCTIONS
    #region ("Player UI")
    void ShowAllPlayerUI()
    {
        ShowSpeedUI();
        ShowNitrousUI();
        ShowLivesUI();
        RemoveLives();
        ShowHealth();
        ShowScore();
        ShowRankingScore();
        StartTimer();
    }

    #region ("All Callable Player UI Functions")
    void ShowSpeedUI()
    {
        speedText.text = "Speed: " + (int)actualSpeed + " mph";
    }
    void ShowNitrousUI()
    {
        nitroText.text = "Nitrous: " + (int)currentNitro + " %";
    }
    void ShowLivesUI()
    {
        // show lives
        carLivesText.text = "Lives: " + (int)currentLives;
    }
    void RemoveLives()
    {
        // Take -1 from players life if the players life is zero
        if (currentCarHealth <= 0 && currentLives <= 3)
        {
            currentLives--;
            currentCarHealth = 100;

            //SPACE TO WRITE NETWORK CODE HERE...
            //Respawning back into game lobby and getting car back...
        }


        if (currentLives <= 0)
        {
            Debug.Log("You are wrecked!");
            currentLives = 0;
            currentCarHealth = 0;
            StopAllCoroutines(); 

            //SPACE TO WRITE NETWORK CODE HERE...
            //Out of the game can no longer play
        }

        if (currentCarHealth == 0)
        {
            this.gameObject.SetActive(false);
            Debug.Log("Player is out of the game");
        }
    }
    void ShowHealth()
    {
        healthbar.fillAmount = currentCarHealth / maxCarHealth;
    }
    void ShowScore()
    {
        score.text = "Score: " + scoreValue;
    }
    void ShowRankingScore()
    {
        // Initial ranking of nothing until conditions are met.
        rankingText.text = "Ranking: ";

        if (scoreValue >= 10 && scoreValue <= 19)
        {
            rankingText.text = "Ranking: Bronze";
        }

        if (scoreValue >= 20 && scoreValue <= 29)
        {
            rankingText.text = "Ranking: Silver";
        }

        if (scoreValue >= 30 && scoreValue <= 39)
        {
            rankingText.text = "Ranking: Gold";
        }

        if (scoreValue >= 40 && scoreValue <= 49)
        {
            rankingText.text = "Ranking: Platinum";
        }

        if (scoreValue >= 50)
        {
            scoreValue = maxScore;
            rankingText.text = "Max Score!";
        }
    }
    
    #endregion

    // Setters/Miscillaneous Functions
    void AddScore(int amount)
    {
        scoreValue += amount;
    }
    void StartTimer()
    {
        // Timer Function
        if (!timerPause)
        {
            RunTimer();
        }
    }
    void RunTimer()
    {
        // Stops Timer when score is equal to the MaxScore, Change if necessary 
        if (finished)
            return;

        if (scoreValue == maxScore)
        {
            finished = true;
        }

        // Timer Code
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        timerText.text = "Time: " + minutes + ":" + seconds;
    }
    void CarWreaked()
    {
        if (currentCarHealth <= 0)
        {
            currentCarHealth = 0;
            Debug.Log("You are wreaked");
        }
    }
    #endregion


    //Remember all Interface functions need to be public
    public void Damage(float damageAmount)
    {
        // take the enemy's car health down...
        currentCarHealth -= damageAmount;
        Debug.Log("Hit the car");
    }

    public float GetDamage
    {
        get
        {
            throw new System.NotImplementedException();
        }

        set
        {
            throw new System.NotImplementedException();
        }
    }


    void TestCarDamage(float damageRate)
    {
        currentCarHealth -= damageRate * Time.deltaTime;
    }

    // TESTING FUNCTION FOR SIMULATING PLAYER DAMAGE!!
    void PressKeyToDamage()
    {
        if (Input.GetKey(KeyCode.R))
        {
            TestCarDamage(changeInDamage);
        }
    }
}
