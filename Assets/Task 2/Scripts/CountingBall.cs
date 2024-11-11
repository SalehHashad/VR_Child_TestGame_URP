using UnityEngine;
using TMPro;          // Import TextMesh Pro
using UnityEngine.UI;
using System.Collections;

public class CountingBall : MonoBehaviour
{
    public GameObject ballPrefab;
    public TextMeshProUGUI instructionText;
    public TMP_InputField answerInputField;
    public Button submitButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeDuration;

    public TextMeshProUGUI levelText; // New UI element for current level
    public TextMeshProUGUI questionText; // New UI element for current question

    public AudioSource audioSource; // Single AudioSource for playing sounds
    public AudioClip correctSound;  // AudioClip for correct answers
    public AudioClip wrongSound;    // AudioClip for wrong answers

    public float minX = -7f;
    public float maxX = 7f;
    public float minY = -4f;
    public float maxY = 4f;
    public float minZ = 10f;
    public float maxZ = 15f;
    public float forwardOffset = 5f;

    private int score = 0;
    private int currentLevel = 1;
    private int questionsRemaining;
    private int consecutiveCorrect = 0;
    private int correctCount = 0;
    private float displayTime;

    public int questionPerLevel = 20;
    public GameObject keyboard;

    void Start()
    {
        submitButton.onClick.AddListener(OnSubmitAnswer);
        StartCoroutine(StartTask());
    }

    IEnumerator StartTask()
    {
        for (int i = 1; i <= 5; i++)
        {
            StartLevel(i);
            while (questionsRemaining > 0)
            {
                SpawnBalls();
                StartCoroutine(StartCountdown(displayTime)); // Start countdown coroutine
                yield return new WaitForSeconds(displayTime);

                ClearBalls();
                instructionText.text = "How many balls were there?";
                submitButton.interactable = true;

                // Update the question text
                UpdateQuestionText();
                yield return new WaitUntil(() => !submitButton.interactable);
                questionsRemaining--;
            }
            // Congratulate the player after completing all questions in the level
            instructionText.text = "Congratulations! Level " + currentLevel + " completed!";
            yield return new WaitForSeconds(2f); // Wait for 2 seconds before starting the next level
        }
    }
     IEnumerator StartCountdown(float duration)
    {
        float timeRemaining = duration;
        while (timeRemaining > 0)
        {
            timeDuration.text = "Time: " + timeRemaining.ToString("F1") + "s";
            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        timeDuration.text = "Time: 0.0s";
    }

    void StartLevel(int level)
    {
        currentLevel = level;
        switch (level)
        {
            case 1:
                displayTime = 5f;
                break;
            case 2:
                displayTime = 4f;
                break;
            case 3:
                displayTime = 3f;
                break;
            case 4:
                displayTime = 2f;
                break;
            case 5:
                displayTime = 1f;
                break;
        }
        questionsRemaining = questionPerLevel;
        UpdateScoreText();
        levelText.text = "Level: " + currentLevel; // Update the level text
        instructionText.text = "Level " + level + ": Count the balls!";
    }

    void SpawnBalls()
    {
        ShowKeyboard(false);
        switch (currentLevel)
        {
            case 1:
                correctCount = Random.Range(1, 4);
                break;
            case 2:
                correctCount = Random.Range(4, 6);
                break;
            case 3:
                correctCount = Random.Range(6, 8);
                break;
            case 4:
                correctCount = Random.Range(8, 10);
                break;
            case 5:
                correctCount = Random.Range(10, 13);
                break;
        }

        Vector3 forwardPosition = Camera.main.transform.position + Camera.main.transform.forward * forwardOffset;

        for (int i = 0; i < correctCount; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                forwardPosition.z + Random.Range(minZ, maxZ)
            );

            position += new Vector3(forwardPosition.x, forwardPosition.y, 0);
            Instantiate(ballPrefab, position, Quaternion.identity);
        }
    }

    void ClearBalls()
    {
        ShowKeyboard(true);
        foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            Destroy(ball);
        }
    }

    public void OnSubmitAnswer()
    {
        int userAnswer;
        if (int.TryParse(answerInputField.text, out userAnswer))
        {
            CheckAnswer(userAnswer);
        }

        answerInputField.text = "";
        submitButton.interactable = false;
    }

    void CheckAnswer(int userAnswer)
    {
        if (userAnswer == correctCount)
        {
            score += 1;
            consecutiveCorrect++;
            // if (consecutiveCorrect > 1)
            // {
            //     score += 1;
            // }
            instructionText.text = "Correct!";
            audioSource.clip = correctSound;  // Set correct sound clip
        }
        else
        {
            consecutiveCorrect = 0;
            instructionText.text = "Incorrect. Try again!";
            audioSource.clip = wrongSound;    // Set wrong sound clip
        }

        audioSource.Play(); // Play the selected sound
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateQuestionText()
    {
        int currentQuestion = questionPerLevel - questionsRemaining + 1;
        questionText.text = "Question: " + currentQuestion + "/" + questionPerLevel; // Update the question text
    }

    void ShowKeyboard(bool show)
    {
        keyboard.SetActive(show); // Show or hide the keyboard
    }
}
