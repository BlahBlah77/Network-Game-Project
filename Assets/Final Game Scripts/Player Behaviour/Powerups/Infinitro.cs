using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infinitro : Photon.MonoBehaviour
{
    PlayerCar carController;
    float timer = 20;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerCol")
        {
            GenerateEnergyExplosion();
            carController = other.GetComponentInParent<PlayerCar>();
            carController.currentNitro = carController.maxNitro;
            StartCoroutine(InfinitroPickup(other));
        }

    }

    void GenerateEnergyExplosion()
    {
        GameObject particle = ParticlePooling.particlePool.GetNitroParticle();
        if (particle == null) return;
        particle.transform.position = transform.position;
        particle.transform.rotation = transform.rotation;
        particle.SetActive(true);
    }

    IEnumerator InfinitroPickup(Collider Player)
    {
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