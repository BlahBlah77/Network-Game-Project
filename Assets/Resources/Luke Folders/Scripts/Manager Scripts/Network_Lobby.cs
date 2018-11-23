using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Network_Lobby : MonoBehaviour {

	Network_manager nt;
	public GameObject pi;
	public RectTransform view;
	List<GameObject> infoList;

	// Use this for initialization
	void Start () 
	{
		//PhotonNetwork.ConnectUsingSettings ("v4.2");
		nt = Network_manager.Instnce;
		infoList = new List<GameObject>();
	}

	public void FindRoom()
	{
		if (PhotonNetwork.room == null) 
		{
			if (nt.RoomList != null) 
			{
				if (infoList.Count != 0) 
				{
					foreach (GameObject info in infoList) 
					{
						Destroy (info);
					}
				}

				for (int i = 0; i < nt.RoomList.Length; i++) 
				{
					var obj = Instantiate(pi, transform.position, transform.rotation);
					obj.transform.SetParent (view.transform, false);
					obj.transform.localPosition = new Vector3 (view.transform.localPosition.x, view.transform.localPosition.y - (50 * i), view.transform.localPosition.z);
					obj.GetComponent<Player_Info> ().roomNametxt.text = nt.RoomList [i].Name;
					infoList.Add (obj);
				}
			}
		}
	}

	public void StartRoom()
	{
		nt.StartRoom ();
	}

	public void JoinRoom(string room)
	{
		PhotonNetwork.JoinRoom (room);
		SceneManager.LoadScene ("lvl_Test1");
	}
}
