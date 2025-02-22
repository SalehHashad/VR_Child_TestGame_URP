using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using System.Collections; 
using UnityEngine.Events;
public class ObjectController : MonoBehaviour
{
    public GameObject objectPrefab; // Prefab to spawn
    public Transform spawnPoint; // Parent object with GridLayoutGroup
    public GameObject buttonPrefab; // Prefab for the buttons
    public GameObject taskObjects;
    public GameObject QuestionObject;

    [Range(0, 1)] public float transparencyValue = 0.5f;
    public Vector3 rotationAxis = Vector3.up; // Default to Y-axis

    private GameObject rotatingObject;
    private GameObject[] transparentObjects = new GameObject[3];
    private GameObject[] spawnedObjects = new GameObject[4]; // Array to hold spawned objects

    private bool hasRotated = false;
    private int score = 0;
    private int Question = 1;
    public int QuestionPerLevel = 50;
    public int MaxQuestionTrailLevel = 1;
    public float appearSeconds = 50;
    public float timeBetwenQ = 1f;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI QuestionText;
    public TextMeshProUGUI instructionText; // Instruction text for each step
    public TextMeshProUGUI verticalButtonText; // Instruction text for each step
    public TextMeshProUGUI horizontalButtonText; // Instruction text for each step

    private GameObject[] buttons = new GameObject[4]; // Array to hold button references
    private GameObject targetObject; // The target object to find in the buttons

    // Add two buttons for vertical and horizontal actions
    public Button verticalButton;
    public Button horizontalButton;

    private float targetRotation = 0f; // Track the rotation of the target object (0 or 90)
[Header(" Events")]
    public UnityEvent trialEnd;
    
[Header("Audio Feedback")]
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip incorrectSound;
    [SerializeField] private AudioClip trialEndClip;
    private AudioSource audioSource;

 
public bool isEnglish = true; // Default to English
public bool taskActive = true; // Default to English

public void mainTaskStart()
{
    if (audioSource == null)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    if (trialEndClip == null)
    {
        Debug.LogError("trialEndClip is null! Assign it in the inspector.");
        StartTask();
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
        StartTask(); // Ensure the next step continues even if no audio is present
        yield break;
    }

    yield return new WaitForSeconds(audioSource.clip.length);

    StartTask();
}
    public void SetLanguage()
    {
        isEnglish = false;
         Debug.Log("isEnglish>: " + isEnglish); // Debugging purpose
         
    }
    void AddArabicFixerToAllText()
{
    TextMeshProUGUI[] allTextElements = { instructionText, QuestionText,
        scoreText,horizontalButtonText,verticalButtonText};

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
    // private void SetArabicText(TMP_Text textElement, string newText)
    // {
    //     if (textElement != null)
    //     {
    //         ArabicFixerTMPRO arabicFixer = textElement.GetComponent<ArabicFixerTMPRO>();
    //         if (arabicFixer != null)
    //         {
    //             arabicFixer.fixedText = newText;
    //         }
    //     }
    // }
    private void SetArabicText(TMP_Text textElement, string newText)
{
    if (textElement != null)
    {
        ArabicFixerTMPRO arabicFixer = textElement.GetComponent<ArabicFixerTMPRO>();
        if (arabicFixer != null)
        {
            arabicFixer.fixedText = newText;
        }
        textElement.text = newText; // Ensure the text updates correctly
    }
}

    // Countdown timer before starting the task
    IEnumerator CountdownBeforeStart()
    {
        SetArabicText(instructionText, isEnglish ? "Task starting in 5 seconds..." : "المهمة تبدأ خلال 5 ثوان...");

for (int i = 0; i < 5; i++)
{
    SetArabicText(instructionText, isEnglish ? $"Task starting in {5 - i} seconds..." : $"المهمة تبدأ خلال {5 - i} ثوان...");
    yield return new WaitForSeconds(1);  // Wait for 1 second
}

SetArabicText(instructionText, isEnglish ? "Task Started!" : "تم بدء المهمة!");


        // Start the game after countdown
        StartNewLevel();
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    void StartNewLevel()
    {
        
        if (!taskActive)
        return;

        // Spawn objects and start the game
        SpawnObjects();

        
    }

    public void StartTask(){
        audioSource = gameObject.AddComponent<AudioSource>();
        // Add listeners to the vertical and horizontal buttons
        verticalButton.onClick.AddListener(VerticalButtonClicked);
        horizontalButton.onClick.AddListener(HorizontalButtonClicked);
        AddArabicFixerToAllText();
        SetArabicText(scoreText, isEnglish ? "Score: " + score : "النتيجة: " + score);
        SetArabicText(horizontalButtonText, isEnglish ? "Horizontal"  : "افقي");
        SetArabicText(verticalButtonText, isEnglish ? "Vertical" : "عمودي" );


// Initialize the first question  
Question = 1;  // Reset question to 1  

SetArabicText(QuestionText, isEnglish ? $"Question: {Question}/{QuestionPerLevel}" : $"السؤال: {Question}/{QuestionPerLevel}");  

        StartCoroutine(CountdownBeforeStart());
    }

void DestroyButtonsAndObj(){
    // Destroy old buttons from previous round
        foreach (GameObject button in buttons)
        {
            if (button != null) Destroy(button);
        }

        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null) Destroy(obj);
        }
}
    void SpawnObjects()
    {
        if (!taskActive)
        return;
        // Destroy old buttons from previous round
        DestroyButtonsAndObj();

        // Spawn objects in the parent object with GridLayoutGroup
        for (int i = 0; i < spawnedObjects.Length; i++)
        {
            GameObject spawnedObject = Instantiate(objectPrefab, spawnPoint);
            spawnedObject.name = "Object " + i; // Set the name to "Object [index]"
            spawnedObjects[i] = spawnedObject; // Store reference to the spawned object
            targetRotation = Random.Range(0, 2) * 90f; // Randomly set 0 or 90 degrees
            targetObject = spawnedObjects[i];
            targetObject.transform.rotation = Quaternion.Euler(rotationAxis * targetRotation); // Apply the rotation
            // Apply transparency to all objects
            SetObjectTransparency(spawnedObject, transparencyValue);
        }

        // Randomly select the target object and set it to have full transparency
        int targetIndex = Random.Range(0, spawnedObjects.Length);
        targetObject = spawnedObjects[targetIndex];
        SetObjectTransparency(targetObject, 1f); // Full opacity for the target object

        // Randomly rotate the target object to either 0 or 90 degrees
        targetRotation = Random.Range(0, 2) * 90f; // Randomly set 0 or 90 degrees
        targetObject.transform.rotation = Quaternion.Euler(rotationAxis * targetRotation); // Apply the rotation

        // Show the spawned objects for 1 second, then hide them
        StartCoroutine(DisplayObjectsForSeconds(appearSeconds));
    }

    System.Collections.IEnumerator DisplayObjectsForSeconds(float seconds)
    {
        // Show objects for a set duration
        yield return new WaitForSeconds(seconds);

        // Hide the spawned objects after 1 second
        foreach (GameObject obj in spawnedObjects)
        {
            obj.SetActive(false);
        }

        // Show buttons after the objects are hidden
        ShowButtons();
    }

    void ShowButtons()
    {
        // Destroy old buttons from previous round
        foreach (GameObject button in buttons)
        {
            if (button != null) Destroy(button);
        }

        // Instantiate the buttons and place them under the UI canvas (or parent object)
        for (int i = 0; i < buttons.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab, taskObjects.transform);
            buttons[i] = button; // Store the button reference

            // Assign the correct button label
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            SetArabicText(buttonText, isEnglish ? " Here" : " هنا");

            // buttonText.text = "Here";

            // Assign button click listeners
            int index = i; // Capture index to avoid closure issue in the loop
            button.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(index));
        }

        SetArabicText(instructionText, isEnglish ? "Click the button representing the target object!" : "انقر على الزر الذي يمثل الكائن الهدف!");

    }

    void OnButtonClick(int buttonIndex)
    {
        if (!taskActive)
        return;
        if (spawnedObjects[buttonIndex] == targetObject)
        {
            // Correct button clicked, increase score
            IncreaseScore();
            SetArabicText(instructionText, isEnglish ? "Correct!" : "صحيح!");

            PlaySound(correctSound);

            // Deactivate task objects and activate QuestionObject
            taskObjects.SetActive(false);
            QuestionObject.SetActive(true);
        }
        else
        {
            // Incorrect button clicked
            SetArabicText(instructionText, isEnglish ? "Incorrect! Try again." : "خطأ! حاول مرة أخرى.");

            PlaySound(incorrectSound);
            ResetForNextRound();
        }
    }

    public void IncreaseScore()
    {
        score++;
        SetArabicText(scoreText, isEnglish ? "Score: " + score : "النقاط: " + score);

    }

    IEnumerator ResetForNextRoundCoroutine()
{
    taskObjects.SetActive(true);
    QuestionObject.SetActive(false);
    
    // Proceed to the next question
    Question++;
    if (Question <= QuestionPerLevel)
    {
        // instructionText.text = "Next question in 3 seconds...";
        yield return new WaitForSeconds(timeBetwenQ); // Wait before starting the next question

        // Display question number
       SetArabicText(QuestionText, isEnglish ? "Question: " + Question + "/" + QuestionPerLevel : "السؤال: " + Question + "/" + QuestionPerLevel);

        // Restart the process
        SpawnObjects();
    }
    else
    {
        // Game over or complete
       SetArabicText(instructionText, isEnglish ? "You've completed all questions!" : "لقد أكملت جميع الأسئلة!");
       DestroyButtonsAndObj();
       taskActive=false;
      trialEnd?.Invoke();

    }
}

// Call this method instead of directly resetting
void ResetForNextRound()
{
    StartCoroutine(ResetForNextRoundCoroutine());
}


    void SetObjectTransparency(GameObject obj, float alphaValue)
    {
        Transform imageChildTransform = obj.transform.Find("Image"); // Replace "image" with the actual child name
        if (imageChildTransform != null)
        {
            Debug.Log("Image found");
            Image childImage = imageChildTransform.GetComponent<Image>(); // Get Image component from the child
            if (childImage != null)
            {
                Debug.Log("Image Component found");
                Color color = childImage.color; // Get current color of the child image
                color.a = alphaValue; // Set the alpha (transparency)
                childImage.color = color; // Apply the new color to the child image
            }
        }
    }

    void VerticalButtonClicked()
    {
        if (!taskActive)
        return;
       if (targetRotation == 0f)
{
    IncreaseScore(); // Add point if the rotation is 0
    SetArabicText(instructionText, isEnglish ? "Correct!" : "صحيح!");
    PlaySound(correctSound);
}
else
{
    SetArabicText(instructionText, isEnglish ? "Incorrect! Try again." : "خطأ! حاول مرة أخرى.");
    PlaySound(incorrectSound);
}

        
        ResetForNextRound();
    }

    void HorizontalButtonClicked()
    {
        if (!taskActive)
        return;
        if (targetRotation == 90f)
{
    IncreaseScore(); // Add point if the rotation is 90
    SetArabicText(instructionText, isEnglish ? "Correct!" : "صحيح!");
    PlaySound(correctSound);
}
else
{
    SetArabicText(instructionText, isEnglish ? "Incorrect! Try again." : "خطأ! حاول مرة أخرى.");
    PlaySound(incorrectSound);
}

        
        ResetForNextRound();
    }
}
