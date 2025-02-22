using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class NumberPresentation : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject onboardingUI;
    [SerializeField] private GameObject taskUI;
    [SerializeField] private GameObject resultsUI;

    [Tooltip("The Text component to display numbers.")]
    [SerializeField] private TextMeshProUGUI digitDisplay;

    [Tooltip("The Text component to display task time remaining.")]
    [SerializeField] private TextMeshProUGUI taskTimerDisplay;

    [Tooltip("The Text component to display the time until the next digit appears.")]
    [SerializeField] private TextMeshProUGUI nextDigitTimerDisplay;

    [Tooltip("The Text component to display the time until the next digit appears.")]
    [SerializeField] private TextMeshProUGUI TotalDigit;

    [Tooltip("The Text component to display total commission errors.")]
    [SerializeField] private TextMeshProUGUI totalCommissionErrorsDisplay;

    [Tooltip("The Text component to display total omission errors.")]
    [SerializeField] private TextMeshProUGUI totalOmissionErrorsDisplay;

    [Tooltip("The Text component to display total correct responses.")]
    [SerializeField] private TextMeshProUGUI totalCorrectResponsesDisplay;

    [Tooltip("The Text component to display average response time.")]
    [SerializeField] private TextMeshProUGUI averageResponseTimeDisplay;

    [Tooltip("The Text component to display correct response percentage.")]
    [SerializeField] private TextMeshProUGUI correctResponsePercentageDisplay;

    // Add new UI reference for displaying the current level
    [Tooltip("The Text component to display the current level.")]
    [SerializeField] private TextMeshProUGUI levelDisplay;

    [Header("Timing Settings")]
    [SerializeField] private float initialInterval = 2.0f;
    [SerializeField] private float minInterval = 0.5f;
    [SerializeField] private float taskDuration = 500.0f;
    [SerializeField] private float startDelay = 5.0f; // 5-second delay before starting task

    [Header("Level Progression")]
    [SerializeField] private int digitsPerLevel = 45;
    [SerializeField] private int MaxLevel = 1;

    [Header("Response Input")]
    [SerializeField] private InputActionReference response;

    [Header("Task Settings")]
    [SerializeField] private int targetNumber = 3;

    [Header("Audio Feedback")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip correctResponseSound;
    [SerializeField] private AudioClip commissionErrorSound;
    public AudioClip trialEndClip;

    [Header(" Events")]
    public UnityEvent trialEnd;

    private float currentInterval;
    private int currentLevel = 1;
    private float nextDisplayTime = 0.0f;
    private float elapsedTime = 0.0f;
    private int totalDigitsShown = 0;
    private bool taskActive = false;

    private int commissionErrorCount = 0;
    private int omissionErrorCount = 0;
    private int correctResponseCount = 0;
    private bool responseExpected = false;
    private float lastDigitTime = 0.0f;
    private int lastDisplayedNumber = -1;
    private bool hasResponded = false;

    private List<float> correctResponseTimes = new List<float>();

    public bool isEnglish = true; // Default to English

    public void SetLanguage()
    {
        isEnglish = false;
         Debug.Log("isEnglish>: " + isEnglish); // Debugging purpose
         
    }
    void AddArabicFixerToAllText()
{
    TextMeshProUGUI[] allTextElements = { taskTimerDisplay, nextDigitTimerDisplay, TotalDigit,
        totalCommissionErrorsDisplay, totalOmissionErrorsDisplay, totalCorrectResponsesDisplay,
        averageResponseTimeDisplay, correctResponsePercentageDisplay, levelDisplay };

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

    private void Start()
    {
        // ShowOnboardingUI();
AddArabicFixerToAllText();
        response.action.Enable();
        response.action.performed += OnResponseAction;
    }

    private void Update()
{
    if (!taskActive)
        return;

    elapsedTime += Time.deltaTime;
    float remainingTaskTime = Mathf.Max(0, taskDuration - elapsedTime);
    float nextDigitTimeRemaining = Mathf.Max(0, nextDisplayTime - Time.time);

    SetArabicText(taskTimerDisplay, 
    !isEnglish 
    ? $"الوقت المتبقي: {remainingTaskTime:F0} ثانية" 
    : $"Time Remaining:{remainingTaskTime:F0}s");

SetArabicText(nextDigitTimerDisplay, 
    !isEnglish 
    ? $"التالي في: {nextDigitTimeRemaining:F0} ثانية" 
    : $"Next In:{nextDigitTimeRemaining:F0}s");

SetArabicText(TotalDigit, 
    !isEnglish 
    ? $"إجمالي الأرقام: {totalDigitsShown:F0}" 
    : $"Total Digits:{totalDigitsShown:F0}");

SetArabicText(levelDisplay, 
    !isEnglish 
    ? $"المستوى: {currentLevel}" 
    : $"Level:{currentLevel}");


    if (elapsedTime >= taskDuration)
    {
        EndTask();
        return;
    }

    if (Time.time >= nextDisplayTime)
    {
        // If the user didn't respond and the last displayed number was the target number
        if (responseExpected && !hasResponded)
        {
            Debug.Log("lastDisplayedNumber:"+lastDisplayedNumber);
            
                omissionErrorCount++;
                ChangeNumberColor(Color.red);
                PlaySound(commissionErrorSound);
                
                Debug.Log("Erorr");
            
        }
        if(!responseExpected&& !hasResponded){
            if (lastDisplayedNumber == targetNumber)
            {
                correctResponseCount++;
                PlaySound(correctResponseSound);
                ChangeNumberColor(Color.green);
                Debug.Log("ok");
                
                
            }
        }

        ShowNextDigit();
        nextDisplayTime = Time.time + currentInterval;
    }

    UpdateDifficulty();
}
public void mainTaskStart()
{
    if (audioSource == null)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    if (trialEndClip == null)
    {
        Debug.LogError("trialEndClip is null! Assign it in the inspector.");
        OnStartButtonClicked(); // Skip audio and proceed
        return;
    }

    // Play the audio
    PlaySound(trialEndClip);

    // Start a coroutine to wait until the audio finishes
    StartCoroutine(WaitForAudioToFinish());
}


    private IEnumerator WaitForAudioToFinish()
{
    if (audioSource == null || audioSource.clip == null)
    {
        Debug.LogError("AudioSource or AudioClip is null. Skipping wait.");
        OnStartButtonClicked(); // Ensure the next step continues even if no audio is present
        yield break;
    }

    yield return new WaitForSeconds(audioSource.clip.length);

    OnStartButtonClicked();
}

    public void OnStartButtonClicked()
    {
        ShowTaskUI();
        StartCoroutine(StartTaskWithDelay());
    }

    private IEnumerator StartTaskWithDelay()
    {
        // Countdown before starting the task
        float countdown = startDelay;
        while (countdown > 0)
        {
           SetArabicText(taskTimerDisplay, 
    !isEnglish 
    ? $"البدء في: {countdown:F0} ثانية" 
    : $"Starting in:{countdown:F0}");

            countdown -= Time.deltaTime;
            yield return null;
        }

        StartTask();
    }

    private void StartTask()
    {
        taskActive = true;
        currentInterval = initialInterval;
        ShowNextDigit();
    }

    private void ShowNextDigit()
    {
        int randomDigit;
        do
        {
            randomDigit = Random.Range(1, 10);
        } while (randomDigit == lastDisplayedNumber);

        digitDisplay.text = randomDigit.ToString();
        ChangeNumberColor(Color.black);
        totalDigitsShown++;
        lastDisplayedNumber = randomDigit;
        responseExpected = randomDigit != targetNumber;
        lastDigitTime = Time.time;
        hasResponded = false;
    }

    private void UpdateDifficulty()
    {
        if(currentLevel>MaxLevel){
            EndTask();
            
        }
        if (totalDigitsShown >= currentLevel * digitsPerLevel)
        {
            currentLevel++;
            currentInterval = Mathf.Max(currentInterval - 0.3f, minInterval);
        }
    }

    private void EndTask()
    {
        taskActive = false;
        // ShowResultsUI();
trialEnd?.Invoke();
        float averageResponseTime = correctResponseTimes.Count > 0 ? CalculateAverageResponseTime() : 0.0f;
        float correctResponsePercentage = (totalDigitsShown > 0) ? (float)correctResponseCount / totalDigitsShown * 100 : 0.0f;

        SetArabicText(totalCommissionErrorsDisplay, 
    !isEnglish 
    ? $"إجمالي أخطاء العمولة: {commissionErrorCount}" 
    : $"Total Commission Errors:{commissionErrorCount}");

SetArabicText(totalOmissionErrorsDisplay, 
    !isEnglish 
    ? $"إجمالي أخطاء الحذف: {omissionErrorCount}" 
    : $"Total Omission Errors:{omissionErrorCount}");

SetArabicText(totalCorrectResponsesDisplay, 
    !isEnglish 
    ? $"إجمالي الإجابات الصحيحة: {correctResponseCount}" 
    : $"Total Correct Responses:{correctResponseCount}");

SetArabicText(averageResponseTimeDisplay, 
    !isEnglish 
    ? $"متوسط وقت الاستجابة: {averageResponseTime:F2} ثانية" 
    : $"Average Response Time:{averageResponseTime:F2}s");

SetArabicText(correctResponsePercentageDisplay, 
    !isEnglish 
    ? $"نسبة الاستجابات الصحيحة: {correctResponsePercentage:F2}%" 
    : $"Correct Response Percentage:{correctResponsePercentage:F2}%");


        response.action.Disable();
    }

    private void OnResponseAction(InputAction.CallbackContext context)
    {
        if (!taskActive || hasResponded)
            return;

        int currentDigit;
        if (int.TryParse(digitDisplay.text, out currentDigit))
        {
            float responseTime = Time.time - lastDigitTime;

            if (currentDigit == targetNumber)
            {
                commissionErrorCount++;
                ChangeNumberColor(Color.red);
                PlaySound(commissionErrorSound);
                
                
            }
            else
            {
                correctResponseCount++;
                correctResponseTimes.Add(responseTime);
                PlaySound(correctResponseSound);
                ChangeNumberColor(Color.green);
            }

            hasResponded = true;
        }
    }
private void ChangeNumberColor(Color color)
    {
        if (digitDisplay != null)
        {
            digitDisplay.color = color;
        }
    }
    private float CalculateAverageResponseTime()
    {
        float sum = 0.0f;
        foreach (float time in correctResponseTimes)
        {
            sum += time;
        }
        return sum / correctResponseTimes.Count;
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void ShowOnboardingUI()
    {
        onboardingUI.SetActive(true);
        taskUI.SetActive(false);
        resultsUI.SetActive(false);
    }

    private void ShowTaskUI()
    {
        onboardingUI.SetActive(false);
        taskUI.SetActive(true);
    }

    public void ShowResultsUI()
    {
        taskUI.SetActive(false);
        resultsUI.SetActive(true);
    }

    private void OnDestroy()
    {
        response.action.performed -= OnResponseAction;
    }
}
