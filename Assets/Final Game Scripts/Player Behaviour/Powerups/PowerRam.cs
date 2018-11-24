using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRam : MonoBehaviour {

    public GameObject player;
    GameObject particleRamEffect;
    PlayerCar carController;
    float timer = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            StartCoroutine(RamPickup(other));
        }
    }

    IEnumerator RamPickup(Collider Player)
    {
        // Instantiate some particle effects here...
        //Instantiate(particleRamEffect, transform.position, transform.rotation);
        carController = player.GetComponent<PlayerCar>();
        carController.damageRate = 1.2f; // modify damage to 1.2 times more
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Invoke("RespawnPowerRam", 5f);
        yield return new WaitForSeconds(timer);
        carController.damageRate = 0.2f; // bring it back down to the original damage hit rate
    }

    void RespawnPowerRam()
    {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
