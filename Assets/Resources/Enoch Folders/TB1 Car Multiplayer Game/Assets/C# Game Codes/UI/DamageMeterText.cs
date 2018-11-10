using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageMeterText : MonoBehaviour {

    [SerializeField]
    EnemyDamageMeter enemyDamage;
    CarController carController;
    public GameObject playerCar;
    public GameObject enemyCar;
    public Text carHealthText;
    private Rigidbody rb;

    public float maxCarHealth = 100; // the maximum amount of health the car can have
    public float currentCarHealth; // current car health thats in game
    public float collisionSpeed; // a variable to swap with the current speed
    public float currentSpeed; // the current speed of the car 
    public float changeInSpeed; // the speed that the car changes by
    public float damageRate; // multiplier that alters how much damage the car can do to another car
    public float changeInDamage; //TEST FOR THE SHIELD POWER UP... MODIFIED LATER ON WITH COLLISIONS....
    float collisionTime; // time.deltatime variable helper
    float minCollisionSpeed = 1; // the variable that helps define if the current speed of the car is greater than 1mph


    bool isDamaging = false; // are we damaging the enemy car?


    // Use this for initialization
    void Start()
    {
        rb = FindObjectOfType<Rigidbody>();
        currentCarHealth = maxCarHealth; // set the current health to max health
        carController = playerCar.GetComponent<CarController>(); // get the Car Controller script
        enemyDamage = playerCar.GetComponent<EnemyDamageMeter>(); // get the Enemy Damage script
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ShowCarHealth();
        DamageSystem();
        PressKeyToDamage();
    }

    void ShowCarHealth()
    {
        // show the car health
        carHealthText.text = "Player Car Health: " + (int)currentCarHealth + "%";
    }

    public void DamageSystem()
    {
        // get the car's current speed mph
        currentSpeed = rb.velocity.magnitude * 2.23693629f;


        // if the game time is greater than collision time
        // and damaging is happening on OnCollisionEnter
        // take health away based on how fast/slow the car went
        // damage depends on how fast/slow the car is going.
        // if you go slow it will give little damage and vice versa...
        if (Time.time > collisionTime && isDamaging)
        {
            changeInSpeed = collisionSpeed - currentSpeed;

            if (changeInSpeed > minCollisionSpeed)
            {
                // gives damage to enemy player car..
                enemyDamage.currentEnemyCarHealth -= changeInSpeed * damageRate;
            }

            isDamaging = false;
        }
    }

    void TestCarDamage(float damageRate)
    {
        currentCarHealth -= damageRate * Time.deltaTime;

    }

    void PressKeyToDamage()
    {
        if(Input.GetKey(KeyCode.R))
        {
            TestCarDamage(changeInDamage);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        CarCollision(collision);
    }

    public void CarCollision(Collision collision)
    {
        if (collision.gameObject == enemyCar && (currentSpeed >= 0))
        {
            float nitroRandNumber = Random.Range(5.0f, 10.0f);
            enemyDamage = collision.gameObject.GetComponent<EnemyDamageMeter>(); // get the enemy car that your damaging
            collisionSpeed = currentSpeed; // whatever the speed of the car to cause collision will now be the collision speed
            carController.UpdateNitroRate(0, nitroRandNumber); // gain a nitrous boost if you damage another player...
            isDamaging = true;
            Debug.Log(nitroRandNumber);
        }
    }
}

