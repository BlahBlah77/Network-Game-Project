using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongNetworkManager : MonoBehaviour {

	private const string roomName = "RoomNamePongThing";
	private TypedLobby lobbyName = new TypedLobby ("New_Pong_Lobby", LobbyType.Default);
	private RoomInfo[] roomsList;
	public GameObject player;
	public GameObject ball;

	// Use this for initialization
	void Start () 
	{
		PhotonNetwork.ConnectUsingSettings ("v4.2");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI()
	{
		if (!PhotonNetwork.connected) 
		{
			GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
		} 
		else if (PhotonNetwork.room == null) 
		{
			if (GUI.Button (new Rect (100, 100, 250, 250), "Start Server")) 
			{
				PhotonNetwork.CreateRoom (roomName, new RoomOptions () 
					{ MaxPlayers = 4, IsOpen = true, IsVisible = true }, lobbyName);
			}

			if (roomsList != null) 
			{
				for (int i = 0; i < roomsList.Length; i++) 
				{
					if (GUI.Button (new Rect (100, 250 + (110 * i), 250, 250), "Join " + roomsList [i].Name)) 
					{
						PhotonNetwork.JoinRoom (roomsList [i].Name);
					}
				}
			}
		}
	}

	void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby (lobbyName);
	}

	void OnReceivedRoomListUpdate()
	{
		Debug.Log ("Room Made");
		roomsList = PhotonNetwork.GetRoomList ();
	}

	void OnJoinedLobby()
	{
		Debug.Log ("Joined to the lobby");
	}

	void OnJoinedRoom()
	{
		Debug.Log ("Connected to a room");
		print (PhotonNetwork.playerList.Length);
		if (PhotonNetwork.playerList.Length > 1) 
		{
			PhotonNetwork.Instantiate (player.name, new Vector3 (-4f, 1.5f, -2f), Quaternion.identity, 0);
			PhotonNetwork.Instantiate (ball.name, new Vector3 (0f, 1.5f, -2f), Quaternion.identity, 0);
		} 
		else 
		{
			PhotonNetwork.Instantiate (player.name, new Vector3 (4f, 1.5f, -2f), Quaternion.identity, 0);
		}
	}
}
