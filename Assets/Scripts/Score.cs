using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	// Sets up variables for current score "scoreValue" and MaxScore (Text included)
	public static int scoreValue = 0;
	public static int MaxScore = 70;
	Text score;

	// Set up text to show score value
	void Start()
	{
		score = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update ()
	{

		ScoreValue ();

	}

	// Displays Score text and Updates it
	public void ScoreValue ()
	{
		score.text = "Score: " + scoreValue;
	}
}
