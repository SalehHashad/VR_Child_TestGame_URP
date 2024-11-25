using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoryRecallGame : MonoBehaviour
{
    // Game settings
    private int currentLevel = 1; // Start at Level 1
    public int questionsPerLevel = 50; // Number of sequences per level
    private float questionCounter = 1;

    public GameObject squarePrefab;
    public Transform gridParent;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI recallTimeText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI questionCounterText;

    private List<GameObject> squares = new List<GameObject>();
    private List<int> sequence = new List<int>();
    private List<int> userSequence = new List<int>();
    private int score = 0;
    private bool isRecalling = false;

    private float recallStartTime;
    private float recallTime ;
    private float recallEndTime ;

    // Level configurations
    private class LevelConfig
    {
        public int gridSize;
        public int sequenceStartLength;
        public float sequenceSpeed;

        public LevelConfig(int gridSize, int sequenceStartLength, float sequenceSpeed)
        {
            this.gridSize = gridSize;
            this.sequenceStartLength = sequenceStartLength;
            this.sequenceSpeed = sequenceSpeed;
        }
    }

    private List<LevelConfig> levels = new List<LevelConfig>
    {
        new LevelConfig(6, 2, 5f),   // Level 1
        new LevelConfig(9, 3, 4f),   // Level 2
        new LevelConfig(12, 4, 3f),  // Level 3
        new LevelConfig(16, 4, 2.5f),// Level 4
        new LevelConfig(20, 4, 2f),  // Level 5
    };

    void Start()
    {
        StartNextLevel();
        scoreText.text = $"Score: {score}";
    }
    void Update()
    {
        scoreText.text = $"Score: {score}";
        // recallTimeText.text=$"Time {recallTime}";
        LevelText.text=$"Level {currentLevel}";
        questionCounterText.text=$"Question{questionCounter}/{questionsPerLevel}";
    }

    // Starts the next level or question
    void StartNextLevel()
    {
        if (currentLevel > levels.Count)
        {
            // Infinite scaling after Level 5
            levels.Add(new LevelConfig(20, levels[4].sequenceStartLength + (currentLevel - 5), Mathf.Max(1f, levels[4].sequenceSpeed - 0.1f)));
        }

        LevelConfig config = levels[currentLevel - 1];
        InitializeGrid(config.gridSize);
        sequence.Clear();
        userSequence.Clear();

        GenerateSequence(config.sequenceStartLength);
        StartCoroutine(ShowSequence(config.sequenceSpeed));
    }

    void InitializeGrid(int gridSize)
{
    // Destroy any existing cubes from the previous level
    foreach (Transform child in gridParent)
    {
        Destroy(child.gameObject);
    }

    squares.Clear();

    float gap = 2f;
    int gridColumns = Mathf.CeilToInt(Mathf.Sqrt(gridSize));

    for (int i = 0; i < gridSize; i++)
    {
        // Instantiate the cube from prefab
        GameObject square = Instantiate(squarePrefab, gridParent);
        
        // Set the cube's position in the grid
        float x = (i % gridColumns) * gap;
        float y = (i / gridColumns) * gap;
      
        if(gridSize==12||gridSize==16){
            square.transform.localPosition = new Vector3(x-(gap/2), y, 0);
        }
        else if(gridSize==20){
            square.transform.localPosition = new Vector3(x-(gap), y, 0);
        }
        else{  
            square.transform.localPosition = new Vector3(x, y, 0);
        }

        // Ensure the CubeInteraction script is initialized with the right data
        CubeInteraction cubeInteraction = square.GetComponent<CubeInteraction>();
        if (cubeInteraction != null)
        {
            cubeInteraction.Setup(this, i);  // Pass the current MemoryRecallGame instance and cube index
        }

        // Add the cube to the list
        squares.Add(square);
    }
}


    // Generate a sequence for the current level
    void GenerateSequence(int length)
    {
        for (int i = 0; i < length; i++)
        {
            sequence.Add(Random.Range(0, squares.Count));
        }
    }

    IEnumerator ShowSequence(float baseSpeed)
{
    feedbackText.text = "Memorize the sequence!";
    for (int i = 0; i < sequence.Count; i++)
    {
        int index = sequence[i];
        Renderer renderer = squares[index].GetComponent<Renderer>();
        renderer.material.color = Color.red;

        // Adjust speed dynamically: speed decreases as questionCounter increases
        float adjustedSpeed = baseSpeed / (1 + (questionCounter / 10));
        yield return new WaitForSeconds(adjustedSpeed);

        // Reset previous cube to gray
        renderer.material.color = Color.gray;
    }
    foreach (Transform child in gridParent)
    {
         Renderer renderer = child.GetComponent<Renderer>();
        // Reset previous cube to gray
        renderer.material.color = Color.black;
    }
    // for (int i = 0; i < sequence.Count; i++)
    // {
    //     int index = sequence[i];
       
    //     // GetComponent<Renderer>().material.color = Color.black;
    // }

    feedbackText.text = "Click the cubes to recall the sequence!";
    recallStartTime = Time.time;
    isRecalling = true;
}


    // Handle user clicks
    public void OnCubeClicked(int index)
    {
        if (!isRecalling) return;

        userSequence.Add(index);
        squares[index].GetComponent<Renderer>().material.color = Color.yellow;

        // Check if the sequence is complete
        if (userSequence.Count == sequence.Count)
        {
            EvaluateUserSequence();
        }
    }

    // Evaluate the user's sequence
    void EvaluateUserSequence()
    {
        isRecalling = false;

        recallEndTime = Time.time;
        recallTime = recallEndTime - recallStartTime;

        recallTimeText.text=$"Time {recallTime}";

        int correctCount = 0;

        for (int i = 0; i < sequence.Count; i++)
        {
            if (userSequence[i] == sequence[i])
            {
                correctCount++;
            }
        }

        // Calculate points
        int points = correctCount; // Base points
        if (recallTime <= 5f)
{
    recallTimeText.color = Color.green;
     points += 2; // Bonus for completing in under 5 seconds
}
else if (recallTime <= 10f)
{
      points += 1; // Bonus for completing in under 10 seconds
    recallTimeText.color = Color.yellow;
}
else
{
    recallTimeText.color = Color.red;
}


        score += points;
        questionCounter++;

        scoreText.text = $"Score: {score}";
        feedbackText.text = $"You got {correctCount}/{sequence.Count} correct! (+{points} points)";

        if (questionCounter >= questionsPerLevel)
        {
            currentLevel++;
            questionCounter = 0;
            feedbackText.text = $"Level Up! Welcome to Level {currentLevel}.";
        }

        Invoke(nameof(StartNextLevel), 2f);
    }
}
