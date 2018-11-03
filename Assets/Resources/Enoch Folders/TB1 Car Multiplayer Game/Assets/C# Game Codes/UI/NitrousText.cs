using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NitrousText : MonoBehaviour {

    CarController carController;
    public GameObject playerCar;
    public Text nitroText;

    // Use this for initialization
    void Start()
    {
        carController = playerCar.GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        nitroText.text = "Nitrous: " + (int)carController.currentNitro + " %";
    }
}
