using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Add the TextMeshPro namespace
using System.Linq;
using System.Collections;
using System.Collections.Generic;  // Add this for List<>

public class MemoryRecallGame : MonoBehaviour
{
    // Game settings
    public int gridSize = 12;
    public int sequenceLength = 2;
    public float sequenceSpeed = 1f;
    public float recallTimeLimit = 10f;  // Default time limit for recall
    private float recallStartTime;

    // UI elements
    public GameObject squarePrefab;
    public Transform gridParent;
    public TextMeshProUGUI scoreText;  // Use TextMeshProUGUI for score
    public TextMeshProUGUI feedbackText;  // Use TextMeshProUGUI for feedback
    public Button recallButton;

    // Variables
    private List<GameObject> squares = new List<GameObject>();
    private List<int> sequence = new List<int>();  // The sequence of colored squares
    private int score = 0;

    private bool isRecalling = false;

    void Start()
    {
        InitializeGrid();
        recallButton.onClick.AddListener(OnRecallButtonClicked);
        StartNextLevel();
    }

    // Initializes the grid of squares
    void InitializeGrid()
    {
        for (int i = 0; i < gridSize; i++)
        {
            GameObject square = Instantiate(squarePrefab, gridParent);
            square.GetComponent<Image>().color = Color.gray;  // Gray squares initially
            squares.Add(square);
        }
    }

    // Starts the next level with updated parameters
    void StartNextLevel()
    {
        // Increase the difficulty (change grid size, sequence length, and speed)
        AdjustLevelSettings();
        sequence.Clear();

        // Generate a new sequence
        GenerateSequence();

        // Start showing the sequence to the player
        StartCoroutine(ShowSequence());
    }

    // Adjust settings for the next level
    void AdjustLevelSettings()
    {
        sequenceLength++;
        sequenceSpeed = Mathf.Max(0.5f, sequenceSpeed - 0.1f); // Make the sequence faster with each level

        // Update grid size and recall time limit based on difficulty
        if (sequenceLength <= 2)
        {
            gridSize = 6;
            recallTimeLimit = 10f;
        }
        else if (sequenceLength <= 4)
        {
            gridSize = 9;
            recallTimeLimit = 8f;
        }
        else if (sequenceLength <= 6)
        {
            gridSize = 12;
            recallTimeLimit = 7f;
        }
        else if (sequenceLength <= 8)
        {
            gridSize = 16;
            recallTimeLimit = 6f;
        }
        else
        {
            gridSize = 20;
            recallTimeLimit = 5f;
        }

        // Clear any previous squares
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        // Reinitialize the grid with updated grid size
        InitializeGrid();
    }

    // Generate a sequence of colored squares
    void GenerateSequence()
    {
        for (int i = 0; i < sequenceLength; i++)
        {
            int randomSquare = Random.Range(0, gridSize);  // Select random square
            sequence.Add(randomSquare);
        }
    }

    // Show the sequence to the player
    IEnumerator ShowSequence()
    {
        feedbackText.text = "Remember the sequence!";
        
        // Set the squares to red according to the sequence
        foreach (int index in sequence)
        {
            squares[index].GetComponent<Image>().color = Color.red;
        }

        // Wait for the sequence to be visible for the specified time
        yield return new WaitForSeconds(sequenceSpeed * sequenceLength);

        // Hide the squares again (back to gray)
        foreach (int index in sequence)
        {
            squares[index].GetComponent<Image>().color = Color.gray;
        }

        // Start the recall phase
        StartRecallPhase();
    }

    // Start the recall phase where the player inputs the sequence
    void StartRecallPhase()
    {
        isRecalling = true;
        recallStartTime = Time.time;
        feedbackText.text = "Recall the sequence!";
    }

    // Handle the recall input
    void OnRecallButtonClicked()
    {
        if (!isRecalling) return;

        float recallDuration = Time.time - recallStartTime;
        int correctCount = 0;

        // Check the player's recall against the sequence
        for (int i = 0; i < sequence.Count; i++)
        {
            // Check if the player selected the correct square for the sequence
            if (squares[sequence[i]].GetComponent<Image>().color == Color.red)
            {
                correctCount++;
            }
        }

        // Calculate score based on correct recall
        score += correctCount;

        // Provide feedback on timing and correct responses
        if (recallDuration < 5f)
        {
            score += 2;  // Bonus points for fast recall
            feedbackText.text = $"Well done! You recalled in {recallDuration:0.0}s!";
        }
        else if (recallDuration < 10f)
        {
            score += 1;  // Bonus points for completing under 10 seconds
            feedbackText.text = $"Nice! You recalled in {recallDuration:0.0}s!";
        }
        else
        {
            feedbackText.text = $"You took too long. Try again!";
        }

        // Update score display
        scoreText.text = "Score: " + score;

        // Move to the next level after a brief pause
        Invoke("StartNextLevel", 2f);
    }
}
