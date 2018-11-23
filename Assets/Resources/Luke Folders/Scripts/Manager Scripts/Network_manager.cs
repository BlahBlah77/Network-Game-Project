using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Network_manager : MonoBehaviour {

	private static Network_manager instnce = null;

	public string RoomName = "RoomName";
	private TypedLobby LobbyName = new TypedLobby("New Lobby", LobbyType.Default);
	public RoomInfo[] RoomList;
	public GameObject Player;

	public static Network_manager Instnce
	{
		get 
		{
			return instnce;
		}
	}

	void Awake()
	{
		if (instnce) 
		{
			DestroyImmediate (gameObject);
			return;
		}
		instnce = this;
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () 
	{
		Debug.Log ("HAHA");
		PhotonNetwork.ConnectUsingSettings ("v4.2");
	}

//	void OnGUI()
//	{
//		if (!PhotonNetwork.connected) 
//		{
//			GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
//		} 
//		else if (PhotonNetwork.room == null) 
//		{
//			if (GUI.Button (new Rect (100, 100, 250, 100), "Start Server")) 
//			{
//				PhotonNetwork.CreateRoom (RoomName, new RoomOptions ()
//					{ MaxPlayers = 4, IsOpen = true, IsVisible = true }, LobbyName);
//			}
//			if (RoomList != null) 
//			{
//				for (int i = 0; i < RoomList.Length; i++) 
//				{
//					if (GUI.Button (new Rect (100, 250 + (110 * i), 250, 100), "Join " + RoomList [i].Name)) 
//					{
//						PhotonNetwork.JoinRoom (RoomList [i].Name);
//					}
//				}
//			}
//		}
//	}

	void Update()
	{
//		if (PhotonNetwork.room == null) 
//		{
//			if (RoomList != null) 
//			{
//				for (int i = 0; i < RoomList.Length; i++) 
//				{
//					var obj = Instantiate(pi, transform.position, transform.rotation);
//					obj.transform.SetParent (GameObject.Find("Viewport").GetComponent<Transform>(), false);
//					obj.transform.localPosition = new Vector3 (obj.transform.localPosition.x, obj.transform.localPosition.y - (50 * i), obj.transform.localPosition.z);
//				}
//			}
//		}
	}

	public void StartRoom()
	{
		SceneManager.LoadScene ("lvl_Test1");
		PhotonNetwork.CreateRoom (RoomName, new RoomOptions ()
		{ 
			MaxPlayers = 4, 
			IsOpen = true, 
			IsVisible = true 
		}, 
		LobbyName);
	}

	public void JoinRoom()
	{
		if (RoomList != null)
		{
			for (int i = 0; i < RoomList.Length; i++) 
			{
				PhotonNetwork.JoinRoom (RoomList [i].Name);
			}
		}
	}

	void OnConnectedToMaster()
	{
		Debug.Log ("OnMaster");
		PhotonNetwork.JoinLobby (LobbyName);
	}

	void OnReceivedRoomListUpdate()
	{
		Debug.Log ("Room was Created");
		RoomList = PhotonNetwork.GetRoomList ();
	}

	void OnJoinedLobby()
	{
		Debug.Log ("Joined Lobby");
	}

	void OnJoinedRoom()
	{
		Debug.Log ("Connected to Room");
		PhotonNetwork.Instantiate (Player.name, Vector3.up * 5, Quaternion.identity, 0);
	}
}