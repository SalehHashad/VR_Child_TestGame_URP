using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using System.Collections.Generic;

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

    [Header("Response Input")]
    [SerializeField] private InputActionReference response;

    [Header("Task Settings")]
    [SerializeField] private int targetNumber = 3;

    [Header("Audio Feedback")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip correctResponseSound;
    [SerializeField] private AudioClip commissionErrorSound;

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

    private void Start()
    {
        ShowOnboardingUI();

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

    taskTimerDisplay.text = $"Time Remaining: {remainingTaskTime:F0}s";
    nextDigitTimerDisplay.text = $"Next In: {nextDigitTimeRemaining:F0}s";
    TotalDigit.text = $"TotalDigit: {totalDigitsShown:F0}";

    // Update the level display UI
    levelDisplay.text = $"Level: {currentLevel}";

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
            taskTimerDisplay.text = $"Starting in: {countdown:F0}"; // Update countdown UI
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
        if (totalDigitsShown >= currentLevel * digitsPerLevel)
        {
            currentLevel++;
            currentInterval = Mathf.Max(currentInterval - 0.3f, minInterval);
        }
    }

    private void EndTask()
    {
        taskActive = false;
        ShowResultsUI();

        float averageResponseTime = correctResponseTimes.Count > 0 ? CalculateAverageResponseTime() : 0.0f;
        float correctResponsePercentage = (totalDigitsShown > 0) ? (float)correctResponseCount / totalDigitsShown * 100 : 0.0f;

        totalCommissionErrorsDisplay.text = $"Total Commission Errors: {commissionErrorCount}";
        totalOmissionErrorsDisplay.text = $"Total Omission Errors: {omissionErrorCount}";
        totalCorrectResponsesDisplay.text = $"Total Correct Responses: {correctResponseCount}";
        averageResponseTimeDisplay.text = $"Average Response Time: {averageResponseTime:F2}s";
        correctResponsePercentageDisplay.text = $"Correct Response Percentage: {correctResponsePercentage:F2}%";

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

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
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

    private void ShowResultsUI()
    {
        taskUI.SetActive(false);
        resultsUI.SetActive(true);
    }

    private void OnDestroy()
    {
        response.action.performed -= OnResponseAction;
    }
}
