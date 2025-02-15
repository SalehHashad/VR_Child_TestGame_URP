using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Task8 : MonoBehaviour
{
    public TextMeshProUGUI questionText;           // Text for displaying the question
    public TextMeshProUGUI scoreText;              // Text for displaying the score
    public TextMeshProUGUI levelText;              // Text for displaying the current level
    public TextMeshProUGUI currentQuestionText;    // Text for displaying the current question number
    public TextMeshProUGUI currentNumberText;      // Text for displaying the current number
    public Button leftButton;                      // Left button
    public Button rightButton;                     // Right button
    public TextMeshProUGUI leftButtonText;         // Text for the left button label
    public TextMeshProUGUI rightButtonText;        // Text for the right button label
    public Transform spawnPoint;                   // Spawn point for the shape prefab
    public GameObject diamondPrefab;               // Diamond shape prefab for odd/even
    public GameObject squarePrefab;                // Square shape prefab for greater/less
    public int questionPerLevel = 50;              // Number of questions before level up

    private GameObject currentShapeInstance;       // Holds the current shape instance
    private int currentLevel = 1;
    private int score = 0;
    private int questionCount = 0;
    private int number;
    private Color blueColor = Color.blue;          // Blue color for odd/even task
    private Color redColor = Color.red;            // Red color for greater/less task

    private enum TaskType { OddEven, GreaterLess }
    private TaskType currentTaskType;
    private int maxRange = 0;

    // Sound clips
    public AudioClip correctSound;   // Sound for correct answer
    public AudioClip wrongSound;     // Sound for wrong answer
    private AudioSource audioSource; // AudioSource to play sounds

    // Task End Flag
    private bool isTaskEnd = false;
    private int consecutiveWrongAnswers = 0;
    public int maxConsecutiveWrong = 4;

    void Start()
    {
        // Get the AudioSource component from the same GameObject
        audioSource = GetComponent<AudioSource>();

        SetupLevel(currentLevel);
        leftButton.onClick.AddListener(() => HandleButtonClick(true));
        rightButton.onClick.AddListener(() => HandleButtonClick(false));
        UpdateUI();
    }

    void SetupLevel(int level)
    {
        if (isTaskEnd) return; // Don't set up a new task if the task is already ended

        maxRange = GetMaxRangeForLevel(level);
        // number = Random.Range(0, maxRange + 1);
        int restrictedValue = Mathf.CeilToInt(maxRange / 2f);
        do
        {
            number = Random.Range(0, maxRange + 1);
        } 
        while (number == restrictedValue);
        questionCount++;

        // Randomly choose between the odd/even or greater/less task
        if (Random.value > 0.5f)
        {
            currentTaskType = TaskType.OddEven;
            questionText.text = $"Is number odd or even?";
            currentNumberText.text = $"{number}";
            DisplayShape(diamondPrefab);

            // Set button texts for odd/even check
            leftButtonText.text = "Odd";
            rightButtonText.text = "Even";
        }
        else
        {
            currentTaskType = TaskType.GreaterLess;
            questionText.text = $"Is number greater or less than {Mathf.CeilToInt(maxRange / 2f)}?";
            currentNumberText.text = $"{number}";

            DisplayShape(squarePrefab);

            // Set button texts for greater/less check
            leftButtonText.text = "HIGH";
            rightButtonText.text = "LOW";
        }

        // Update UI elements for score, level, and question count
        UpdateUI();
    }

    int GetMaxRangeForLevel(int level)
    {
        switch (level)
        {
            case 1: return 9;
            case 2: return 20;
            case 3: return 40;
            case 4: return 70;
            case 5: return 100;
            default: return 9;
        }
    }

    void DisplayShape(GameObject shapePrefab)
    {
        // Destroy previous shape instance if it exists
        if (currentShapeInstance != null)
        {
            Destroy(currentShapeInstance);
        }

        // Check if the shape is diamond and needs to be rotated
        Quaternion rotation = spawnPoint.rotation;
        if (shapePrefab == diamondPrefab)
        {
            // Rotate 45 degrees in Z-axis for the diamond shape
            rotation = Quaternion.Euler(spawnPoint.rotation.eulerAngles.x, spawnPoint.rotation.eulerAngles.y, 45);
        }

        // Instantiate the selected shape prefab at the spawn point with the adjusted rotation
        currentShapeInstance = Instantiate(shapePrefab, spawnPoint.position, rotation);
    }

    void HandleButtonClick(bool isLeftButton)
    {
        if (isTaskEnd) return; // Don't handle input if task is already ended

        bool isCorrect = false;

        if (currentTaskType == TaskType.OddEven)
        {
            // Odd/Even task
            if (isLeftButton && number % 2 != 0) // Left button is "Odd"
            {
                isCorrect = true;
            }
            else if (!isLeftButton && number % 2 == 0) // Right button is "Even"
            {
                isCorrect = true;
            }
        }
        else if (currentTaskType == TaskType.GreaterLess)
        {
            // Greater/Less task
            if (isLeftButton && number > (Mathf.CeilToInt(maxRange / 2f))) // Left button is "Greater"
            {
                isCorrect = true;
            }
            else if (!isLeftButton && number < (Mathf.CeilToInt(maxRange / 2f))) // Right button is "Less"
            {
                isCorrect = true;
            }
        }

        // Handle wrong answers count
        if (!isCorrect)
        {
            consecutiveWrongAnswers++;
            if (consecutiveWrongAnswers >= maxConsecutiveWrong)
            {
                EndTask();
                return;
            }
        }
        else
        {
            consecutiveWrongAnswers = 0; // Reset on correct answer
        }

        // Play sound based on correctness
        if (isCorrect)
        {
            score++;
            Debug.Log("Correct! Current Score: " + score);
            audioSource.PlayOneShot(correctSound); // Play correct sound
        }
        else
        {
            Debug.Log("Incorrect.");
            audioSource.PlayOneShot(wrongSound); // Play wrong sound
        }

        // Load the next question and check for level up
        LoadNextQuestion();
    }

    void LoadNextQuestion()
    {
        // Check if the current level's question limit has been reached
        if (questionCount >= questionPerLevel)
        {
            // Congratulate the user and move to the next level
            Debug.Log($"Congratulations! You reached level {currentLevel + 1}!");
            currentLevel++;
            questionCount = 0; // Reset question count for new level
        }

        SetupLevel(currentLevel);
    }

    void EndTask()
    {
        // End the task and clear any remaining spawned objects
        isTaskEnd = true;

        // Clear the spawned shape and disable the buttons
        if (currentShapeInstance != null)
        {
            Destroy(currentShapeInstance);
        }

        // Disable buttons
        leftButton.interactable = false;
        rightButton.interactable = false;

        Debug.Log("Task ended due to 4 consecutive wrong answers.");
        // Display task end message if desired (optional)
        questionText.text = "Task Ended!";
    }

    void UpdateUI()
    {
        // Update the displayed score, level, and question count
        scoreText.text = $"Score: {score}";
        levelText.text = $"Level: {currentLevel}";
        currentQuestionText.text = $"Question: {questionCount}/{questionPerLevel}";
    }

    public void SetDifficultyLevel(int level)
    {
        currentLevel = level;
        questionCount = 0;  // Reset question count for new level
        SetupLevel(currentLevel);
    }
}
