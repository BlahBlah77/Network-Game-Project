using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	// Sets up all varibles to show Timer on text, create the start time and two booleans to freeze timer at the beggining and set the timer finished to false.
	public Text timerText;
	private float startTime;
    private bool finished = false;
    private bool timerpause = true;

    // Starts Countdown before timer starts, Change if necessary
    void Start ()
    {
        StartCoroutine(HoldTimer(5));
    }

    IEnumerator HoldTimer(float time)
    {
        yield return new WaitForSeconds(5);
        startTime = Time.time;
        timerpause = false;
    }

	// Updates and starts timer
	void Update ()
    {
        if (!timerpause)
        {
            TimerStart();
        }
    }

    // Stops Timer
    void Finished ()
    {
        finished = true;
    }

    // Timer Function
    void TimerStart()
    {
        // Stops Timer when score is equal to the MaxScore, Change if necessary 
        if (finished)
            return;

		if (Score.scoreValue == Score.MaxScore)
        {
            Finished();
        }

        // Timer Code
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");
        timerText.text = minutes + ":" + seconds;
    }
}
