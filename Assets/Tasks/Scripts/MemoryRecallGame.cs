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

    private int totalErrors = 0; // Track the total errors for the level
public  int maxErrors = 4; // The maximum allowed errors per level


    [Header("Audio Feedback")]
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip incorrectSound;
    private AudioSource audioSource;

    public GameObject taskUI;
public GameObject onboardingUI;

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
void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void StartTask()
{
    // Activate the task UI and deactivate the onboarding UI
    taskUI.SetActive(true);
    onboardingUI.SetActive(false);

    // Start the first level after task starts
    StartNextLevel();
}
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        // StartNextLevel();
        taskUI.SetActive(false);
        onboardingUI.SetActive(true);
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
        int previousNumber = -1; // Initialize to a value that cannot occur (digits are 0-9)

        for (int i = 0; i < length; i++)
        {
        int newNumber;
        do
        {
            newNumber = Random.Range(0, squares.Count); // Generate a number between 0 and 9
        } 
        while (newNumber == previousNumber); // Ensure it's not the same as the last number
        sequence.Add(newNumber);
        previousNumber = newNumber; // Update the last number
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


   
    public void OnCubeClicked(int index)
{
    if (!isRecalling) return;

    userSequence.Add(index);

    // Start the color change coroutine for the clicked cube
    StartCoroutine(FadeToBlack(squares[index].GetComponent<Renderer>()));

    // Check if the sequence is complete
    if (userSequence.Count == sequence.Count)
    {
        EvaluateUserSequence();
    }
}

    IEnumerator FadeToBlack(Renderer renderer)
{
    // Check if the renderer or its GameObject is still valid
    if (renderer == null || renderer.gameObject == null)
    {
        yield break; // Exit the coroutine if the renderer is null
    }

    Color startColor = Color.yellow;
    Color endColor = Color.black;
    float duration = 1f; // Duration of the fade effect
    float elapsed = 0f;

    while (elapsed < duration)
    {
        // Check again if the object is still valid
        if (renderer == null || renderer.gameObject == null)
        {
            yield break; // Exit the coroutine if the renderer is destroyed
        }

        elapsed += Time.deltaTime;
        renderer.material.color = Color.Lerp(startColor, endColor, elapsed / duration);
        yield return null;
    }

    // Final color set
    if (renderer != null && renderer.gameObject != null)
    {
        renderer.material.color = endColor; // Ensure the color is fully black at the end
    }
}



    // Evaluate the user's sequence
    void EvaluateUserSequence()
{
    isRecalling = false;

    recallEndTime = Time.time;
    recallTime = recallEndTime - recallStartTime;

    recallTimeText.text = $"Time {recallTime}";

    int correctCount = 0;
    

    // Loop through the user sequence and compare with the correct sequence
    for (int i = 0; i < sequence.Count; i++)
    {
        if (userSequence[i] == sequence[i])
        {
            correctCount++;
            PlaySound(correctSound);
        }
        else
        {
            totalErrors++;  // Increment error count if the user clicked wrong
            PlaySound(incorrectSound);
        }
    }

    // Calculate points
    int points = correctCount; // Base points for correct sequence matches
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

    score += points; // Add points to the score
    questionCounter++;

    scoreText.text = $"Score: {score}";
    feedbackText.text = $"You got {correctCount}/{sequence.Count} correct! (+{points} points)\nTotal errors: {totalErrors}/{maxErrors}";

    // Check if errors exceed the limit
    if (totalErrors >= maxErrors)
    {
        feedbackText.text = $"Maximum errors reached! Level failed.\nTotal errors: {totalErrors}/{maxErrors}";
        EndTask(); // End the level if max errors reached
        return;
    }

    // Check if the level is complete (all questions are done)
    if (questionCounter >= questionsPerLevel)
    {
         totalErrors = 0; // Reset errors for this evaluation
        currentLevel++;
        questionCounter = 0;
        feedbackText.text = $"Level Up! Welcome to Level {currentLevel}.";
    }

    // Delay before starting the next level
    Invoke(nameof(StartNextLevel), 2f);
}
void EndTask()
{
    // Display final feedback message
    if (totalErrors >= maxErrors)
    {
        feedbackText.text = $"Task Over! You reached the maximum errors ({totalErrors}/{maxErrors}).\nBetter luck next time!";
    }
    else
    {
        feedbackText.text = $"Task Completed! \nTotal score: {score}";
    }

    // Disable further inputs or actions, effectively "ending" the task
    isRecalling = false;  // Stop recall process
    userSequence.Clear();  // Clear the user's sequence for the next task

    
}

}
