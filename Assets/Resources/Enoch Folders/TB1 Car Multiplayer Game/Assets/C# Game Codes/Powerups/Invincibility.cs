using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour {

    public GameObject player;
    GameObject particleShieldEffect;
    DamageMeterText playerDamage;
    float timer = 30;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            playerDamage = player.GetComponent<DamageMeterText>();
            playerDamage.currentCarHealth = playerDamage.maxCarHealth;
            StartCoroutine(ShieldPickup(other));
        }
    }

    IEnumerator ShieldPickup(Collider Player)
    {
        // Instantiate some particle effects here...
        //Instantiate(particleShieldEffect, transform.position, transform.rotation);
        playerDamage = player.GetComponent<DamageMeterText>();
        playerDamage.changeInDamage = 0;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(timer);
        playerDamage.changeInDamage = 5;
        Destroy(gameObject);
    }
}
