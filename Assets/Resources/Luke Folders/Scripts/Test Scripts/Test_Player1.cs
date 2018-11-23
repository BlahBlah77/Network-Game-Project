using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Player1 : Photon.MonoBehaviour {

	public float speed = 10f;
	Rigidbody rb;
	public Camera cam;
	public string ball = "Sphere";

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
		if (!photonView.isMine) 
		{
			cam.gameObject.SetActive (false);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (photonView.isMine) 
		{
			InputMovement ();
			InputColorChange ();
		}
	}

	void InputMovement()
	{
		if (Input.GetKey (KeyCode.W)) 
		{
			rb.MovePosition (rb.position + Vector3.forward * speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.S)) 
		{
			rb.MovePosition (rb.position - Vector3.forward * speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.D)) 
		{
			rb.MovePosition (rb.position + Vector3.right * speed * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.A)) 
		{
			rb.MovePosition (rb.position - Vector3.right * speed * Time.deltaTime);
		}
		if (Input.GetKeyDown (KeyCode.Q)) 
		{
			PhotonNetwork.Instantiate (ball, transform.position, transform.rotation,0);
		}
	}

	void InputColorChange()
	{
		if (Input.GetKeyDown (KeyCode.R)) 
		{
			ChangeColorTo (new Vector3 (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f)));
		}
	}

	[PunRPC]
	void ChangeColorTo(Vector3 color)
	{
		GetComponent<Renderer> ().material.color = new Color (color.x, color.y, color.z, 1f);
		if (photonView.isMine) 
		{
			photonView.RPC ("ChangeColorTo", PhotonTargets.OthersBuffered, color);
		}
	}
}
