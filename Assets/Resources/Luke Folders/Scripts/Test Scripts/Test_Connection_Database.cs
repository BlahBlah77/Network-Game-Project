using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_Connection_Database : MonoBehaviour {

	public string playerName = "Trystan";
	public int score = 12;

	string loadURL = "http://kunet.kingston.ac.uk/k1623759/Network Game/TestSave.php";
	string saveURL = "http://kunet.kingston.ac.uk/k1623759/Network Game/TestSave.php";

	public GameObject txtInfo;
	public string[] items;
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.A)) 
		{
			SaveData ();
		}
		if (Input.GetKeyDown (KeyCode.B)) 
		{
			LoadData ();
		}
	}

	void SaveData()
	{
		StartCoroutine (CSaveData ());
	}

	void LoadData()
	{
		StartCoroutine (CLoadData ());
	}

	IEnumerator CSaveData()
	{
		WWWForm form = new WWWForm ();

		form.AddField ("NovaName", playerName);
		form.AddField ("NovaScore", score);

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
	}

	IEnumerator CLoadData()
	{
		WWW webRequest = new WWW (loadURL);
		yield return webRequest;
		txtInfo.GetComponent<Text> ().text = webRequest.text;
		string itemsDataString = webRequest.text;
		print (itemsDataString);
		items = itemsDataString.Split (';');
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
