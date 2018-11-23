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
        if (other.CompareTag ("player"))
        {
            StartCoroutine(RamPickup(other));
        }
    }

    IEnumerator RamPickup(Collider Player)
    {
        // Instantiate some particle effects here...
        //Instantiate(particleRamEffect, transform.position, transform.rotation);
        carController = player.GetComponent<PlayerCar>();
        carController.damageRate = 2;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Invoke("RespawnPowerRam", 5);
        yield return new WaitForSeconds(timer);
        carController.damageRate = 1;
    }

    void RespawnPowerRam()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
