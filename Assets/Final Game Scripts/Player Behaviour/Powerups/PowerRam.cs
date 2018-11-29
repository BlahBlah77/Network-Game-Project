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
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        foreach (GameObject player in players)
        {
            if (other.gameObject == player)
            {
                StartCoroutine(RamPickup(other));
            }
        }
    }

    IEnumerator RamPickup(Collider Player)
    {
        // Instantiate some particle effects here...
        //Instantiate(particleRamEffect, transform.position, transform.rotation);

        for (int i = 0; i < players.Length; i++)
        {
            carController = players[i].GetComponent<PlayerCar>();
        }

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
