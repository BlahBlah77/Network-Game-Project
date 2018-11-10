using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infinitro : MonoBehaviour {

    CarController carController;
    public GameObject player;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            StartCoroutine(InfinitroPickup(other));
        }
    }

    IEnumerator InfinitroPickup(Collider Player)
    {
        carController = player.GetComponent<CarController>();
        carController.changeInNitro = 0;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(5);
        carController.changeInNitro = 5;
        Destroy(gameObject);
    }
}
