using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class VisualAttentionTask : MonoBehaviour
{
    public GameObject spherePrefab; // Assign sphere prefab in the inspector
    public Transform spawnPoint; // Point where spheres will spawn
    public int startingObjects = 5; // Initial number of objects
    public float speed = 3f; // Speed of object movement
    public int level = 1; // Current level
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

    private int correctTrackingStreak = 0; // To track consecutive correct answers
    private int correctRedObjectsCount = 0; // Count of correctly tracked red objects
    private int totalRedObjects = 0; // Total red objects for the round

    private void Start()
    {
        currentTime = roundTime;
        UpdateUI();
        StartLevel(level);
    }

    void StartLevel(int level)
    {
        ClearObjects();
        SetLevelParameters(level);
        SpawnObjects();
        UpdateUI();
    }

    void Update()
    {
        if (roundActive)
        {
            currentTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.Ceil(currentTime);

            if (currentTime <= 0)
            {
                EndRound();
            }
        }
    }

    void SetLevelParameters(int level)
    {
        startingObjects = 5 + level; // Increase objects each level
        speed = 3f + level * 0.5f; // Increase speed slightly each level
    }

    void SpawnObjects()
    {
        correctRedObjectsCount = 0; // Reset count for new round
        totalRedObjects = level; // Number of red objects based on level

        for (int i = 0; i < startingObjects; i++)
        {
            GameObject sphere = Instantiate(spherePrefab, spawnPoint.position, Quaternion.identity);
            sphere.GetComponent<Rigidbody>().useGravity = false;
            objects.Add(sphere);

            // Attach the VisualAttentionTaskBall script
            VisualAttentionTaskBall ballScript = sphere.AddComponent<VisualAttentionTaskBall>();
            if (i < totalRedObjects) // Increase red objects based on level
            {
                ballScript.HighlightRed(); // Highlight red objects
            }

            // Add RandomMovement component and set its speed
            RandomMovement movement = sphere.AddComponent<RandomMovement>();
            movement.SetSpeed(speed);
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
        GameObject boundary = GameObject.Find("Boundary");
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

    void UpdateUI()
    {
        levelText.text = "Level: " + level;
        scoreText.text = "Score: " + score;
        questionText.text = "Question: 1"; // Placeholder, update as needed
        instructionsText.text = "Track the red objects and remember their positions!";
    }

    // Function to handle ball clicks
    public void OnBallClicked(bool isRed)
    {
        if (isRed)
        {
            // Correctly identified red object
            correctRedObjectsCount++;
            score += 1; // Earn 1 point for correctly identified red object
            instructionsText.text = "Correct! You clicked a red ball.";
        }
        else
        {
            // Incorrectly identified object
            score -= 1; // Deduct 1 point for an incorrect identification
            instructionsText.text = "Oops! That wasn't a red ball.";
        }

        // If all red objects were correctly identified, check for consecutive tracking
        if (correctRedObjectsCount == totalRedObjects)
        {
            correctTrackingStreak++;
            score += correctTrackingStreak; // Bonus points for consecutive correct answers
            instructionsText.text = "Well done! All red objects tracked correctly. Streak: " + correctTrackingStreak;
        }

        // Update the score UI
        UpdateUI();
    }

    // Call this method when the round ends to check if the player tracked all red objects correctly
    public void CheckRoundTracking()
    {
        if (correctRedObjectsCount == totalRedObjects)
        {
            correctTrackingStreak++;
            score += correctTrackingStreak; // Bonus for consecutive correct tracking
            instructionsText.text = "Correct tracking! Streak: " + correctTrackingStreak;
        }
        else
        {
            instructionsText.text = "You missed some red objects.";
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
