using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerRam : MonoBehaviour {

    PlayerCar carController;
    float timer = 10;

    void OnTriggerEnter(Collider other)
    {
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
