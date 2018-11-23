using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTaker : MonoBehaviour, IDamageable
{
    float enemyHealth = 100;

    //public Text enemyCarHealth;

    public float GetDamage
    {
        get
        {
            return enemyHealth;
        }

        set
        {
            enemyHealth = value;
        }
    }

    public void Damage(float damageAmount)
    {
        enemyHealth -= damageAmount;
        Debug.Log( gameObject.name + " have taken: " + damageAmount);
    }

    void Update()
    {
        //enemyCarHealth.text = "Enemy Car Health: " + (int)enemyHealth + "%";

        if (enemyHealth < 0)
        {
            enemyHealth = 0;
            Debug.Log("You are wrecked");
        }
    }


}
