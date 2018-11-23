using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class dbConnectiontest : MonoBehaviour {

	private dbConnectiontest constant = null;
	public dbConnectiontest Constant{ get{return constant;}}

	public string playerName = "Default Value";
	public string password = "Default Value";

	string loadURL = "http://kunet.kingston.ac.uk/k1623759/NetworkGame/ProfileSave.php";
	string saveURL = "http://kunet.kingston.ac.uk/k1623759/NetworkGame/ProfileSave.php";

	public GameObject txtInfo;
	public string[] items;

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

	// Update is called once per frame
	void Update () 
	{
//		if (Input.GetKeyDown (KeyCode.A)) 
//		{
//			SaveData ();
//		}
//		if (Input.GetKeyDown (KeyCode.B)) 
//		{
//			LoadData ();
//		}
	}

	public void SaveData()
	{
		playerName = user.text;
		password = pass.text;
		StartCoroutine (CSaveData ());
	}

	public void LoadData()
	{
		StartCoroutine (CLoadData ());
	}

	IEnumerator CSaveData()
	{
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
		nt.RoomName = playerName;
		SceneManager.LoadScene ("lvl_Test_Lobby");
	}

	IEnumerator CLoadData()
	{
		WWW webRequest = new WWW (loadURL);
		yield return webRequest;
		//txtInfo.GetComponent<Text> ().text = webRequest.text;
		string itemsDataString = webRequest.text;
		//print (itemsDataString);
		items = itemsDataString.Split (';');
//		if ((GetDataValue (items [0], 0)) && (GetDataValue (items [1], 0))) 
//		{
//
//		}
		print (GetDataValue (items [0], "Score:"));
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
