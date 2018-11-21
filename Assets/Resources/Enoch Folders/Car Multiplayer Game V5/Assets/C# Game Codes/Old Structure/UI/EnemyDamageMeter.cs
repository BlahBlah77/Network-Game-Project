using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamageMeter : MonoBehaviour //IDamageable
{

    public Text enemyCarHealth;
    public GameObject mainPlayer;
    public float currentEnemyCarHealth;
    public float maxEnemyCarHealth = 100;

    // Use this for initialization
    void Start()
    {
        currentEnemyCarHealth = maxEnemyCarHealth; // set the current health to max health
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ShowCarHealth();
        CarWreaked();
    }

    void ShowCarHealth()
    {
        // show the car health
        enemyCarHealth.text = "Enemy Car Health: " + (int)currentEnemyCarHealth + "%";
    }

    void CarWreaked()
    {
        if (currentEnemyCarHealth <= 0)
        {
            currentEnemyCarHealth = 0;
            Debug.Log("You are wreaked");
            //can write more logic here to respawn 
            //back to lobby or start again...
        }
    }

    //Implement the Damage function of IDamageable
    public void Damage(float damageAmount)
    {
        currentEnemyCarHealth -= damageAmount;
    }
}
