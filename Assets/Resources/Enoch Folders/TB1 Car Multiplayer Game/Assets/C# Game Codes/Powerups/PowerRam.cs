using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRam : MonoBehaviour {

    public GameObject player;
    DamageMeterText playerDamage;
    float timer = 5;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag ("player"))
        {
            StartCoroutine(RamPickup(other));
        }
    }

    IEnumerator RamPickup(Collider Player)
    {
        playerDamage = player.GetComponent<DamageMeterText>();
        playerDamage.damageRate = 10;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(timer);
        playerDamage.damageRate = 1;
        Destroy(gameObject);
    }
}
