using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// DAMAGE SYSTEM FOR THE PLAYER CAR..
// MUST BE REDONE AND PUT IN THE PLAYER CLASS...
// REDO THIS CLASS...


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
        PressKeyToDamage();
    }

    void ShowCarHealth()
    {
        // show the car health
        carHealthText.text = "Player Car Health: " + (int)currentCarHealth + "%";
    }

    /// <summary>
    /// Returns the current speed
    /// </summary>
    /// <returns></returns>
    float GetCurrentSpeed()
    {
        return rb.velocity.magnitude * 2.23693629f;
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
        //Reference to hit object
        GameObject hitObject = collision.gameObject;


        //Reference to attached Damageable interface (if there is one)
        //If the car that we are hitting has the IDamageable interface attached they will take damage.
        if (hitObject.GetComponent<IDamageable>() != null)
        {
            IDamageable damgeObject = hitObject.GetComponent<IDamageable>();

            collisionSpeed = GetCurrentSpeed(); // get the speed of the car

            if (Time.time > collisionTime)
            {
                if (collisionSpeed > minCollisionSpeed)
                {
                    Debug.Log(collisionSpeed * damageRate);
                    damgeObject.Damage(collisionSpeed * damageRate);

                    //Nitro logic here**************
                }
            }
        }
        
        /*if (collision.gameObject == enemyCar && (GetCurrentSpeed() >= 0))
        {
            float nitroRandNumber = Random.Range(5.0f, 10.0f);
            enemyDamage = collision.gameObject.GetComponent<EnemyDamageMeter>(); // get the enemy car that your damaging
            DamageSystem();
            collisionSpeed = currentSpeed; // whatever the speed of the car to cause collision will now be the collision speed
            carController.UpdateNitroRate(0, nitroRandNumber); // gain a nitrous boost if you damage another player...

            

            //isDamaging = true;
            Debug.Log(nitroRandNumber);
        }*/
    }
}

