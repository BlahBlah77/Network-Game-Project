using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRam : MonoBehaviour {

    public GameObject[] players;
    GameObject particleRamEffect;
    PlayerCar carController;
    float timer = 10;

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
        //        GenerateBigExplosion();
        //        StartCoroutine(RamPickup(other));
        //    }
        //}
        if (other.tag == "PlayerCol")
        {
            GenerateBigExplosion();
            carController = other.GetComponentInParent<PlayerCar>();
            StartCoroutine(RamPickup(other));
        }
    }

    void GenerateBigExplosion()
    {
        GameObject particle = ParticlePooling.particlePool.GetPowerRamParticle();
        if (particle == null) return;
        particle.transform.position = transform.position;
        particle.transform.rotation = transform.rotation;
        particle.SetActive(true);
    }

    IEnumerator RamPickup(Collider Player)
    {
        // Instantiate some particle effects here...
        //Instantiate(particleRamEffect, transform.position, transform.rotation);

        //for (int i = 0; i < players.Length; i++)
        //{
        //    carController = players[i].GetComponent<PlayerCar>();
        //}

        carController.damageRate = 1.5f; // modify damage to 1.2 times more
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Invoke("RespawnPowerRam", 5f);
        yield return new WaitForSeconds(timer);
        carController.damageRate = 0.8f; // bring it back down to the original damage hit rate
    }

    void RespawnPowerRam()
    {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
