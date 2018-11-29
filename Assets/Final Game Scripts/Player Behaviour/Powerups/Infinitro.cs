using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infinitro : MonoBehaviour {

    public GameObject[] players;
    GameObject particleInfinitroEffect;
    PlayerCar carController;
    float timer = 20;

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
                carController = player.GetComponent<PlayerCar>();
                carController.currentNitro = carController.maxNitro;
                StartCoroutine(InfinitroPickup(other));
            }
        }

    }

    IEnumerator InfinitroPickup(Collider Player)
    {
        // Instantiate some particle effects here...
        //Instantiate(particleInfinitroEffect, transform.position, transform.rotation);

        for (int i = 0; i < players.Length; i++)
        {
            carController = players[i].GetComponent<PlayerCar>();
        }

        carController.changeInNitro = 0;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Invoke("RespawnInfinitro", 5f);
        yield return new WaitForSeconds(timer);
        carController.changeInNitro = 5;
    }

    void RespawnInfinitro()
    {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
