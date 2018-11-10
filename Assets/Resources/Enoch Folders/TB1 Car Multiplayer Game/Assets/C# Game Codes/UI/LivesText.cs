using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesText : MonoBehaviour {

    DamageMeterText playerDamage;
    public GameObject player;
    public Text carLivesText;
    public float currentLives;
    public float maxLives = 3;

    // Use this for initialization
    void Start ()
    {
        currentLives = maxLives;
	}
	
	// Update is called once per frame
	void Update ()
    {
        ShowPlayerLives();
        RemoveLives();
	}

    void ShowPlayerLives()
    {
        carLivesText.text = "Lives: "  + (int)currentLives;
    }

    void RemoveLives()
    {
        playerDamage = player.GetComponent<DamageMeterText>();

        if (playerDamage.currentCarHealth <= 0 && currentLives <= 3)
        {
            currentLives--;
            playerDamage.currentCarHealth = 100;

            //SPACE TO WRITE NETWORK CODE HERE...
            //Respawning back into game lobby and getting car back...
        }

        if (currentLives <= 0)
        {
            Debug.Log("You are wrecked!");
            currentLives = 0;
            playerDamage.currentCarHealth = 0;
            
            //SPACE TO WRITE NETWORK CODE HERE...
            //Out of the game can no longer play
        }
    }
}
