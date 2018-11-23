using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Info : MonoBehaviour {

	public Text roomNametxt;
	public bool isSpawned = false;
	public Network_Lobby nl;

	void Start () 
	{
		nl = GameObject.FindGameObjectWithTag ("Network Lobby").GetComponent<Network_Lobby> ();
	}
	
	// Update is called once per frame
	public void Joining () 
	{
		nl.JoinRoom (roomNametxt.text);
	}
}
