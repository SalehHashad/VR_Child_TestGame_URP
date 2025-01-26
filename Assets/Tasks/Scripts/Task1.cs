using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;  

public class Task1 : MonoBehaviour
{
    // UI Elements
    public GameObject fixationPoint; // The "+" sign for fixation
    public GameObject cuePrefab; // The "*" for the cue
    public TMP_Text levelText; // Displays the current level
    public TMP_Text questionCounterText; // Displays the current question
    public TMP_Text feedbackText; // Displays feedback (e.g., "Correct", "Wrong")
    public TextMeshProUGUI orientingText;
    public TextMeshProUGUI executiveControlText;
    public TextMeshProUGUI alertingText;
    // Level Prefabs
    public List<GameObject> arrowPrefabs; // Assign prefabs for each level in the Inspector
    private List<GameObject> spawnedArrows = new List<GameObject>();
    private List<TrialData> trialDataList = new List<TrialData>();

    // Buttons
    public GameObject rightButton; // Button for the "Right" answer
    public GameObject leftButton; // Button for the "Left" answer

    // Spawn Point
    public Transform spawnPoint; // Location where targets (arrows) will appear

    // Configurable Parameters
    private float fixationTime = 0.5f; // Time for fixation point display (in seconds)
    public float fixationTimeMin = 0.5f; // Time for fixation point display (in seconds)
    public float fixationTimeMax = 0.5f; // Time for fixation point display (in seconds)
    public float cueTime = 0.5f; // Time for cue display (in seconds)
    private float interTrialInterval = 1.0f; // Time between trials (in seconds)
    public float interTrialIntervalMax = 1.0f; // Time between trials (in seconds)
    public float interTrialIntervalMin = 1.0f; // Time between trials (in seconds)

    // Gameplay Variables
    private int currentLevel = 1; // Current game level
    private int totalLevels = 5; // Current game level
    public int totalQuestions = 96; // Questions per level
    private int currentQuestion = 0; // Current question number
    private float responseStartTime; // Time when target appears
    private bool isResponding = false; // If awaiting player response
    // Variable to control the offset between arrows
public float arrowOffset = 1.0f; // Adjust this in the Inspector for spacing
    // Data Recording
    private float totalResponseTime = 0; // Sum of all response times
    private int correctResponses = 0; // Count of correct responses

    private GameObject currentTarget; // The currently active arrow

    public float cueOffset = 2.0f; // Distance between cues for Spatial/Double Cue
    public float targetOffset = 2.0f; // Distance between cues for Spatial/Double Cue
    private string[] cueTypes = { "Spatial Cue", "Double Cue", "No Cue", "Center Cue" };
    private string currentCueType; // The current cue type
    private List<GameObject> activeCues = new List<GameObject>(); // Track active cues

    // Arrow Directions
    private string[] directions = { "Left", "Right" }; // Possible arrow directions
    private string currentCorrectDirection; // Direction of the current arrow
private TrialData currentTrialData;
private float iti;  // Inter-Trial Interval in milliseconds (add this at the top of the class)
  private string participantResponse; 
  private string currentLocation; 
  string congruencyState;

  private int trialNumber=0;
private AudioSource audioSource;
Vector3 targetPosition; // Default to center

public AudioClip correctAnswerClip;
    public AudioClip wrongAnswerClip;

// Configurable error thresholds
public int maxTotalErrors = 4;          // Maximum total incorrect answers per level
public int maxConsecutiveErrors = 2;   // Maximum consecutive incorrect answers

// Class-level counters
private int totalErrors = 0;          // Total incorrect answers per level
private int consecutiveErrors = 0;   // Consecutive incorrect answers
private bool isTaskEnded = false;
public GameObject resultUI;

private Button rightButtonComponent; // Reference to the Button component
    private Button leftButtonComponent;  // Reference to the Button component

   
    public float CalculateFixationTimeAndIti(int currentLevel, int currentQuestion, bool isFixationTime)
    {
        // Ensure currentLevel and currentQuestion are within valid ranges
        currentLevel = Mathf.Clamp(currentLevel, 1, totalLevels);
        currentQuestion = Mathf.Clamp(currentQuestion, 0, totalQuestions);

        // Calculate the progress factor (normalized value between 0 and 1)
        float levelProgress = (currentLevel - 1) / (float)(totalLevels - 1);
        float questionProgress = currentQuestion / (float)totalQuestions;
        float overallProgress = (levelProgress + questionProgress) / 2f;

        // Interpolate fixationTime between fixationTimeMax and fixationTimeMin
        if(isFixationTime)
        return Mathf.Lerp(fixationTimeMax, fixationTimeMin, overallProgress);
        else
        return Mathf.Lerp(interTrialIntervalMax, interTrialIntervalMin, overallProgress);
    }


    void Start()
    {
        rightButtonComponent = rightButton.GetComponent<Button>();
        leftButtonComponent = leftButton.GetComponent<Button>();
        // Check if AudioSource is already attached to the GameObject
        audioSource = GetComponent<AudioSource>();

        // If no AudioSource exists, create one automatically
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ensure AudioSource properties (optional, you can adjust as needed)
        audioSource.loop = false; // Prevent looping of sounds
        audioSource.playOnAwake = false; // Don't play on awake
        // Assign button listeners
        rightButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => AnswerSelected("Right"));
        leftButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => AnswerSelected("Left"));
// activate the buttons
    if (rightButton != null) rightButton.SetActive(true);
    if (leftButton != null) leftButton.SetActive(true);

    // deActivate the result UI
    if (resultUI != null) resultUI.SetActive(false);

        StartCoroutine(GameFlow(1));
    }

    public void SetLevelAndStartTask(int level)
    {
        // currentLevel = level; // Set the selected level
        isTaskEnded=false;
        Debug.Log("Current Level: " + currentLevel); // Debugging purpose
resultUI.SetActive(false);
rightButton.SetActive(true);
    leftButton.SetActive(true);
    ShowFeedback(" ");
        // Call StartClick to begin the task
        StartCoroutine(GameFlow(level));
    }

    IEnumerator GameFlow(int lvl)
{
    
    for (int level=lvl; level <= arrowPrefabs.Count; level++) // Loop through levels
    {
        if (isTaskEnded) yield break; // Stop if task has ended

        currentLevel = level;
        totalErrors = 0;
        currentQuestion = 0;
        UpdateUI();

        for (int i = 0; i < totalQuestions; i++) // Loop through questions in a level
        {
            ShowFeedback("Determine the direction for objects spawned at the center");
            if (isTaskEnded) yield break; // Stop if task has ended

            currentQuestion++;
            UpdateUI();

            // Run a trial
            yield return StartCoroutine(RunTrial());
            interTrialInterval = CalculateFixationTimeAndIti(currentLevel, currentQuestion, false);
            Debug.Log("interTrialInterval: " + interTrialInterval);

            // Short delay before the next trial
            yield return new WaitForSeconds(interTrialInterval);
        }

        // Display level summary
        ShowFeedback($"Level {level} Complete! Accuracy: {correctResponses * 100 / totalQuestions}%");
        correctResponses = 0;
        yield return new WaitForSeconds(3.0f);
        ClearFeedback();
    }

    // Game complete
    EndTask();
    if (!isTaskEnded)
    {
        ShowFeedback("Task Complete! Thank you for participating.");
    }
}


 IEnumerator RunTrial()
{
     rightButtonComponent.interactable = false; 
        leftButtonComponent.interactable = false;  

    // Create an object to store trial data
    TrialData currentTrialData = new TrialData();

    trialNumber++;
    currentTrialData.trialNumber = trialNumber;
    // fixationTime=Random.Range(fixationTimeMin, fixationTimeMax);
    fixationTime=CalculateFixationTimeAndIti(currentLevel,currentQuestion,true);
    
    // Record the fixation time (converted to seconds for usage in yield)
    currentTrialData.fixationTime = fixationTime * 1000f; // in milliseconds

    // Display fixation point
    if (fixationPoint != null)
    {
        fixationPoint.SetActive(true);
    }
    yield return new WaitForSeconds(fixationTime);

    // Hide fixation point and display cue
    if (fixationPoint != null)
    {
        fixationPoint.SetActive(false);
    }

    // Display the cue
    DisplayCue();

    yield return new WaitForSeconds(cueTime);
    currentTrialData.cueType = currentCueType;
    // Record Inter-Trial Interval (ITI)
    currentTrialData.iti = interTrialInterval * 1000f; // in milliseconds
    ClearCues(); // Clear any active cues

    // Hide cue and display target
    SpawnTarget(); // Spawn the target arrow at the spawn point
    yield return new WaitForSeconds(fixationTime);
    DestroyPreviousArrows();
     rightButtonComponent.interactable = true; 
        leftButtonComponent.interactable = true;  

    // Record the direction of the target
    currentTrialData.direction = currentCorrectDirection;
 currentTrialData.targetLocation=currentLocation;
    // Record response
    isResponding = true;
    responseStartTime = Time.time;

    while (isResponding)
    {
        yield return null;
    }
    
    currentTrialData.congruencyState=congruencyState;
    // Calculate response time
    currentTrialData.responseTime = (Time.time - responseStartTime) * 1000f; // in milliseconds

    // Store the participant's response (this should be updated based on actual user input)
    currentTrialData.response = participantResponse;  // Assume participantResponse is tracked elsewhere

    // Determine accuracy based on response
    currentTrialData.accuracy = (currentTrialData.response == currentTrialData.direction) ? 1 : 0;
    correctResponses+=currentTrialData.accuracy;
    Debug.Log("correctResponses:"+correctResponses);
    // Log the trial data for later analysis (you can print, store in a file, or add to a list)
    LogTrialData(currentTrialData);

    // Hide target
    if (currentTarget != null)
    {
        Destroy(currentTarget);
    }
}
void LogTrialData(TrialData trialData)
{
    // Log the trial data to the console for debugging or analysis
    Debug.Log("Trial Data:");
    Debug.Log("Trial Number: " + trialData.trialNumber);
    Debug.Log("Fixation Time: " + trialData.fixationTime + " ms");
    Debug.Log("ITI: " + trialData.iti + " ms");
    Debug.Log("Direction: " + trialData.direction);
    Debug.Log("Response: " + trialData.response);
    Debug.Log("Accuracy: " + trialData.accuracy);
    Debug.Log("Response Time: " + trialData.responseTime + " ms");
    Debug.Log("Congruency State: " + trialData.congruencyState);
    Debug.Log("Cue Type: " + trialData.cueType);
    Debug.Log("Target Location: " + trialData.targetLocation);

    // Optionally, you can add the trial data to a list for further analysis later
    trialDataList.Add(trialData);  // trialDataList should be a List<TrialData> defined elsewhere
}




void DisplayCue()
{
    

    string[] cueTypes = { "Spatial Cue", "Double Cue", "No Cue", "Center Cue" };
    currentCueType = cueTypes[Random.Range(0, cueTypes.Length)];
    

    if (spawnPoint == null)
    {
        Debug.LogError("spawnPoint is not assigned.");
        return; // Exit early if spawnPoint is null
    }

    // Ensure cueOffset is properly assigned (this can be a value you define)
    float cueOffset = 2.0f; // Adjust this based on your needs

    if (currentCueType == "Spatial Cue")
    {
        Vector3 offsetPosition = spawnPoint.position + new Vector3(0, cueOffset, 0);
        SpawnCue(offsetPosition);
    }
    else if (currentCueType == "Double Cue")
    {
        Vector3 bottomPosition = spawnPoint.position + new Vector3(0, -cueOffset, 0);
        Vector3 topPosition = spawnPoint.position + new Vector3(0, cueOffset, 0);
        SpawnCue(bottomPosition);
        SpawnCue(topPosition);
    }
    else if (currentCueType == "Center Cue")
    {
        SpawnCue(spawnPoint.position);
    }
    // "No Cue" does not display anything.
}



void ClearCues()
{
    foreach (GameObject cue in activeCues)
    {
        if (cue != null)
        {
            Destroy(cue);
        }
    }
    activeCues.Clear();
}
void SpawnCue(Vector3 position)
{
    if (cuePrefab == null)
    {
        Debug.LogError("Cue prefab not assigned!");
        return;
    }

    GameObject cue = Instantiate(cuePrefab, position, Quaternion.identity);
    cue.SetActive(true);
    activeCues.Add(cue); // Track for cleanup later
}




void SpawnTarget()
{
    // Destroy any previously spawned arrows
    DestroyPreviousArrows();

    // Validate input
    if (arrowPrefabs == null || arrowPrefabs.Count == 0)
    {
        Debug.LogError("SpawnTarget: No arrow prefabs assigned.");
        return;
    }

    if (spawnPoint == null)
    {
        Debug.LogError("SpawnTarget: Spawn point is not assigned.");
        return;
    }

    // Randomize the target location (top, bottom, center)
    targetPosition = spawnPoint.position; // Default to center
    string[] targetLocations = { "Top", "Bottom", "Center" };
    string targetLocation = targetLocations[Random.Range(0, targetLocations.Length)];
    currentLocation=targetLocation;

    if (targetLocation == "Top")
    {
        targetPosition += new Vector3(0, targetOffset, 0); // Move up by offset
    }
    else if (targetLocation == "Bottom")
    {
        targetPosition += new Vector3(0, -targetOffset, 0); // Move down by offset
    }

    // Randomize the congruency state
    congruencyState = RandomCongruencyState();
     

    // Determine the direction for the central arrow
    currentCorrectDirection = directions[Random.Range(0, directions.Length)];

    // Create the central arrow at the randomized target position
    currentTarget = Instantiate(arrowPrefabs[currentLevel - 1], targetPosition, spawnPoint.rotation);
    RotateArrow(currentTarget, currentCorrectDirection);

    // Add central arrow to the list
    spawnedArrows.Add(currentTarget);

    // Surrounding arrows logic
    if (congruencyState != "Neutral")
    {
        CreateSurroundingArrows(congruencyState,targetLocation);
    }

    // Ensure the central arrow is active after instantiation
    currentTarget.SetActive(true);
}


   

string RandomCongruencyState()
{
    // Randomly return Congruent, Incongruent, or Neutral
    string[] states = { "Congruent", "Incongruent", "Neutral" };
    return states[Random.Range(0, states.Length)];
}

void CreateSurroundingArrows(string state, string location)
{
    // Calculate positions for arrows in a horizontal line
    Vector3 centralPosition = spawnPoint.position;

    // Determine the vertical offset based on location
    float verticalOffset = targetOffset; // Default value
    if (location == "Top")
    {
        verticalOffset = targetOffset; // No offset for Top
    }
    else if (location == "Bottom")
    {
        verticalOffset = -targetOffset; // Negative offset for Bottom
    }
    else if(location == "Center")
    {
        verticalOffset = 0;
    }

    // Left arrows
    for (int i = 1; i <= 2; i++)
    {
        Vector3 position = centralPosition + new Vector3(-i * arrowOffset, verticalOffset, 0);
        SpawnSurroundingArrow(position, state);
    }

    // Right arrows
    for (int i = 1; i <= 2; i++)
    {
        Vector3 position = centralPosition + new Vector3(i * arrowOffset, verticalOffset, 0);
        SpawnSurroundingArrow(position, state);
    }
}


    void SpawnSurroundingArrow(Vector3 position, string state)
    {
        // Instantiate the arrow
        GameObject surroundingArrow = Instantiate(arrowPrefabs[currentLevel - 1], position, spawnPoint.rotation);

        // Determine the direction for the surrounding arrow
        string surroundingDirection = state == "Congruent"
            ? currentCorrectDirection
            : OppositeDirection();

        RotateArrow(surroundingArrow, surroundingDirection);

        // Activate the arrow and add it to the list
        surroundingArrow.SetActive(true);
        spawnedArrows.Add(surroundingArrow);
    }

    string OppositeDirection()
    {

        // Random.Range(0, directions.Length)
        return directions[Random.Range(0, directions.Length)];
    }


    void RotateArrow(GameObject arrow, string direction)
    {
        if (arrow == null)
        {
            Debug.LogError("RotateArrow: Arrow is null.");
            return;
        }

        if (direction == "Left")
        {
            arrow.transform.rotation = Quaternion.Euler(0, 180, 0); // Rotate for left direction
        }
        else if (direction == "Right")
        {
            arrow.transform.rotation = Quaternion.Euler(0, 0, 0); // Rotate for right direction
        }
        else
        {
            Debug.LogWarning("RotateArrow: Invalid direction provided.");
        }
    }

    void UpdateUI()
    {
        levelText.text = $"Level: {currentLevel}";
        questionCounterText.text = $"Question: {currentQuestion}/{totalQuestions}";
        
    }

    void ShowFeedback(string message)
    {
        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);
    }

    void ClearFeedback()
    {
        feedbackText.gameObject.SetActive(false);
    }



void AnswerSelected(string direction)
{
    if (!isResponding) return;

    if (currentCorrectDirection == null)
    {
        Debug.LogError("currentCorrectDirection is null!");
        return;
    }

    float responseTime = Time.time - responseStartTime;
    totalResponseTime += responseTime;

    // Check if the response is correct
    bool correctResponse = (direction == currentCorrectDirection);
    participantResponse = direction;

    // Provide feedback
    ShowFeedback(correctResponse ? "Correct!" : "Wrong!");
    StartCoroutine(HideFeedbackAfterDelay(1.0f));

    // Play sound based on correctness of the response
    PlayAnswerSound(correctResponse);

    isResponding = false;

    // Update error counters
    if (correctResponse)
    {
        consecutiveErrors = 0; // Reset consecutive errors on correct answer
    }
    else
    {
        totalErrors++;
        consecutiveErrors++;

        // Check if end task conditions are met
        if (totalErrors >= maxTotalErrors || consecutiveErrors >= maxConsecutiveErrors)
        {
            EndTask(); // Call the end task function
            return;    // Exit early to prevent further processing
        }
    }
      rightButtonComponent.interactable = false; 
        leftButtonComponent.interactable = false;  
    // Destroy arrows after the answer
    DestroyPreviousArrows();
}

void EndTask()
{
    Debug.Log("Task ended due to too many errors.");
    ShowFeedback("Task ended due to too many errors.");
    isTaskEnded = true; // Signal to stop GameFlow
    resultUI.SetActive(true);
rightButton.SetActive(false);
    leftButton.SetActive(false);
    StopAllCoroutines(); // Optionally stop all coroutines if necessary
    // Additional logic for ending the task (e.g., showing summary screen)

    // Calculate Mean Reaction Times for various conditions
    float meanReactionTimeCentralCue = CalculateMeanReactionTimecueType("Center Cue");
    float meanReactionTimeSpatialCue = CalculateMeanReactionTimecueType("Spatial Cue");
    float meanReactionTimeIncongruent = CalculateMeanReactionTimecongruencyState("Incongruent");
    float meanReactionTimeCongruent = CalculateMeanReactionTimecongruencyState("Congruent");
    
    // Calculate Mean Reaction Times for No Cue and Double Cue
    float meanReactionTimeNoCue = CalculateMeanReactionTimecueType("No Cue");
    float meanReactionTimeDoubleCue = CalculateMeanReactionTimecueType("Double Cue");

    // Calculate Orienting, Executive Control, and Alerting
    float orienting = meanReactionTimeCentralCue - meanReactionTimeSpatialCue;
    float executiveControl = meanReactionTimeIncongruent - meanReactionTimeCongruent;
    float alerting = meanReactionTimeNoCue - meanReactionTimeDoubleCue;
    // Update the TextMesh Pro UI elements with the results
        orientingText.text = $"Orienting: {orienting} ms";
        executiveControlText.text = $"Executive Control: {executiveControl} ms";
        alertingText.text = $"Alerting: {alerting} ms";



    // Use Debug.Log to display results
    // Debug.Log($"Orienting: {orienting} ms");
    // Debug.Log($"Executive Control: {executiveControl} ms");
    // Debug.Log($"Alerting: {alerting} ms");

    // // Optionally display other results like mean reaction times
    // Debug.Log($"Mean Reaction Time (Central Cue): {meanReactionTimeCentralCue} ms");
    // Debug.Log($"Mean Reaction Time (Spatial Cue): {meanReactionTimeSpatialCue} ms");
    // Debug.Log($"Mean Reaction Time (Incongruent): {meanReactionTimeIncongruent} ms");
    // Debug.Log($"Mean Reaction Time (Congruent): {meanReactionTimeCongruent} ms");
    // Debug.Log($"Mean Reaction Time (No Cue): {meanReactionTimeNoCue} ms");
    // Debug.Log($"Mean Reaction Time (Double Cue): {meanReactionTimeDoubleCue} ms");
}

private float CalculateMeanReactionTimecongruencyState(string congruencyState)
{
    // Filter the trials based on the congruencyState
    var filteredTrials = trialDataList.Where(t => t.congruencyState.ToLower()==congruencyState.ToLower()).ToList();

    // If there are no trials for the given congruencyState, return 0
    if (filteredTrials.Count == 0)
        return 0.0f;

    // Return the average of the response times for the filtered trials
    return filteredTrials.Average(t => t.responseTime);
}

private float CalculateMeanReactionTimecueType(string cueType)
{
    // Filter trials based on the cue type
    var filteredTrials = trialDataList.Where(t => t.cueType.ToLower() == cueType.ToLower()).ToList();

    // If no trials match, return 0
    if (filteredTrials.Count == 0)
        return 0.0f;

    // Calculate and return the average response time
    return filteredTrials.Average(t => t.responseTime);
}






    // Function to play the correct or wrong sound
    void PlayAnswerSound(bool correct)
    {
        // Play the correct sound for correct answers, and wrong sound for incorrect answers
        if (audioSource != null)
        {
            if (correct)
            {
                audioSource.PlayOneShot(correctAnswerClip);
            }
            else
            {
                audioSource.PlayOneShot(wrongAnswerClip);
            }
        }
        else
        {
            Debug.LogError("AudioSource not found!");
        }
    }



    void DestroyPreviousArrows()
    {
        // Destroy all previously spawned arrows
        foreach (var arrow in spawnedArrows)
        {
            if (arrow != null)
            {
                Destroy(arrow);
            }
        }

        // Clear the list
        spawnedArrows.Clear();
    }

    IEnumerator HideFeedbackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClearFeedback();
    }






}
[System.Serializable]
public class TrialData
{
    public int trialNumber;         // The number of the trial (1, 2, 3, ...)
    public float fixationTime;      // Fixation time in milliseconds
    public float iti;               // Inter-Trial Interval in milliseconds
    public string direction;        // Direction of the target ('left' or 'right')
    public string response;         // Participant's response ('left' or 'right')
    public int accuracy;            // 1 if correct, 0 if incorrect
    public float responseTime;      // Response time in milliseconds
    public string congruencyState;  // State of congruency ('Congruent', 'Incongruent', etc.)
    public string cueType;          // Type of cue ('valid', 'invalid', etc.)
    public string targetLocation;   // Location of the target ('top', 'bottom', 'center')
}