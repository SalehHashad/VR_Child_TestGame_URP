using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTimer : MonoBehaviour
{

    public float countdownDuration = 60f; // Set the duration of the countdown timer in seconds

    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }
   

    public void Timer()
    {
        // Start the timer immediately
        StartTimer();
    }

    private void Update()
    {
        // Check if the timer has reached zero
        if (countdownDuration > 0f)
        {
            countdownDuration -= Time.deltaTime;
        }
        else
        {
            // Timer has finished, perform your action here
            Debug.Log("Timer has finished!");
            // Optionally, stop the timer here
            countdownDuration = 0f;
        }
    }

    // Call this method to start or restart the timer
    public void StartTimer()
    {
        countdownDuration = 60f; // Set the timer back to the original duration
    }
    
}