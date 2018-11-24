using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour {

    public GameObject player;
    GameObject particleShieldEffect;
    PlayerCar carController;
    float timer = 20;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            carController = player.GetComponent<PlayerCar>();
            carController.currentCarHealth = carController.maxCarHealth;
            StartCoroutine(ShieldPickup(other));
        }
    }

    IEnumerator ShieldPickup(Collider Player)
    {
        // Instantiate some particle effects here...
        //Instantiate(particleShieldEffect, transform.position, transform.rotation);
        carController = player.GetComponent<PlayerCar>();
        carController.changeInDamage = 0; // testing for simulating player damage 
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Invoke("RespawnInvincibility", 5f); // respawn back the powerup after 2 seconds (testing)
        yield return new WaitForSeconds(timer);
        carController.changeInDamage = 5; // reset the damage back to five (testing)
    }

    void RespawnInvincibility()
    {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
