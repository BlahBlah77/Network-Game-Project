using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillNitrous : MonoBehaviour {

    public GameObject playerCar;
    CarController carcontroller;
    public GameObject[] nitrousPickups;

	// Use this for initialization
	void Start ()
    { 
        carcontroller = playerCar.GetComponent<CarController>();
        nitrousPickups = GameObject.FindGameObjectsWithTag("Nitrous");
	}

    private void OnTriggerEnter(Collider other)
    {
        foreach (GameObject nitrous in nitrousPickups)
        {
            if (other.gameObject.tag == "Nitrous")
            {
                Destroy(other.gameObject);
                carcontroller.UpdateNitroRate(0, 1);
            }
        }
    }
}
