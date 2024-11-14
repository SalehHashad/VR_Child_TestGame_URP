using UnityEngine;
using UnityEngine.UI; // For UI Image component
using TMPro; // For TextMeshPro

public class ObjectController : MonoBehaviour
{
    public GameObject[] objects = new GameObject[4];
    public GameObject taskObjects;
    public GameObject QuestionObject;

    [Range(0, 1)] public float transparencyValue = 0.5f;
    public Vector3 rotationAxis = Vector3.up; // Default to Y-axis

    private GameObject rotatingObject;
    private GameObject[] transparentObjects = new GameObject[3];

    private bool hasRotated = false;
    private int score = 0;
    private int Question = 1;
    public int QuestionPerLevel = 50;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI QuestionText;
    public TextMeshProUGUI instructionText; // Instruction text for each step

    void Start()
    {
        StartQuestion();
    }

    void StartQuestion()
{
    // Reset the rotation of all objects to the default orientation
    foreach (GameObject obj in objects)
    {
        obj.transform.rotation = Quaternion.identity; // Resets rotation to (0,0,0)
    }

    // Display the question and score
    QuestionText.text = "Question: " + Question + "/" + QuestionPerLevel;
    scoreText.text = "Score: " + score;

    // Set the initial instruction
    instructionText.text = "Choose the Clear Image";

    // Randomly select one object to be the rotating object
    int rotatingIndex = Random.Range(0, objects.Length);
    rotatingObject = objects[rotatingIndex];
    Debug.Log("Selected rotating object index: " + rotatingIndex + ", Object name: " + rotatingObject.name);

    // Populate the transparentObjects array with the other objects
    transparentObjects = new GameObject[3];
    int transparentIndex = 0;
    for (int i = 0; i < objects.Length; i++)
    {
        if (i != rotatingIndex)
        {
            transparentObjects[transparentIndex] = objects[i];
            transparentIndex++;
        }
    }

    // Apply transparency to the transparent objects
    foreach (GameObject obj in transparentObjects)
    {
        if (obj != null)
        {
            SetChildImageTransparency(obj, transparencyValue);
        }
        else
        {
            Debug.LogWarning("Encountered a null object in transparentObjects array.");
        }
    }

    // Reset rotation state for the new rotating object
    hasRotated = false;
}


    void Update()
    {
        // Rotate the selected rotating object if it hasn't already been rotated
        if (!hasRotated && rotatingObject != null)
        {
            rotatingObject.transform.Rotate(rotationAxis * 90f, Space.Self);
            hasRotated = true;
        }
    }

    void SetChildImageTransparency(GameObject parentObj, float alphaValue)
    {
        if (parentObj != null)
        {
            Image image = parentObj.GetComponentInChildren<Image>();
            if (image != null)
            {
                Color color = image.color;
                color.a = Mathf.Clamp01(alphaValue);
                image.color = color;
            }
        }
    }

    public bool CheckTransparency(GameObject obj)
    {
        Image image = obj.GetComponentInChildren<Image>();
        return image != null && Mathf.Approximately(image.color.a, 1);
    }

    public bool CheckRotation(GameObject obj)
    {
        float rotationZ = obj.transform.rotation.eulerAngles.z;
        return rotationZ > 0f;
    }

    public void OnVerButtonClick()
    {
        if (!CheckRotation(rotatingObject))
        {
            IncreaseScore();
            instructionText.text = "Correct! The object was rotated vertically. Proceed to the next question.";
        }
        else
        {
            instructionText.text = "Incorrect. The object was not rotated vertically. Proceed to the next question.";
        }
        FlipPhaseBack();
    }

    public void OnHorButtonClick()
    {
        if (CheckRotation(rotatingObject))
        {
            IncreaseScore();
            instructionText.text = "Correct! The object was rotated horizontally. Proceed to the next question.";
        }
        else
        {
            instructionText.text = "Incorrect. The object was not rotated horizontally. Proceed to the next question.";
        }
        FlipPhaseBack();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    public void FlipPhase()
    {
        taskObjects.SetActive(false);
        QuestionObject.SetActive(true);
        instructionText.text = "Decide if it is rotated horizontally or vertically";
    }

    public void FlipPhaseBack()
    {
        taskObjects.SetActive(true);
        QuestionObject.SetActive(false);
        Question++;

        // Restart StartQuestion with a new random rotating object
        StartQuestion();
    }
}
