using UnityEngine;

public class timerAgian : MonoBehaviour
{
    public float timeRemaining = 5f;
    public bool timerIsRunning = false;
    public booleanValueSwitch scriptRef;

    
    void start()
    {
       //ToggleBool= toggler.GetComponent<Toggle>().isOn;
    }
    private void Update()
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

                // Call the function when the timer ends
                AnotherFunction();
            }
        }
    }

    // Method to start the timer
    public void StartTimer()
    {   if(scriptRef.Switch==false)
        {scriptRef.Switch = true;
        timeRemaining = 5f; // Set the initial time remaining
        timerIsRunning = true;}
    }

    // Method to perform another function when the timer ends
    private void AnotherFunction()
    {
        Debug.Log("Another function executed after the timer ends!");

        // Change the boolean value in AnotherScript to true
        if (scriptRef != null)
        {
            scriptRef.Switch = false;
        }
        else
        {
            Debug.LogError("AnotherScript reference is not set in the Timer script!");
        }
    }
}