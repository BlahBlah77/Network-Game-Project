using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour {

    public GameObject[] players;
    GameObject particleShieldEffect;
    PlayerCar carController;
    float timer = 20;

    void Start()
    {
        // Find the players present in the arena that have the player tag
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        // loop through all the players in the array
        // if ONE or more of the players collides with the powerup
        // Start the couroutine for the powerup logic
        //foreach (GameObject player in players)
        //{
        //    if (other.gameObject == player)
        //    {
        //        GenerateWaterSplash();
        //        carController = player.GetComponent<PlayerCar>();
        //        carController.currentCarHealth = carController.maxCarHealth;
        //        StartCoroutine(ShieldPickup(other));
        //    }
        //}
        if (other.tag == "PlayerCol")
        {
            GenerateWaterSplash();
            carController = other.GetComponentInParent<PlayerCar>();
            carController.currentCarHealth = carController.maxCarHealth;
            StartCoroutine(ShieldPickup(other));
        }
    }

    void GenerateWaterSplash()
    {
        GameObject particle = ParticlePooling.particlePool.GetShieldParticle();
        if (particle == null) return;
        particle.transform.position = transform.position;
        particle.transform.rotation = transform.rotation;
        particle.SetActive(true);
    }

    IEnumerator ShieldPickup(Collider Player)
    {
        // Instantiate some particle effects here...
        //Instantiate(particleShieldEffect, transform.position, transform.rotation);

        //for (int i = 0; i < players.Length; i++)
        //{
        //    carController = players[i].GetComponent<PlayerCar>();
        //}

       // carController.currentCarHealth = carController.changeInDamage = 1; // take no damage
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Invoke("RespawnInvincibility", 5f); // respawn back the powerup after 2 seconds (testing)
        yield return new WaitForSeconds(timer);
        //carController.currentCarHealth = carController.changeInDamage = 20; // reset the damage back to five (testing)
    }

    void RespawnInvincibility()
    {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
