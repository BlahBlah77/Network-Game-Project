using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Controller : MonoBehaviour {

    Button button;
	// Use this for initialization
	void Start ()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ClickEvent);

    }
	
    void ClickEvent()
    {
        Network_manager.Instnce.EndRoom();
    }
}
