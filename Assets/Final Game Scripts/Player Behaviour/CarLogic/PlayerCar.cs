﻿using System.Collections;
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

    private Vector3 Offset_Centre_Mass = new Vector3(0, 0, 0);

    [Header("Car Mechanics")]
    private float verticalMovement; // moving forward
    private float horizontalMovement; // steering wheels
    private float steeringAngle; // steering angle of the wheels mesh
    private float maxSteerAngle = 30; // max steering for the wheels
    private float maxSpeed;
    public float setMaxSpeed;
    public float actualSpeed { get { return rb.velocity.magnitude * 2.23693629f; } }
    private bool isHandBraking; // are we handbraking
    private bool isBraking; // are we braking

    [Header("Torque")]
    public float motorTorquePower;
    public float brakeTorquePower;
    private float newTorquePower = 2000;
    private float oldTorquePower = 1500;
    public float brakingPower = 1500f;

    [Header("Nitrous")]
    public float maxNitroSpeed; // maximum speed the car go in nitrous mode
    private float nitroRandNumber;
    public float changeInNitro = 5; // modifier for the infinite nitro power up and decreases nitrous with a value of 5
    private bool isNitrousOn = false; //Nitrous system pressing E and out
  
    [Header("Player Lives")]
    public float currentLives;
    public float maxLives = 3;

    [Header ("Player Health Bar")]
    public Image healthbar;
    public float maxCarHealth = 100; // the maximum amount of health the car can have
    public float currentCarHealth; // current car health thats in game

    [Header("Player Nitrous Bar")]
    public Image nitrousBar;
    public float currentNitro; // will be the max nitro
    public float maxNitro = 100;// how much nitrous do we have.

    [Header("Player Damage System")]
    public float collisionSpeed; // a variable to swap with the current speed
    public float changeInSpeed; // the speed that the car changes by
    public float damageRate; // multiplier that alters how much damage the car can do to another car
    public float changeInDamage; //TEST FOR THE SHIELD POWER UP... MODIFIED LATER ON WITH COLLISIONS....
    float collisionTime; // time.deltatime variable helper
    public float minCollisionSpeed = 150f; // the variable that helps define if the current speed of the car is greater than 1mph
    private bool canTakeDamage = true;
    public float coolDownTime = 0.2f;

    // Player UI
    [Header("Player UI")]
    public Text speedText;
    public Text carLivesText;
    public Text score;
    public Text rankingText;
    public Text timerText;
    public Text stateText;
    public bool isDead = false;
    public List<PlayerCar> players;
    public Button returnButton;

    [Header("Timer")]
    private float startTime;
    private bool finished = false;
    private bool timerPause = true;

    [Header("Score")]
    public static int scoreValue = 0;
    public static int maxScore = 50;
    private int randomScore;

    [Header("Cameras/Canvas")]
    public Camera mainCamera;
    public Camera minimapCamera;
    public Canvas mainCan;



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(currentCarHealth);
            stream.SendNext(currentNitro);
            stream.SendNext(currentLives);
            stream.SendNext(isDead);
        }
        else
        {
            currentCarHealth = (float)stream.ReceiveNext();
            currentNitro = (float)stream.ReceiveNext();
            currentLives = (float)stream.ReceiveNext();
            isDead = (bool)stream.ReceiveNext();
        }
    }

    void Start()
    {
        OnPlayerStart();
    }

    void Update()
    {
        if (!photonView.isMine)
        {
            return;
        }
 
        CarMechanics();
        ShowAllPlayerUI();
    }

    public void VicChecker()
    {
        var obj = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < obj.Length; i++)
        {
            players.Add(obj[i].GetComponent<PlayerCar>());
        }
        if (players.Count == 1)
        {
            Debug.Log("We came in?");
            stateText.text = "Victory";
            stateText.gameObject.SetActive(true);
            returnButton.gameObject.SetActive(true);
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

        rb = GetComponent<Rigidbody>(); // get the rigidbody thats attached to the car..
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -0.9f, 0.2f); // centre of mass to keep car stable
        StartCoroutine(HoldTimer(5)); // wait 5 seconds before starting the timer
        currentNitro = maxNitro; // current nitro is now equal to the max nitro (100 at the start)
        currentCarHealth = maxCarHealth;
        currentLives = maxLives; // current player lives is 3

        var objs = GameObject.FindGameObjectsWithTag("Player");
        foreach (var obj in objs)
        {
            obj.GetComponent<PlayerCar>().players.Add(this);
        }

        if (!photonView.isMine)
        {
            // Disables unneeded cameras and Ui
            mainCamera.gameObject.SetActive(false);
            minimapCamera.gameObject.SetActive(false);
            mainCan.gameObject.SetActive(false);
            return;
        }
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
        //VicChecker();
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

    IEnumerator ReactivateDamage()
    {
        yield return new WaitForSeconds(coolDownTime);

        canTakeDamage = true;
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

                        // --- WORKING OFFLINE FOR DAMAGE SYSTEM --- //
                        canTakeDamage = false;
                        StartCoroutine(ReactivateDamage());


                        UpdateNitroRate(0, nitroRandNumber); // replenish player nitro if used 
                        AddScore(randomScore); // add score 
                        GenerateSparks();
                        Debug.Log(collisionSpeed * damageRate);
                    }
                }
            }
        }

        // When its networked
        if (photonView.isMine) //|| PhotonNetwork.connected == false)
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
                        Debug.Log(collisionSpeed * damageRate);
                        damgeObject.Damage(collisionSpeed * damageRate);

                        // --- ACTIVATE THIS CODE ONLINE FOR THE DAMAGE TO WORK --- //
                        canTakeDamage = false;
                        StartCoroutine(ReactivateDamage());

                        UpdateNitroRate(0, nitroRandNumber); //Update Nitro 
                        AddScore(10);
                    }
                }
            }
        }
    }

    void GenerateSparks()
    {
        //    initialise a game object and call the method from the object pooler class created to get the particle gameobject.
        //    Check if the particle is null to ensure no errors, else return back.
        //    When the particle is in contact with the player
        //     We want to instantiate the particle from the position and the rotation of where it hit the player from
        //     So whichever angle it hits the player thats where the particle will instantiate from

        // WORKING SPARK PARTICLE EFFECT
        Debug.Log("Sparks Hit The Car");
        GameObject particle = ParticlePooling.particlePool.GetSparkParticle();
        if (particle == null) return;
        particle.transform.position = transform.position;
        particle.transform.rotation = transform.rotation;
        particle.SetActive(true);
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
        speedText.text = "SPEED : " + (int)actualSpeed + " mph";
    }
    void ShowNitrousUI()
    {
        nitrousBar.fillAmount = currentNitro / maxNitro;
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
        }


        if (currentLives <= 0)
        {
            Debug.Log("You are wrecked!");
            currentLives = 0;
            currentCarHealth = 0;
            StopAllCoroutines(); 
        }

        if (currentCarHealth == 0)
        {
            isDead = true;
            if (photonView.isMine)
            {
                stateText.text = "You Lose";
                stateText.gameObject.SetActive(true);
                PhotonNetwork.LeaveRoom();
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);

            }
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
        rankingText.text = "Ranking : ";

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
    #endregion


    //Remember all Interface functions need to be public
    public void Damage(float damageAmount)
    {
        // take the enemy's car health down...

        if (canTakeDamage == false)
        {
            return;
        }

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

    void OnPhotonPlayerDisconnected(PhotonPlayer newPlay)
    {
        players = new List<PlayerCar>();
        Debug.Log("Isn't this where");
        StartCoroutine(DisconnectTime());
    }

    IEnumerator DisconnectTime()
    {
        yield return new WaitForSeconds(1.5f);
        VicChecker();

    }

    // TESTING FUNCTION FOR SIMULATING PLAYER DAMAGE!!
    // WILL BE DELETED LATER
    void TestCarDamage(float damageRate)
    {
        currentCarHealth -= damageRate * Time.deltaTime;
    }
}
