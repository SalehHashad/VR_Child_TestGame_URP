using UnityEngine;
using TMPro;          // Import TextMesh Pro
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

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
    public AudioClip trialEndClip;

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
    private int errorCount = 0; 


    public int maxLevel = 1;
    public int maxTotalErrors = 4;
    public int questionPerLevel = 20;
    public GameObject keyboard;
    public Transform spawnPoint;
public bool isEnglish = true; // Default to English
public UnityEvent trialEnd;

public void mainTaskStart()
{
    if (audioSource == null)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    if (trialEndClip == null)
    {
        Debug.LogError("trialEndClip is null! Assign it in the inspector.");
        StartTheTask();
        return;
    }
    audioSource.clip = trialEndClip;
    audioSource.Play();
    // Play the audio
    // PlaySound(trialEndClip);

    // Start a coroutine to wait until the audio finishes
    StartCoroutine(WaitForAudioToFinish());
}


    private IEnumerator WaitForAudioToFinish()
{
    if (audioSource == null || audioSource.clip == null)
    {
        Debug.LogError("AudioSource or AudioClip is null. Skipping wait.");
        StartTheTask(); // Ensure the next step continues even if no audio is present
        yield break;
    }

    yield return new WaitForSeconds(audioSource.clip.length);

    StartTheTask();
}
    public void SetLanguage()
    {
        isEnglish = false;
         Debug.Log("isEnglish>: " + isEnglish); // Debugging purpose
         
    }
    void AddArabicFixerToAllText()
{
    TextMeshProUGUI[] allTextElements = { timeDuration, instructionText, questionText,
        scoreText,levelText};

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
    public void StartTheTask()
    {
        AddArabicFixerToAllText();
        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(OnSubmitAnswer);
        UpdateScoreText();
        StartCoroutine(StartTask());
    }

    IEnumerator StartTask()
    {
        UpdateScoreText();
        for (int i = 1; i <= 5; i++)
        {
            submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(OnSubmitAnswer);
            UpdateScoreText();

            if(i>maxLevel){
                EndTask();
                // return;
            }
            else{

            
            StartLevel(i);
           
            while (questionsRemaining > 0)
            {
                SpawnBalls();
                UpdateScoreText();
                StartCoroutine(StartCountdown(displayTime)); // Start countdown coroutine
                yield return new WaitForSeconds(displayTime);

                ClearBalls();
                SetArabicText(instructionText, isEnglish ? "How many balls were there?" : "كم عدد الكرات التي كانت موجودة؟");

                submitButton.interactable = true;

                // Update the question text
                UpdateQuestionText();
                UpdateQuestionText();
                yield return new WaitUntil(() => !submitButton.interactable);
                questionsRemaining--;
            }
            }
            // Congratulate the player after completing all questions in the level
            SetArabicText(instructionText, isEnglish ? "Congratulations! Level " + currentLevel + " completed!" : "تهانينا! تم إكمال المستوى " + currentLevel + "!");

            yield return new WaitForSeconds(2f); // Wait for 2 seconds before starting the next level
        }
    }
     IEnumerator StartCountdown(float duration)
{
    float timeRemaining = duration;
    while (timeRemaining > 0)
    {
        SetArabicText(timeDuration, isEnglish ? "Time: " + timeRemaining.ToString("F1") + "s" : "الوقت: " + timeRemaining.ToString("F1") + " ثانية");
        timeRemaining -= Time.deltaTime;
        yield return null;
    }
    SetArabicText(timeDuration, isEnglish ? "Time: 0.0s" : "الوقت: 0.0 ثانية");
}


    void StartLevel(int level)
    {
        currentLevel = level;
        switch (level)
        {
            case 1:
                displayTime = 2f;
                break;
            case 2:
                displayTime = 1.5f;
                break;
            case 3:
                displayTime = 1f;
                break;
            case 4:
                displayTime = 0.5f;
                break;
            case 5:
                displayTime = 0.25f;
                break;
        }
        questionsRemaining = questionPerLevel;
        UpdateScoreText();
        SetArabicText(levelText, isEnglish ? "Level: " + currentLevel : "المستوى: " + currentLevel);
SetArabicText(instructionText, isEnglish ? "Level " + level + ": Count the balls!" : "المستوى " + level + ": احسب الكرات!");

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

         Vector3 forwardPosition = spawnPoint.position + spawnPoint.forward * forwardOffset;


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
    SetArabicText(instructionText, isEnglish ? "Correct!" : "صحيح!");
    audioSource.clip = correctSound;  // Set correct sound clip
}
else
{
    consecutiveCorrect = 0;
    errorCount++; // Increment error count
    SetArabicText(instructionText, isEnglish ? "Incorrect. Try again!" : "خطأ. حاول مرة أخرى!");
    audioSource.clip = wrongSound;    // Set wrong sound clip
    
    // Check if errors have reached 4
    if (errorCount >= maxTotalErrors)
    {
        EndTask(); // Call the method to end the task
        return;    // Stop further processing
    }
}


    audioSource.Play(); // Play the selected sound
    UpdateScoreText();
}

void EndTask()
{
    StopAllCoroutines(); // Stop all coroutines to end the task
    SetArabicText(instructionText, isEnglish ? "Task ended due to too many errors." : "تم إنهاء المهمة بسبب عدد كبير من الأخطاء.");
    submitButton.interactable = false;
    ShowKeyboard(false); // Hide the keyboard
    ClearBalls();        // Clear the balls
    trialEnd?.Invoke();
}


    void UpdateScoreText()
    {
        SetArabicText(scoreText, isEnglish ? score+ ":Score" : "النقاط: " + score);
         SetArabicText(levelText, isEnglish ? currentLevel+ ":Level" : "المستوى: " + currentLevel);

    }

    void UpdateQuestionText()
    {
        int currentQuestion = questionPerLevel - questionsRemaining + 1;
        SetArabicText(questionText, isEnglish ? currentQuestion + "/" + questionPerLevel+":Question" : "السؤال: " + currentQuestion + "/" + questionPerLevel);

    }

    void ShowKeyboard(bool show)
    {
        keyboard.SetActive(show); // Show or hide the keyboard
    }
}
