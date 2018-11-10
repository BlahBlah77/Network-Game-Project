using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infinitro : MonoBehaviour {

    public GameObject player;
    GameObject particleInfinitroEffect;
    CarController carController;
    float timer = 30;
   
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            carController = player.GetComponent<CarController>();
            carController.currentNitro = carController.maxNitro;
            StartCoroutine(InfinitroPickup(other));
        }
    }

    IEnumerator InfinitroPickup(Collider Player)
    {
        // Instantiate some particle effects here...
        //Instantiate(particleInfinitroEffect, transform.position, transform.rotation);
        carController = player.GetComponent<CarController>();
        carController.changeInNitro = 0;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(timer);
        carController.changeInNitro = 5;
        Destroy(gameObject);
    }
}
