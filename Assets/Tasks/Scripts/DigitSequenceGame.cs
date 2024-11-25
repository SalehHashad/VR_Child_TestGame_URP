using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DigitSequenceGame : MonoBehaviour
{
    public TextMeshProUGUI digitText;
    public TextMeshProUGUI instructionText;
    public Button rightButton;
    public Button wrongButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;  // Display current level
    public TextMeshProUGUI questionText; // Display current question number
    public TextMeshProUGUI responseTimeText;
    public TextMeshProUGUI AvergeResponseTimeText;
    public TextMeshProUGUI accuracyRateText;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    private AudioSource audioSource;

public GameObject TaskUI;
public GameObject OnboardingUI;
public GameObject resultsUI;
    [Header("Level Configurations")]
    public float[] displayTimePerLevel = { 5.0f, 4.5f, 4.0f, 3.5f, 3.0f };
    public int[] minDigitsPerLevel = { 3, 4, 5, 6, 7 };
    public int[] maxDigitsPerLevel = { 10, 11, 12, 13, 14 };
    public Color[] digitColorsPerLevel = { Color.blue, Color.green, Color.yellow, Color.magenta, Color.red };

    private List<int> digitSequence = new List<int>();
    private int sequenceLength = 3;
    private int randomPromptNumber;
    private int score = 0;
    private float promptDisplayTime;
    
    private int currentLevel = 0; // Start at level 0, which is Level 1 for user
    private int questionCount = 0;
    public int maxQuestionsPerLevel = 50;
    private const int maxLevels = 5;
private float totalResponseTime = 0f;
private int correctResponses = 0;
private int totalQuestions = 0;
private int consecutiveWrong = 0;
public int maxconsecutiveWrong = 4;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rightButton.onClick.AddListener(OnRightButtonClicked);
        wrongButton.onClick.AddListener(OnWrongButtonClicked);
        StartNewSequence();
    }

    private void StartNewSequence()
    {
        GenerateSequence();
        instructionText.text = "Memorize the sequence!";
        rightButton.gameObject.SetActive(false);
        wrongButton.gameObject.SetActive(false);
        responseTimeText.text = "";
        questionText.text = "Question: " + (questionCount + 1) + " / " + maxQuestionsPerLevel;
        levelText.text = "Level: " + (currentLevel + 1);
        StartCoroutine(DisplaySequence());
    }
    

    private void GenerateSequence()
{
    digitSequence.Clear();

    // Increment sequence length with each question until it reaches maxDigitsPerLevel
    sequenceLength = Mathf.Clamp(minDigitsPerLevel[currentLevel] + questionCount, minDigitsPerLevel[currentLevel], maxDigitsPerLevel[currentLevel]);

    // Generate the digit sequence based on the calculated sequence length
    for (int i = 0; i < sequenceLength; i++)
    {
        digitSequence.Add(Random.Range(0, 10));
    }

    // Set a random number for the prompt, ensuring it's a single digit (0-9)
    randomPromptNumber = Random.Range(0, 10);
}


    private IEnumerator DisplaySequence()
    {
        for (int i = 0; i < digitSequence.Count; i++)
        {
            digitText.color = Color.black;
            digitText.text = digitSequence[i].ToString();
            yield return new WaitForSeconds(displayTimePerLevel[currentLevel]);
        }

        digitText.text = "";
        yield return new WaitForSeconds(2.0f);  // Pause after showing the sequence
        PromptForAnswer();
    }

    private void PromptForAnswer()
    {
        instructionText.text = $"Was the number {randomPromptNumber} in the sequence?";
        digitText.color = digitColorsPerLevel[currentLevel];  // Use level color
        digitText.text = randomPromptNumber.ToString();
        promptDisplayTime = Time.time;
        rightButton.gameObject.SetActive(true);
        wrongButton.gameObject.SetActive(true);
    }

    private void OnRightButtonClicked()
    {
        RecordResponseTime();
        CheckAnswer(true);
    }

    private void OnWrongButtonClicked()
    {
        RecordResponseTime();
        CheckAnswer(false);
    }

    
    private void RecordResponseTime()
{
    float responseTime = Time.time - promptDisplayTime;
    responseTimeText.text = "Response Time: " + responseTime.ToString("F2") + " seconds";
    totalResponseTime += responseTime; // Accumulate total response time
}


    private void CheckAnswer(bool isRightButtonClicked)
{
    bool isCorrectAnswer = digitSequence.Contains(randomPromptNumber);
    
    if ((isRightButtonClicked && isCorrectAnswer) || (!isRightButtonClicked && !isCorrectAnswer))
    {
        score++;
        correctResponses++; // Increment correct responses if the answer is correct
        instructionText.text = "Correct!";
        PlaySound(correctSound);
        consecutiveWrong=0;
    }
    else
    {
        consecutiveWrong++;
        instructionText.text = "Incorrect!";
        PlaySound(incorrectSound);
    }
if(consecutiveWrong==maxconsecutiveWrong)
{
    DisplayResults();
}
    totalQuestions++; // Increment total questions counter
    scoreText.text = "Score: " + score;

    rightButton.gameObject.SetActive(false);
    wrongButton.gameObject.SetActive(false);

    questionCount++;

    if (questionCount >= maxQuestionsPerLevel)
    {
        LevelUp();
    }
    else
    {
        Invoke("StartNewSequence", 2.0f);
    }
}


    
    private void LevelUp()
{
    if (currentLevel < maxLevels - 1)
    {
        currentLevel++;
        questionCount = 0;
        instructionText.text = $"Level {currentLevel + 1}!";
        Invoke("StartNewSequence", 2.0f);
    }
    else
    {
        DisplayResults(); // Call DisplayResults when all levels are completed
    }
}

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    private void DisplayResults()
{
    TaskUI.SetActive(false);
    resultsUI.SetActive(true);
    float averageResponseTime = totalResponseTime / totalQuestions;
    float accuracyRate = (float)correctResponses / totalQuestions * 100f;

    // Display results in the UI
    instructionText.text = "Game Over! Here are your results:";
    AvergeResponseTimeText.text = "Average Response Time: " + averageResponseTime.ToString("F2") + " seconds";
    accuracyRateText.text = "Accuracy Rate: " + accuracyRate.ToString("F2") + "%";
}



}
