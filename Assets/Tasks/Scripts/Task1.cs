using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Task1 : MonoBehaviour
{
    // UI Elements
    public GameObject fixationPoint; // The "+" sign for fixation
    public GameObject cuePrefab; // The "*" for the cue
    public TMP_Text levelText; // Displays the current level
    public TMP_Text questionCounterText; // Displays the current question
    public TMP_Text feedbackText; // Displays feedback (e.g., "Correct", "Wrong")

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
    public float fixationTime = 0.5f; // Time for fixation point display (in seconds)
    public float cueTime = 0.5f; // Time for cue display (in seconds)
    public float interTrialInterval = 1.0f; // Time between trials (in seconds)

    // Gameplay Variables
    private int currentLevel = 1; // Current game level
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


    void Start()
    {
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

        StartCoroutine(GameFlow());
    }

    IEnumerator GameFlow()
    {
        for (int level = 1; level <= arrowPrefabs.Count; level++) // Loop through levels
        {
            currentLevel = level;
            currentQuestion = 0;
            UpdateUI();

            for (int i = 0; i < totalQuestions; i++) // Loop through questions in a level
            {
                currentQuestion++;
                UpdateUI();

                // Run a trial
                yield return StartCoroutine(RunTrial());

                // Short delay before the next trial
                yield return new WaitForSeconds(interTrialInterval);
            }

            // Display level summary
            ShowFeedback($"Level {level} Complete! Accuracy: {correctResponses * 100 / totalQuestions}%");
            correctResponses=0;
            yield return new WaitForSeconds(3.0f);
            ClearFeedback();
        }

        // Game complete
        ShowFeedback("Task Complete! Thank you for participating.");
    }

 IEnumerator RunTrial()
{
    // Create an object to store trial data
    TrialData currentTrialData = new TrialData();

    trialNumber++;
    currentTrialData.trialNumber = trialNumber;

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

        if (currentTarget == null)
        {
            Debug.LogError("currentTarget is null!");
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

        // Destroy arrows after the answer
        DestroyPreviousArrows();
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