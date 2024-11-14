using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectButton : MonoBehaviour
{
    public ObjectController objectController;
    public GameObject objectToCheck;
    public Button verButton;  // Button for vertical check
    public Button horButton;  // Button for horizontal check

    // Flag to prevent multiple calls
    private bool isButtonPressed = false;

    void Start()
    {
        // Adding listeners for both buttons
        verButton.onClick.AddListener(OnVerButtonClick);
        horButton.onClick.AddListener(OnHorButtonClick);
    }

    // Function to handle transparency check
    public void OnTransparencyButtonClick()
    {
        if (isButtonPressed) return;  // Check if button was already pressed
        isButtonPressed = true;       // Set flag to true to prevent multiple calls

        if (objectController.CheckTransparency(objectToCheck))
        {
            objectController.IncreaseScore();
            Debug.Log("Correct Transparency!");
        }
        else
        {
            Debug.Log("Incorrect Transparency.");
        }
        objectController.FlipPhase();

        // Reset flag after action is complete (if needed, based on phase transition)
        isButtonPressed = false;
    }

    // Function to handle the "Ver" button click (vertical rotation check)
    void OnVerButtonClick()
    {
        if (isButtonPressed) return;  // Prevent multiple calls
        isButtonPressed = true;       // Set flag

        Debug.Log("Checking rotation for object: " + objectToCheck.name);
        if (!objectController.CheckRotation(objectToCheck)) // If rotation is false
        {
            objectController.IncreaseScore(); // Increase score
            Debug.Log("Rotation is incorrect. Increased score.");
        }
        else
        {
            Debug.Log("Rotation is correct. No score change.");
        }
        objectController.FlipPhaseBack();

        // Reset flag after the operation completes
        isButtonPressed = false;
    }

    // Function to handle the "Hor" button click (horizontal rotation check)
    void OnHorButtonClick()
    {
        if (isButtonPressed) return;  // Prevent multiple calls
        isButtonPressed = true;       // Set flag

        Debug.Log("Checking rotation for object: " + objectToCheck.name);
        if (objectController.CheckRotation(objectToCheck)) // If rotation is true
        {
            Debug.Log("Rotation is correct. No score change.");
        }
        else
        {
            Debug.Log("Rotation is incorrect. No score change.");
        }
        objectController.FlipPhaseBack();

        // Reset flag after the operation completes
        isButtonPressed = false;
    }
}
