using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Database_Connector : MonoBehaviour {

	private Database_Connector constant = null;
	public Database_Connector Constant{ get{return constant;}}

	public string playerName = "Default Value";
	public string password = "Default Value";

	string loadURL = "http://kunet.kingston.ac.uk/k1623759/NetworkGame/ProfileLoad.php";
	string saveURL = "http://kunet.kingston.ac.uk/k1623759/NetworkGame/ProfileSave.php";

	public GameObject txtInfo;
	public string[] items;

	string newuser;
	string newpass;

	public InputField user;
	public InputField pass;

	public Network_manager nt;

	void Awake()
	{
		constant = this;
	}

	void Start()
	{
		nt = Network_manager.Instnce;
	}

	public void SaveData()
	{
		playerName = user.text;
		password = pass.text;
		StartCoroutine (CSaveData ());
	}

	public void LoadData()
	{
		playerName = user.text;
		password = pass.text;
		StartCoroutine (CLoadData ());
	}

	IEnumerator CSaveData()
	{
		//Makes webrequest for the php code, which loads the database information and sends the user input to the database
		WWWForm form = new WWWForm ();

		form.AddField ("NovaName", playerName);
		form.AddField ("NovaPass", password);

		WWW webRequest = new WWW (saveURL, form);
		Debug.Log (saveURL);
		yield return webRequest;

		if (!string.IsNullOrEmpty (webRequest.error)) 
		{
			print ("This isn't working: " + webRequest.error);
		}
		else 
		{
			Debug.Log (webRequest.text.ToString ());
		}
		//Sets the room name to the username and sets the scene to the lobby
		nt.RoomName = playerName;
		SceneManager.LoadScene ("lvl_Test_Lobby");
	}

	IEnumerator CLoadData()
	{

		//Makes webrequest for the php code, which loads the database information and sets the return to a variable
		WWW webRequest = new WWW (loadURL);
		yield return webRequest;
		string itemsDataString = webRequest.text;
		items = itemsDataString.Split (';');

		Debug.Log (items.Length);

		//For each of the items of the array, the username and password are assigned to a variable
		for (int i = 0; i < items.Length; i++) 
		{
			Debug.Log (items[i]);
			newuser = GetDataValue (items [i], "Profile Name: ");
			newpass = GetDataValue (items [i], "Profile Password: ");

			if (newuser == playerName) 
			{
				if (newpass == password) 
				{
					//If the username and password match the user input, set the room name to the username and set the scene to the lobby
					nt.RoomName = newuser;
					SceneManager.LoadScene ("lvl_Test_Lobby");
				} 
				else 
				{
					txtInfo.GetComponent<Text> ().text = "Incorrect Username or Password";
				}
			}
		}
		txtInfo.GetComponent<Text> ().text = "Incorrect Username";

	}

	string GetDataValue(string data, string index)
	{
		string value = data.Substring (data.IndexOf (index) + index.Length);
		if (value.Contains ("|")) 
		{
			value = value.Remove (value.IndexOf ("|"));
		}
		return value;
	}
}
