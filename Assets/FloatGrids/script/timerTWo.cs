using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class timerTWo : MonoBehaviour
{
    public float timeRemaining = 5;
    public bool timerIsRunning = false;

    public booleanValueSwitch scriptRef;
    public void timer()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }
    private void AnotherFunction()
    {
        Debug.Log("Another function executed after the timer ends!");

        // Change the boolean value in AnotherScript to true
        if (scriptRef != null)
        {
            scriptRef.Switch = true;
        }
        else
        {
            Debug.LogError("AnotherScript reference is not set in the Timer script!");
        }
    }
    
}