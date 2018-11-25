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
    int playerNo;

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
        //SceneManager.LoadScene ("Arena");
        PhotonNetwork.LoadLevel("Arena");

        PhotonNetwork.CreateRoom (RoomName, new RoomOptions ()
		{ 
			MaxPlayers = 4, 
			IsOpen = true, 
			IsVisible = true 
		}, 
		LobbyName);
    }

	public void JoinRoom(string room)
	{
        PhotonNetwork.LoadLevel("Arena");
        //if (RoomList != null)
        //{
        //	for (int i = 0; i < RoomList.Length; i++) 
        //	{
        //		PhotonNetwork.JoinRoom (RoomList [i].Name);
        //	}
        //}
        PhotonNetwork.JoinRoom (room);
        //PhotonNetwork.LoadLevel ("lvl_Test1");
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
        Spawn_Manager sp = GameObject.Find("Spawn Manager").GetComponent<Spawn_Manager>();
        CheckNumber();

        Debug.Log ("Connected to Room");
        if (playerNo == 1)
        {
            PhotonNetwork.Instantiate(Player.name, sp.spawnList[0].position, sp.spawnList[0].rotation, 0);
            playerNo = 2;
        }
        else if (playerNo == 2)
        {
            PhotonNetwork.Instantiate(Player.name, sp.spawnList[1].position, sp.spawnList[1].rotation, 0);
            playerNo = 3;
        }
        else if (playerNo == 3)
        {
            PhotonNetwork.Instantiate(Player.name, sp.spawnList[2].position, sp.spawnList[2].rotation, 0);
            playerNo = 4;
        }
        else if (playerNo == 4)
        {
            PhotonNetwork.Instantiate(Player.name, sp.spawnList[3].position, sp.spawnList[3].rotation, 0);
            playerNo = 1;
        }
    }

    public void EndRoom()
    {
        //Returns to the lobby screen on level end
        PhotonNetwork.LeaveRoom();

    }

    void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel("lvl_Test_Lobby");
    }

    void CheckNumber()
    {
        //Sets the player number to the number of players in the room
        //In event of more players than the game allows
        playerNo = PhotonNetwork.room.PlayerCount;
        for (int i = 0; i <= playerNo; i++)
        {
            if (playerNo > 4)
            {
                playerNo -= 4;
            }
        }
    }
}