using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Damage_Points : MonoBehaviour {

	// Text and Maxscore Condition Variables (Maxscore being set to false)
	public Text PositionText;
	public bool MaxScoreReached = false;


	
	// Update is called once per frame
	void Update () 
	{
		PointsRanking ();
	}

	// Point Ranking system, change if necessary 
	public void PointsRanking()
	{
		
		if (Score.scoreValue >= 10 && Score.scoreValue <= 19) 
			{
				PositionText.text = "Bronze";
			}

		if (Score.scoreValue >= 20 && Score.scoreValue <= 29)
			{
				PositionText.text = "Silver";
			}

		if (Score.scoreValue >= 30 && Score.scoreValue <= 39) 
			{
				PositionText.text = "Gold";
			}

		if (Score.scoreValue >= 40 && Score.scoreValue <= 49) 
			{
				PositionText.text = "Platinum";
			}

		// Sets MaxScoreReached as true once the scoreValue and MaxScore values in the Score Script are the same
		if (Score.scoreValue == Score.MaxScore)
			{
				MaxScoreReached = true;
			}
		
	}

	// Checks Collision with players and will increase score only when Maxscore is false, change values and name if necessary
	void OnCollisionEnter(Collision col)
	{
		
		if (col.gameObject.name == "Car8" && MaxScoreReached == false) 
		{

			Score.scoreValue += 2;
		
		}

	}
}
