using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePooling : MonoBehaviour {

    public static ParticlePooling particlePool; // create an instance of this and no need to reference
    public int pooledAmount = 1;
    public bool expandPool = true;

    [Header ("Car Effects Pooling")]
    public GameObject sparkParticle;
    private List<GameObject> sparkParticles;

    [Header ("Power Up Pooling")]
    // NITRO UP PARTICLE EFFECTS
    public GameObject nitroPowerupParticle;
    private List<GameObject> nitroPowerupParticles;

    // POWER RAM PARTICLE EFFECTS
    public GameObject powerRamPowerupParticle;
    private List<GameObject> powerRamPowerupParticles;

    //// SHIELD PARTICLE EFFECTS
    public GameObject shieldPowerupParticle;
    private List<GameObject> shieldPowerupParticles;

    //// WHEN PLAYER DIES PARTICLE EFFECT  (networking code addition)
    //public GameObject explosionParticle;
    //private List<GameObject> explosionParticles;


    // Destroy unloads object from the memory and set reference to null so in order to use it again you need to recreate it, via let's say instantiate. 
    //Meanwhile SetActive just hides the object and disables all components on it so if you need you can use it again.

    void Awake()
    {
        // the current script is equals to everything in this script
        particlePool = this;
    }

    // Use this for initialization
    void Start()
    {
        SetupSparkParticle();
        //SetupExplosionParticle();
        SetupNitroParticle();
        SetupPowerRamParticle();
        SetupShieldParticle();
    }

    void SetupSparkParticle()
    {
        sparkParticles = new List<GameObject>(); // create new list that will store particles as 'Gameobjects'

        // loop through this condition
        // then instantiate the game object but set the particles at the start to false..
        // then add the particle gameobject to our list of pooledParticles
        for (int x = 0; x < pooledAmount; x++)
        {
            GameObject particle = Instantiate(sparkParticle, transform.position, transform.rotation);
            particle.SetActive(false);
            sparkParticles.Add(particle);
        }
    }

    //void SetupExplosionParticle()
    //{
    //    explosionParticles = new List<GameObject>(); // create new list that will store particles as 'Gameobjects'

    //    // loop through this condition
    //    // then instantiate the game object but set the particles at the start to false..
    //    // then add the particle gameobject to our list of pooledParticles
    //    for (int x = 0; x < pooledAmount; x++)
    //    {
    //        GameObject particle = Instantiate(explosionParticle, transform.position, transform.rotation);
    //        particle.SetActive(false);
    //        explosionParticles.Add(particle);
    //    }
    //}

    void SetupNitroParticle()
    {
        nitroPowerupParticles = new List<GameObject>();

        for (int x = 0; x < pooledAmount; x++)
        {
            GameObject particle = Instantiate(nitroPowerupParticle, transform.position, transform.rotation);
            particle.SetActive(false);
            nitroPowerupParticles.Add(particle);
        }
    }

    void SetupPowerRamParticle()
    {
        powerRamPowerupParticles = new List<GameObject>();

        for (int x = 0; x < pooledAmount; x++)
        {
            GameObject particle = Instantiate(powerRamPowerupParticle, transform.position, transform.rotation);
            particle.SetActive(false);
            powerRamPowerupParticles.Add(particle);
        }
    }

    void SetupShieldParticle()
    {
        shieldPowerupParticles = new List<GameObject>();

        for (int x = 0; x < pooledAmount; x++)
        {
            GameObject particle = Instantiate(shieldPowerupParticle, transform.position, transform.rotation);
            particle.SetActive(false);
            shieldPowerupParticles.Add(particle);
        }
    }

    //----------------------- CALLABLE METHODS FOR INSTANTIATING PARTICLES ----------------------- //
    //Can call any of these methods in our collectables script or other scripts to instantiate the particles...
    public GameObject GetSparkParticle()
    {
        // loop through the number of particle game objects in the list
        for (int x = 0; x < sparkParticles.Count; x++)
        {
            // if the particle game object is not active in the game
            // then return the particles in the game object list.
            if (!sparkParticles[x].activeInHierarchy)
            {
                return sparkParticles[x];
            }
        }

        // if we are growing the pool of particles.
        // then instantiate the particle game object
        // and add it to the list
        // return the particle afterwards
        // This allows us to expand how many particles we need if instantiated many times
        if (expandPool)
        {
            //GameObject particle = Instantiate(pooledParticle);
            GameObject particle = Instantiate(sparkParticle, transform.position, transform.rotation);
            particle.SetActive(false);
            sparkParticles.Add(particle);
            return particle;
        }

        return null;
    }

    //public GameObject GetExplosionParticle()
    //{
    //    // loop through the number of particle game objects in the list
    //    for (int x = 0; x < explosionParticles.Count; x++)
    //    {
    //        // if the particle game object is not active in the game
    //        // then return the particles in the game object list.
    //        if (!explosionParticles[x].activeInHierarchy)
    //        {
    //            return sparkParticles[x];
    //        }
    //    }

    //    // if we are growing the pool of particles.
    //    // then instantiate the particle game object
    //    // and add it to the list
    //    // return the particle afterwards
    //    // This allows us to expand how many particles we need if instantiated many times
    //    if (expandPool)
    //    {
    //        //GameObject particle = Instantiate(pooledParticle);
    //        GameObject particle = Instantiate(sparkParticle, transform.position, transform.rotation);
    //        particle.SetActive(false);
    //        explosionParticles.Add(particle);
    //        return particle;
    //    }

    //    return null;
    //}

    public GameObject GetNitroParticle()
    {
        // loop through the number of particle game objects in the list
        for (int x = 0; x < nitroPowerupParticles.Count; x++)
        {
            // if the particle game object is not active in the game
            // then return the particles in the game object list.
            if (!nitroPowerupParticles[x].activeInHierarchy)
            {
                return nitroPowerupParticles[x];
            }
        }

        // if we are growing the pool of particles.
        // then instantiate the particle game object
        // and add it to the list
        // return the particle afterwards
        // This allows us to expand how many particles we need if instantiated many times
        if (expandPool)
        {
            //GameObject particle = Instantiate(pooledParticle);
            GameObject particle = Instantiate(nitroPowerupParticle, transform.position, transform.rotation);
            particle.SetActive(false);
            nitroPowerupParticles.Add(particle);
            return particle;
        }

        return null;
    }

    public GameObject GetPowerRamParticle()
    {
        // loop through the number of particle game objects in the list
        for (int x = 0; x < powerRamPowerupParticles.Count; x++)
        {
            // if the particle game object is not active in the game
            // then return the particles in the game object list.
            if (!powerRamPowerupParticles[x].activeInHierarchy)
            {
                return powerRamPowerupParticles[x];
            }
        }

        // if we are growing the pool of particles.
        // then instantiate the particle game object
        // and add it to the list
        // return the particle afterwards
        // This allows us to expand how many particles we need if instantiated many times
        if (expandPool)
        {
            //GameObject particle = Instantiate(pooledParticle);
            GameObject particle = Instantiate(powerRamPowerupParticle, transform.position, transform.rotation);
            particle.SetActive(false);
            powerRamPowerupParticles.Add(particle);
            return particle;
        }

        return null;
    }

    public GameObject GetShieldParticle()
    {
        // loop through the number of particle game objects in the list
        for (int x = 0; x < shieldPowerupParticles.Count; x++)
        {
            // if the particle game object is not active in the game
            // then return the particles in the game object list.
            if (!shieldPowerupParticles[x].activeInHierarchy)
            {
                return shieldPowerupParticles[x];
            }
        }

        // if we are growing the pool of particles.
        // then instantiate the particle game object
        // and add it to the list
        // return the particle afterwards
        // This allows us to expand how many particles we need if instantiated many times
        if (expandPool)
        {
            //GameObject particle = Instantiate(pooledParticle);
            GameObject particle = Instantiate(shieldPowerupParticle, transform.position, transform.rotation);
            particle.SetActive(false);
            shieldPowerupParticles.Add(particle);
            return particle;
        }

        return null;
    }
}
