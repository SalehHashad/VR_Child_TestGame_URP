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
    public TextMeshProUGUI rightButtonText;
    public TextMeshProUGUI wrongButtonText;
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

public bool isEnglish = true; // Default to English

    public void SetLanguage()
    {
        isEnglish = false;
         Debug.Log("isEnglish>: " + isEnglish); // Debugging purpose
         
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rightButton.onClick.AddListener(OnRightButtonClicked);
        wrongButton.onClick.AddListener(OnWrongButtonClicked);
        AddArabicFixerToAllText();
       
        StartNewSequence();
    }
    void AddArabicFixerToAllText()
{
    TextMeshProUGUI[] allTextElements = {wrongButtonText, rightButtonText, instructionText, scoreText, levelText, 
                                          questionText, responseTimeText, AvergeResponseTimeText, accuracyRateText };

    foreach (TextMeshProUGUI textElement in allTextElements)
    {
        if (textElement != null)
        {
            ArabicFixerTMPRO fixer = textElement.GetComponent<ArabicFixerTMPRO>();
            if (fixer == null)
            {
                textElement.gameObject.AddComponent<ArabicFixerTMPRO>();
            }
        }
    }
}
    private void SetArabicText(TMP_Text textElement, string newText)
    {
        if (textElement != null)
        {
            ArabicFixerTMPRO arabicFixer = textElement.GetComponent<ArabicFixerTMPRO>();
            if (arabicFixer != null)
            {
                arabicFixer.fixedText = newText;
            }
        }
    }
    public void SetLevelAndStartTask(int level)
    {
        currentLevel = level-1; // Set the selected level
        Debug.Log("Current Level: " + currentLevel); // Debugging purpose

        // Call StartClick to begin the task
        StartNewSequence();
    }

    private void StartNewSequence()
    {
         SetArabicText(rightButtonText, 
        !isEnglish 
        ? "نعم" 
        : "YES");
         SetArabicText(wrongButtonText, 
        !isEnglish 
        ? "لا" 
        : "NO");
        GenerateSequence();
        SetArabicText(instructionText, 
    !isEnglish 
    ? "احفظ التسلسل!" 
    : "Memorize the sequence!");

rightButton.gameObject.SetActive(false);
wrongButton.gameObject.SetActive(false);
resultsUI.gameObject.SetActive(false);
TaskUI.gameObject.SetActive(true);

SetArabicText(responseTimeText, 
    !isEnglish 
    ? "" 
    : "");

SetArabicText(questionText, 
    !isEnglish 
    ? $"السؤال: {questionCount + 1} / {maxQuestionsPerLevel}" 
    : $" {questionCount + 1} / {maxQuestionsPerLevel}: Question");

SetArabicText(levelText, 
    !isEnglish 
    ? $"المستوى: {currentLevel + 1}" 
    : $"{currentLevel + 1}:Level");

SetArabicText(scoreText, 
    !isEnglish 
    ? $"الدرجة: {score}" 
    : $"{score}:Score");

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
        SetArabicText(instructionText, 
    !isEnglish 
    ? $"هل كان الرقم {randomPromptNumber} في التسلسل؟" 
    : $"Was the number {randomPromptNumber} in the sequence?");

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
    SetArabicText(responseTimeText, 
    !isEnglish 
    ? $"وقت الاستجابة: {responseTime.ToString("F2")} ثواني" 
    : $"Response Time: {responseTime.ToString("F2")} seconds");

    totalResponseTime += responseTime; // Accumulate total response time
}


    private void CheckAnswer(bool isRightButtonClicked)
{
    bool isCorrectAnswer = digitSequence.Contains(randomPromptNumber);
    
    if ((isRightButtonClicked && isCorrectAnswer) || (!isRightButtonClicked && !isCorrectAnswer))
{
    score++;
    correctResponses++; // Increment correct responses if the answer is correct
    SetArabicText(instructionText, 
        !isEnglish 
        ? "صحيح!" // Arabic for Correct!
        : "Correct!"); // English for Correct!
    PlaySound(correctSound);
    consecutiveWrong = 0;
}
else
{
    consecutiveWrong++;
    SetArabicText(instructionText, 
        !isEnglish 
        ? "غير صحيح!" // Arabic for Incorrect!
        : "Incorrect!"); // English for Incorrect!
    PlaySound(incorrectSound);
}

if(consecutiveWrong==maxconsecutiveWrong)
{
    DisplayResults();
}
    totalQuestions++; // Increment total questions counter
  SetArabicText(scoreText, 
    !isEnglish 
    ? $"الدرجة: {score}" // Arabic for Score
    : $" {score} :Score"); // English for Score


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
        SetArabicText(instructionText, 
    !isEnglish 
    ? $"المستوى {currentLevel + 1}!" // Arabic for Level
    : $"{currentLevel + 1}! Level "); // English for Level

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
  SetArabicText(instructionText, 
    !isEnglish 
    ? "انتهت اللعبة! هنا نتائجك:" // Arabic for "Game Over! Here are your results:"
    : "Game Over! Here are your results:"); // English for "Game Over! Here are your results:"

SetArabicText(AvergeResponseTimeText, 
    !isEnglish 
    ? $"متوسط وقت الاستجابة: {averageResponseTime.ToString("F2")} ثانية" // Arabic for "Average Response Time"
    : $"Average Response Time: {averageResponseTime.ToString("F2")} seconds"); // English for "Average Response Time"

SetArabicText(accuracyRateText, 
    !isEnglish 
    ? $"معدل الدقة: {accuracyRate.ToString("F2")}% " // Arabic for "Accuracy Rate"
    : $"{accuracyRate.ToString("F2")}%: Accuracy Rate"); // English for "Accuracy Rate"

}



}
