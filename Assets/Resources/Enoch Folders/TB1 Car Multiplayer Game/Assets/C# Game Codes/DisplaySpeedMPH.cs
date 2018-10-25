using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySpeedMPH : MonoBehaviour {

    CarController carControl;
    public Text speedText;
    public GameObject playerCar;
    

    // Use this for initialization
    void Start ()
    {
        carControl = playerCar.GetComponent<CarController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        speedText.text = "Speed: " + (int)carControl.actualSpeed + " mph";
	}
}
