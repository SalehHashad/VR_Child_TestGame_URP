using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class VisualAttentionTask : MonoBehaviour
{
    
    public GameObject resultUI; // Assign sphere prefab in the inspector
    public GameObject taskUI; // Assign sphere prefab in the inspector
    public GameObject spherePrefab; // Assign sphere prefab in the inspector
     public GameObject redBallPrefab; // Assign red ball prefab in the inspector
    public Transform spawnPoint; // Point where spheres will spawn
    public int startingObjects = 4; // Initial number of objects
    public float speed = 3f; // Speed of object movement
    public int level = 1; // Current level
    public int totalwrongError = -10; // Current level
    public int QuestionPerLevel = 20; // Current level
    public float roundTime = 10f; // Timer duration for each round

    public TMP_Text levelText; // TextMeshPro for Level
    public TMP_Text scoreText; // TextMeshPro for Score
    public TMP_Text questionText; // TextMeshPro for Current Question
    public TMP_Text instructionsText; // TextMeshPro for Instructions
    public TMP_Text timerText; // TextMeshPro for Timer

    private List<GameObject> objects = new List<GameObject>();
    private float currentTime;
    private bool roundActive = true;
    private int score = 0;
    private int Question = 0;

    private int correctTrackingStreak = 0; // To track consecutive correct answers
    private int correctRedObjectsCount = 0; // Count of correctly tracked red objects
    private int totalRedObjects = 0; // Total red objects for the round
private int consecutiveWrong = 0;
public int maxconsecutiveWrong = 4;
    private GameObject boundary;

    private void Start()
    {
        
        boundary = GameObject.Find("Boundary");
        UpdateUI();
        StartLevel(level);
        
    }
    public void SetLevelAndStartTask(int lvl)
    {
        // currentLevel = level-1; // Set the selected level
        // Debug.Log("Current Level: " + currentLevel); // Debugging purpose
        level=lvl;
  score=0;
  Question = 0;
  UpdateUI();
        // Call StartClick to begin the task
        StartLevel(lvl);
    }

    void StartLevel(int level)
    {
        
         instructionsText.text = " ";
        roundActive=true;
        SetLevelParameters(level);
        currentTime = roundTime;
        UpdateUI();
        taskUI.SetActive(true);
        resultUI.SetActive(false);
        boundary.SetActive(true);
      
       UpdateUI();
        ClearObjects();
        
        SpawnObjects();
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
        if (roundActive)
        {
            currentTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(currentTime);

            if (currentTime <= 0)
            {
                EndRound();
            }
        }
        Debug.Log("correctRedObjectsCount:"+correctRedObjectsCount);
        Debug.Log("totalRedObjects:"+totalRedObjects);
    }

    void SetLevelParameters(int level)
    {

        startingObjects = 4 + level; // Increase objects each level
        if(level==5)
        startingObjects=8;
        speed = 1f + level * 0.5f* Question; // Increase speed slightly each level
        currentTime=roundTime-level+1;
    }

    
     void SpawnObjects()
    {
        correctRedObjectsCount = 0; // Reset count for new round
        totalRedObjects = level; // Number of red objects based on level
        if(level==5)
        totalRedObjects=level-1;

        for (int i = 0; i < startingObjects; i++)
        {
            GameObject sphere;
            if (i < totalRedObjects)
            {
                sphere = Instantiate(redBallPrefab, spawnPoint.position, Quaternion.identity); // Spawn red ball prefab
            }
            else
            {
                sphere = Instantiate(spherePrefab, spawnPoint.position, Quaternion.identity); // Spawn regular sphere prefab
            }
            
            sphere.GetComponent<Rigidbody>().useGravity = false;
            objects.Add(sphere);

            // Attach the VisualAttentionTaskBall script
            // VisualAttentionTaskBall ballScript = sphere.AddComponent<VisualAttentionTaskBall>();
            if (i < totalRedObjects) // Mark red objects
            {
                HighlightRed(sphere); // Highlight red objects
            }

            // Add RandomMovement component and set its speed
            RandomMovement movement = sphere.AddComponent<RandomMovement>();
            movement.SetSpeed(speed);
        }
    }
    // Modified HighlightRed to use a coroutine
private void HighlightRed(GameObject ball)
{
    Renderer renderer = ball.GetComponent<Renderer>();
    if (renderer != null)
    {
        renderer.material.color = Color.red;
        StartCoroutine(RevertColorCoroutine(ball, 2f)); // Revert after 2 seconds
    }
}

// Coroutine to revert color after a delay
private IEnumerator RevertColorCoroutine(GameObject ball, float delay)
{
    yield return new WaitForSeconds(delay);

    if (ball != null) // Check if the GameObject still exists
    {
        Renderer renderer = ball.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white; // Change back to default color
        }
    }
}


    void EndRound()
    {
        roundActive = false;
        currentTime = 0;
        timerText.text = "Time: 0";

        // Stop all spheres and change their color to black
        foreach (GameObject sphere in objects)
        {
            Rigidbody rb = sphere.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero; // Stop movement
            }
            sphere.GetComponent<Renderer>().material.color = Color.black; // Change color
        }

        // Update instructions and deactivate boundary
        instructionsText.text = "Time's up! Click on the spheres to identify them.";
        // GameObject boundary = GameObject.Find("Boundary");
        if (boundary != null)
        {
            boundary.SetActive(false);
        }
    }

    void ClearObjects()
    {
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
        objects.Clear();
    }
    void LevelUp()
    {
        level++;
        Question = 0;
        StartLevel(level);
        instructionsText.text = "Congratulations! Level Up!";
    }
    void UpdateUI()
{
    levelText.text = $"Level: {level}";
    scoreText.text = $"Score: {score}";
    questionText.text = $"Question: {Question+1}/{QuestionPerLevel}";
    timerText.text = $"Time: {Mathf.Ceil(currentTime)}";

    if (Question >= QuestionPerLevel)
    {
        LevelUp();
    }
}


    // Function to handle ball clicks
    public void OnBallClicked(bool isRed)
{
    if (isRed)
    {
        correctRedObjectsCount++;
        score += 1; // Increment score
        instructionsText.text = "Correct! You clicked a red ball.";
    }
    else
    {
        score -= 1; // Decrement score
        instructionsText.text = "Oops! That wasn't a red ball.";
        consecutiveWrong++;
        
        correctTrackingStreak = 0;
        

        if (consecutiveWrong >= maxconsecutiveWrong)
        {
            instructionsText.text = $"Too many misses ({consecutiveWrong}/{maxconsecutiveWrong}). Resetting level.";
            consecutiveWrong=0;
            StartLevel(level);
        }
    }

    // Check if the task should end due to low score
    if (score <= totalwrongError)
    {
        scoreText.text = $"Score: {score}";
        StartCoroutine(EndTask());
        return;
    }

    CheckRoundTracking();
    UpdateUI();
}


private IEnumerator EndTask()
{
    roundActive = false;
    ClearObjects();
    instructionsText.text = $"Game Over! Your score dropped to {totalwrongError}.";
     yield return new WaitForSeconds(2);
    resultUI.SetActive(true);
    taskUI.SetActive(false);
    
    // Optionally disable further interactions or reset the game
}


    // Call this method when the round ends to check if the player tracked all red objects correctly
    public void CheckRoundTracking()
{
    if (correctRedObjectsCount == totalRedObjects)
    {
        correctTrackingStreak++;
        consecutiveWrong = 0; // Reset wrong attempts
        Question++;
        instructionsText.text = $"Great job! Streak: {correctTrackingStreak}";
        StartLevel(level);
    }
    else
    {
        
    }
}

}

public class RandomMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float speed;

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = GetRandomDirection() * speed;
    }

    void FixedUpdate()
    {
        if (rb != null && rb.velocity.magnitude > 0)
        {
            // Keep the speed consistent
            rb.velocity = rb.velocity.normalized * speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Reflect the velocity on collision for bounce effect
        Vector3 reflection = Vector3.Reflect(rb.velocity, collision.contacts[0].normal);
        rb.velocity = reflection.normalized * speed;
    }

    Vector3 GetRandomDirection()
    {
        // Generate a random direction
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
