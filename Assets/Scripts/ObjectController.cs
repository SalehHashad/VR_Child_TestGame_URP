using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using System.Collections; 
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
    public float appearSeconds = 50;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI QuestionText;
    public TextMeshProUGUI instructionText; // Instruction text for each step

    private GameObject[] buttons = new GameObject[4]; // Array to hold button references
    private GameObject targetObject; // The target object to find in the buttons

    // Add two buttons for vertical and horizontal actions
    public Button verticalButton;
    public Button horizontalButton;

    private float targetRotation = 0f; // Track the rotation of the target object (0 or 90)
[Header("Audio Feedback")]
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip incorrectSound;
    private AudioSource audioSource;

    // Countdown timer before starting the task
    IEnumerator CountdownBeforeStart()
    {
        instructionText.text = "Task starting in 5 seconds...";
        
        for (int i = 0; i < 5; i++)
        {
            instructionText.text = $"Task starting in {5-i} seconds...";
            yield return new WaitForSeconds(1);  // Wait for 1 seconds
        }
        
        
        instructionText.text = "Task Started!";

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
        
        

        // Spawn objects and start the game
        SpawnObjects();

        
    }

    void Start(){
        audioSource = gameObject.AddComponent<AudioSource>();
        // Add listeners to the vertical and horizontal buttons
        verticalButton.onClick.AddListener(VerticalButtonClicked);
        horizontalButton.onClick.AddListener(HorizontalButtonClicked);
        scoreText.text = "Score: " + score;
        // Initialize the first question
        Question = 1;  // Reset question to 1
        QuestionText.text = "Question: " + Question + "/" + QuestionPerLevel; // Update the question text
        StartCoroutine(CountdownBeforeStart());
    }

    void SpawnObjects()
    {
        // Destroy old buttons from previous round
        foreach (GameObject button in buttons)
        {
            if (button != null) Destroy(button);
        }

        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null) Destroy(obj);
        }

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
            buttonText.text = "Here";

            // Assign button click listeners
            int index = i; // Capture index to avoid closure issue in the loop
            button.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(index));
        }

        instructionText.text = "Click the button representing the target object!";
    }

    void OnButtonClick(int buttonIndex)
    {
        if (spawnedObjects[buttonIndex] == targetObject)
        {
            // Correct button clicked, increase score
            IncreaseScore();
            instructionText.text = "Correct!";
            PlaySound(correctSound);

            // Deactivate task objects and activate QuestionObject
            taskObjects.SetActive(false);
            QuestionObject.SetActive(true);
        }
        else
        {
            // Incorrect button clicked
            instructionText.text = "Incorrect! Try again.";
            PlaySound(incorrectSound);
            ResetForNextRound();
        }
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    void ResetForNextRound()
    {
        taskObjects.SetActive(true);
        QuestionObject.SetActive(false);
        // Proceed to the next question
        Question++;
        if (Question <= QuestionPerLevel)
        {
            // Display question number
            QuestionText.text = "Question: " + Question + "/" + QuestionPerLevel;

            // Restart the process
            SpawnObjects();
        }
        else
        {
            // Game over or complete
            instructionText.text = "You've completed all questions!";
        }
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
        if (targetRotation == 0f)
        {
            IncreaseScore(); // Add point if the rotation is 0
            instructionText.text = "Correct!";
            PlaySound(correctSound);
        }
        else
        {
            instructionText.text = "Incorrect! Try again.";
            PlaySound(incorrectSound);
        }
        
        ResetForNextRound();
    }

    void HorizontalButtonClicked()
    {
        if (targetRotation == 90f)
        {
            IncreaseScore(); // Add point if the rotation is 90
            instructionText.text = "Correct!";
            PlaySound(correctSound);
        }
        else
        {
            instructionText.text = "Incorrect! Try again.";
            PlaySound(incorrectSound);
        }
        
        ResetForNextRound();
    }
}
