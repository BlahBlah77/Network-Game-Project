using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Rotates mesh 
	void Update () {
		
		transform.Rotate (90 * Time.deltaTime, 0, 0);
	}

	// Destroys Mesh when player collides with it
	private void OnTriggerEnter (Collider other)
	{
		if (other.name == "CarBody") 
		{

			Destroy (gameObject);
		}
	}
}
