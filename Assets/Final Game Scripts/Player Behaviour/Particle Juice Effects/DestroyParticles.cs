using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour {

    void OnEnable()
    {
        Invoke("Destruct", 1.5f);
    }

    void Destruct()
    {
        Debug.Log("Destroyed Particles");
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        CancelInvoke();
    }
}
