using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pong_Ball : Photon.MonoBehaviour {

	public float startSpeed = 5f;
	public float maxSpeed = 20f;
	public float speedIncrease = 0.25f;
	private float currentSpeed;
	private Vector2 currentDir;

	// Use this for initialization
	void Start () 
	{
		currentSpeed = startSpeed;
		currentDir = Random.insideUnitCircle.normalized;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (PhotonNetwork.playerList.Length == 0) 
		{
			return;
		}
		Vector2 moveDir = currentDir * currentSpeed * Time.deltaTime;
		transform.Translate (new Vector3 (moveDir.x, moveDir.y, 0f));
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Boundary") 
		{
			currentDir.y *= -1;
		} 
		else if (other.tag == "Player") 
		{
			currentDir.x *= -1;
		} 
		else if (other.tag == "Goal") 
		{
			ChangeColourTo (new Vector3 (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f)));
			ChangePositionTo (new Vector3 (0f, 1.5f, -2f));
			ChangeDirTo (Random.insideUnitCircle.normalized);
		}
	}

	[PunRPC]
	void ChangePositionTo(Vector3 myPosition)
	{
		GetComponent<Transform> ().position = myPosition;
		if (photonView.isMine)
			photonView.RPC ("ChangePositionTo", PhotonTargets.OthersBuffered, myPosition);
	}

	[PunRPC]
	void ChangeDirTo(Vector3 myDirection)
	{
		currentDir = myDirection;
		if (photonView.isMine)
			photonView.RPC ("ChangeDirTo", PhotonTargets.OthersBuffered, myDirection);
	}

	[PunRPC]
	void ChangeColourTo(Vector3 color)
	{
		GetComponent<Renderer> ().material.color = new Color (color.x, color.y, color.z, 1f);
		if (photonView.isMine)
			photonView.RPC ("ChangeColourTo", PhotonTargets.OthersBuffered, color);
	}
}
