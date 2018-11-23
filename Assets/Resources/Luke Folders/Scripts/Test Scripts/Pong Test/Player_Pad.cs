using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Pad : Photon.MonoBehaviour {

	public float speed = 5f;
	Rigidbody rb;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (photonView.isMine) 
		{
			InputMovement ();
		}
	}

	void InputMovement ()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			rb.MovePosition (rb.position + Vector3.up * speed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			rb.MovePosition (rb.position - Vector3.up * speed * Time.deltaTime);
		}
	}
}
