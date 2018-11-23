using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour 
{
	// Sets up varibles
	Image Healthbar;
	float maxHealth = 100f;
	public static float health;

	// Sets up the healthbar and healthbar amount on startup
	void Start () 
	{
		Healthbar = GetComponent<Image> ();
		health = maxHealth;
		
	}
		
	// Update is called once per frame
	void Update () 
	{
		FillHealthBar ();
	}

	// Updates healthbar to match the current health amount
	public void FillHealthBar ()
	{
		Healthbar.fillAmount = health / maxHealth;
	}

}
