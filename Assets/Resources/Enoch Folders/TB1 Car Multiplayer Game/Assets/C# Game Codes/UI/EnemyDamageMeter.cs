using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamageMeter : MonoBehaviour {

    CarController carController;
    DamageMeterText playerDamage;
    public Text enemyCarHealth;
    public GameObject mainPlayer;
    public float currentCarHealth;
    public float maxCarHealth = 100;

    // Use this for initialization
    void Start ()
    {
        currentCarHealth = maxCarHealth; // set the current health to max health
        carController = mainPlayer.GetComponent<CarController>();
        playerDamage = mainPlayer.GetComponent<DamageMeterText>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        ShowCarHealth();
        CarWreaked();
    }

    void ShowCarHealth()
    {
        // show the car health
        enemyCarHealth.text = "Car Health: " + (int)currentCarHealth + "%";
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

    void TakeDamage(float rate)
    {
        currentCarHealth -= rate;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == mainPlayer)
        {
            TakeDamage(10);
            carController.UpdateNitroRate(0, 1);
            Debug.Log("player has collided with me");
        }
    }
}
