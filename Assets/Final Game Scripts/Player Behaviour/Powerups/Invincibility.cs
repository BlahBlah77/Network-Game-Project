using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour {

    PlayerCar carController;
    float timer = 20;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerCol")
        {
            GenerateDustExplosion();
            carController = other.GetComponentInParent<PlayerCar>();
            carController.currentCarHealth = carController.maxCarHealth;
            StartCoroutine(ShieldPickup(other));
        }
    }

    void GenerateDustExplosion()
    {
        GameObject particle = ParticlePooling.particlePool.GetShieldParticle();
        if (particle == null) return;
        particle.transform.position = transform.position;
        particle.transform.rotation = transform.rotation;
        particle.SetActive(true);
    }

    IEnumerator ShieldPickup(Collider Player)
    {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Invoke("RespawnInvincibility", 5f); // respawn back the powerup after 2 seconds (testing)
        yield return new WaitForSeconds(timer);
    }

    void RespawnInvincibility()
    {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}
