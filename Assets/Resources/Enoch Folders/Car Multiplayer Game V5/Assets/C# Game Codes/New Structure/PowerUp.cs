using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public enum PowerUpType
    {
        Speed,
        Damage
    }

    [SerializeField]
    private PowerUpType powerUp;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //Check if collided object uses IPowerUp
        //Then call powerup function - passing name of the type of powerup
        //Then pool away
    }
}
