using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DigitMemoryGame : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private TMP_InputField answerInput;
    [SerializeField] private Button submitButton;
    [SerializeField] private TMP_Text firstAttemptScoreText;
    [SerializeField] private TMP_Text secondAttemptScoreText;
    [SerializeField] private TMP_Text thirdAttemptScoreText;
    [SerializeField] private TMP_Text fourthAttemptScoreText;
    [SerializeField] private TMP_Text totalCorrectResponsesText;
    [SerializeField] private TMP_Text longestSequenceText;
    [SerializeField] private TMP_Text currentLevelText; // New UI for current level display
    [SerializeField] private TMP_Text currentSequenceText; // New UI for current sequence display

    [Header("UI Tab")]
    [SerializeField] private GameObject resultsPanel; // Panel UI
    [SerializeField] private GameObject TaskUI; // Panel UI
    [SerializeField] private GameObject OnboardingUI; // Panel UI
    [SerializeField] private GameObject Keyboard; // Panel UI

    [Header("Game Settings")]
    [SerializeField] private int SequencePerLevel=20; // Panel UI
    [SerializeField] private float timeBetwenLvl=3; // Panel UI
    [SerializeField] private float timeBetwenQ=1; // Panel UI
    [SerializeField] private float timeBetwenDigit=2; // Panel UI

    [Header("Audio Feedback")]
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip incorrectSound;
    private AudioSource audioSource;

    private List<int> currentSequence = new List<int>();
    private int sequenceLength = 2;
    private int currentLevel = 1; // Current game level
    private int sequenceInLevel = 0; // Tracks sequences completed in the current level
    private int currentAttempt = 0;
    private int maxAttempts = 4;

    private int firstAttemptScore = 0;
    private int secondAttemptScore = 0;
    private int thirdAttemptScore = 0;
    private int fourthAttemptScore = 0;

    private int failedSequences = 0;  // Counter to track consecutive failed sequences
    private int totalCorrectResponses = 0;
    private int longestSequence = 0;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        submitButton.onClick.AddListener(CheckUserResponse);
        
        resultsPanel.SetActive(false); // Hide results panel at the start
        TaskUI.SetActive(true);

        ///====code modified here ===..//
        // OnboardingUI.SetActive(true);
    // StartClick();
    }
    public void SetLevelAndStartTask(int level)
    {
        currentLevel = level; // Set the selected level
        Debug.Log("Current Level: " + currentLevel); // Debugging purpose

        // Call StartClick to begin the task
        StartClick();
    }

   // This function is called to start the task with a countdown
    public void StartClick()
    {
        // Activate the TaskUI and start a countdown before setting isTaskOn to true
        OnboardingUI.SetActive(false);  // Deactivate Onboarding UI
        resultsPanel.SetActive(false);  // Deactivate Onboarding UI
        TaskUI.SetActive(true);  // Activate Task UI
        Keyboard.SetActive(true);  // Activate Task UI
        StartCoroutine(CountdownBeforeStart());
    }

    // Countdown timer before starting the task
    IEnumerator CountdownBeforeStart()
    {
        instructionText.text = "Task starting in 5 seconds...";
        
        for (int i = 0; i < 5; i++)
        {
            instructionText.text = $"Task starting in {5-i} seconds...";
            yield return new WaitForSeconds(1);  // Wait for 1 seconds
        }
        
        
        instructionText.text = "Task Started!";

        // Start the game after countdown
        StartNewLevel();
    }

    void StartNewLevel()
    {
        sequenceInLevel = 0;
        currentAttempt = 0;
        totalCorrectResponses = 0;

        switch (currentLevel)
        {
            case 1:
                sequenceLength = Mathf.Clamp(2 + sequenceInLevel, 2, 10);
                instructionText.text = "Level 1: Enter the sequence in the same order.";
                break;
            case 2:
                sequenceLength = Mathf.Clamp(2 + sequenceInLevel, 2, 10);
                instructionText.text = "Level 2: Enter the sequence in reverse order.";
                break;
            case 3:
                sequenceLength = Mathf.Clamp(3 + sequenceInLevel, 3, 11);
                instructionText.text = "Level 3: Enter the sequence in reverse order.";
                break;
            case 4:
                sequenceLength = Mathf.Clamp(4 + sequenceInLevel, 4, 12);
                instructionText.text = "Level 4: Enter the sequence in reverse order.";
                break;
            case 5:
                sequenceLength = Mathf.Clamp(5 + sequenceInLevel, 5, 13);
                instructionText.text = "Level 5: Enter the sequence in reverse order.";
                break;
            default:
                sequenceLength = Mathf.Clamp(5 + (currentLevel - 1), 5, 13);
                instructionText.text = $"Level {currentLevel}: Enter the sequence in reverse order.";
                break;
        }

        UpdateLevelUI();
        GenerateNewSequence();
    }
    

    void UpdateLevelUI()
    {
        currentLevelText.text = "Current Level: " + currentLevel;
        currentSequenceText.text = "Current Sequence: " + sequenceInLevel+"/"+SequencePerLevel;
    }

    void GenerateNewSequence()
{
    currentSequence.Clear();
    answerInput.text = "";

    // Adjust sequence length based on current level and sequence in level
    if (currentLevel == 1)
    {
        sequenceLength = Mathf.Clamp(2 + sequenceInLevel, 2, 10);
    }
    else if (currentLevel == 2)
    {
        sequenceLength = Mathf.Clamp(2 + sequenceInLevel, 2, 10);
    }
    else if (currentLevel == 3)
    {
        sequenceLength = Mathf.Clamp(3 + sequenceInLevel, 3, 11);
    }
    else if (currentLevel == 4)
    {
        sequenceLength = Mathf.Clamp(4 + sequenceInLevel, 4, 12);
    }
    else if (currentLevel == 5)
    {
        sequenceLength = Mathf.Clamp(5 + sequenceInLevel, 5, 13);
    }

    instructionText.text = (currentLevel == 1) ? "Memorize the sequence." : "Enter the sequence in reverse order.";

    int previousNumber = -1; // Initialize to a value that cannot occur (digits are 0-9)

    for (int i = 0; i < sequenceLength; i++)
    {
        int newNumber;
        do
        {
            newNumber = Random.Range(0, 10); // Generate a number between 0 and 9
        } 
        while (newNumber == previousNumber); // Ensure it's not the same as the last number

        currentSequence.Add(newNumber);
        previousNumber = newNumber; // Update the last number
    }

    UpdateLevelUI();

    StartCoroutine(DisplaySequence());
}


    IEnumerator DisplaySequence()
    {
        answerInput.interactable = false; // disEnable the input field
        // Create a copy of the current sequence to avoid modifying it while enumerating
        List<int> sequenceToDisplay = new List<int>(currentSequence);

        foreach (int digit in sequenceToDisplay)
        {
            
            displayText.text = digit.ToString();
            answerInput.text = "";
            yield return new WaitForSeconds(timeBetwenDigit);
        }

        displayText.text = "";
        instructionText.text = (currentLevel == 1) ? "Enter the sequence." : "Enter the sequence in reverse order.";
        answerInput.interactable = true; // Enable the input field
    }

    public void CheckUserResponse()
    {
        string userAnswer = answerInput.text.Trim();
        string correctAnswer = "";

        // Generate the correct answer based on current level
        if (currentLevel == 1)
        {
            for (int i = 0; i < currentSequence.Count; i++)
            {
                
                correctAnswer += currentSequence[i].ToString();
            }
        }
        else
        {
            for (int i = currentSequence.Count - 1; i >= 0; i--)
            {
                correctAnswer += currentSequence[i].ToString();
            }
        }

        if (userAnswer == correctAnswer)
        {
            PlaySound(correctSound);
            HandleCorrectResponse();
        }
        else
        {
            answerInput.text = "";
            PlaySound(incorrectSound);
            HandleIncorrectResponse();
        }
    }

    void HandleCorrectResponse()
    {
        switch (currentAttempt)
        {
            case 0:
                firstAttemptScore++;
                firstAttemptScoreText.text = "First Attempt Score: " + firstAttemptScore;
                break;
            case 1:
                secondAttemptScore++;
                secondAttemptScoreText.text = "Second Attempt Score: " + secondAttemptScore;
                break;
            case 2:
                thirdAttemptScore++;
                thirdAttemptScoreText.text = "Third Attempt Score: " + thirdAttemptScore;
                break;
            case 3:
                fourthAttemptScore++;
                fourthAttemptScoreText.text = "Fourth Attempt Score: " + fourthAttemptScore;
                break;
        }

        // Increment the total correct responses
        totalCorrectResponses++;

        // Update longest sequence if the current sequence length is the longest
        if (sequenceLength > longestSequence)
        {
            longestSequence = sequenceLength;
        }

        sequenceInLevel++;
        
        if (sequenceInLevel >= SequencePerLevel) // Assuming 10 sequences per level
        {
            currentLevel++;
            sequenceInLevel = 0; // Reset for the next level
            if (currentLevel > 5) // End game after Level 5
            {
                EndGame();
                return;
            }
            // StartNewLevel();
             StartCoroutine(WaitAndStartNewLevel());
        }
        else
        {
            currentAttempt = 0;
            // GenerateNewSequence();
            StartCoroutine(WaitAndGenerateNewSequence());
        }
    }

    void HandleIncorrectResponse()
{
    currentAttempt++;

    if (currentAttempt < maxAttempts)
    {
        instructionText.text = $"Attempt {currentAttempt + 1} failed. Try again.";
        StartCoroutine(DisplaySequence());
    }
    else
    {
        // Increment the consecutive failed sequences counter
        failedSequences++;

        if (failedSequences >= 2)
        {
            // End the game if the user fails four attempts twice in a row
            instructionText.text = "Game Over: Too many failed attempts.";
            EndGame();
            return;
        }
        else
        {
            instructionText.text = "Maximum attempts reached. Moving to next sequence.";
            sequenceInLevel++;
            currentAttempt = 0;

            if (sequenceInLevel >= SequencePerLevel)
            {
                currentLevel++;
                sequenceInLevel = 0;
                if (currentLevel > 5)
                {
                    EndGame();
                    return;
                }
                // StartNewLevel();
                 StartCoroutine(WaitAndStartNewLevel());
            }
            else
            {
                // GenerateNewSequence();
                StartCoroutine(WaitAndGenerateNewSequence());
            }
        }
    }
}

IEnumerator WaitAndGenerateNewSequence()
{
    instructionText.text = $"Next sequence in {timeBetwenQ} seconds...";
    yield return new WaitForSeconds(timeBetwenQ);  // Wait for 3 seconds
    GenerateNewSequence();
}

IEnumerator WaitAndStartNewLevel()
{
    instructionText.text = $"Level " + currentLevel + " starting in {timeBetwenLvl} seconds...";
    yield return new WaitForSeconds(timeBetwenLvl);  // Wait for 3 seconds
    StartNewLevel();
}

// IEnumerator WaitAndEnableInput()
// {
//     yield return new WaitForSeconds(2); // Wait for 2 seconds before retrying
//     instructionText.text = (currentLevel == 1) ? "Enter the sequence." : "Enter the sequence in reverse order.";
//     answerInput.interactable = true; // Enable input field again
// }

    void EndGame()
    {
        resultsPanel.SetActive(true);
        TaskUI.SetActive(false);
        longestSequenceText.text = "Longest Sequence: " + longestSequence;
        totalCorrectResponsesText.text = "Total Correct Responses: " + totalCorrectResponses;
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void RestartGame()
    {
        // Reset variables and UI for a new game
        currentLevel = 1;
        firstAttemptScore = 0;
        secondAttemptScore = 0;
        thirdAttemptScore = 0;
        fourthAttemptScore = 0;
        totalCorrectResponses = 0;
        longestSequence = 0;

        resultsPanel.SetActive(false);
        TaskUI.SetActive(true);
        StartNewLevel();
    }
}
