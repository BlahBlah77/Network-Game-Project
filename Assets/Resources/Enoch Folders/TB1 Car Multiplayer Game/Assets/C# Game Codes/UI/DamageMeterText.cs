using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageMeterText : MonoBehaviour {

    CarController carController;
    EnemyDamageMeter enemyDamage;
    public GameObject playerCar;
    public GameObject damage;
    public Text damageText;
    float currentCarHealth;
    float maxCarHealth = 100;
    public float collisionSpeed;
    public float currentSpeed;
    float changeInSpeed;
    float collisionTime;
    float minimumCollision = 1;
    bool isDamaging = false;
    Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = FindObjectOfType<Rigidbody>();
        currentCarHealth = maxCarHealth; // set the current health to max health
        carController = playerCar.GetComponent<CarController>();
        enemyDamage = playerCar.GetComponent<EnemyDamageMeter>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        ShowCarHealth();
        DamageSystem();
        CarWreaked();
        //Debug.Log(currentSpeed);
	}

    void ShowCarHealth()
    {
        // show the car health
        damageText.text = "Car Health: " + (int)currentCarHealth + "%";
    }

    void DamageSystem()
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

            if (changeInSpeed > minimumCollision)
            {
                currentCarHealth -= changeInSpeed;
                //enemyDamage.DamageSystem();
            }
            isDamaging = false;
        }
    }

    void CarWreaked()
    {
        if (currentCarHealth <= 0)
        {
            currentCarHealth = 0;
            Debug.Log("You are wreaked");
            //can write more logic here to respawn 
            //back to lobby or start again...
        }
    }

    void OnCollisionEnter(Collision collision)
    {
            if (collision.gameObject == damage && (currentSpeed > 0))
            {
                collisionSpeed = currentSpeed;
                //carController.UpdateNitroRate(0, 1); // gain a nitrous boost if you damage another player...
                isDamaging = true;
            }
        }
    }

