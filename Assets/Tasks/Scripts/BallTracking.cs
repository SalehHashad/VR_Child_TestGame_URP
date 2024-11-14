using UnityEngine;
using TMPro; // Import TextMeshPro namespace
using System.Collections;
using System.Collections.Generic;

public class BallTracking : MonoBehaviour
{
    public GameObject ballPrefab; // Prefab for the ball
    public int currentLevel = 1; // Current level of the game
    public int totalQuestions = 20; // Total questions to ask per level
    public float movementSpeed = 1f; // Speed of ball movement
    public float spawnOffset = 2f; // Forward offset for ball spawn (Z-axis)
    public Vector3 movementThreshold = new Vector3(10f, 5f, 5f); // Movement bounds for X, Y, Z axes
    public TextMeshProUGUI scoreText; // TextMeshProUGUI for displaying score
    public TextMeshProUGUI levelText; // TextMeshProUGUI for displaying current level
    public TextMeshProUGUI questionText; // TextMeshProUGUI for the question prompt

    private int score = 0; // Player's score
    private int consecutiveCorrect = 0; // Count of consecutive correct answers
    private List<GameObject> balls = new List<GameObject>(); // List to hold ball instances
    private int totalBalls; // Total number of balls for the current level
    private int redBalls; // Number of red balls for the current level

    void Start()
    {
        StartLevel();
    }

    void StartLevel()
    {
        // Set number of balls and red balls based on the current level
        totalBalls = 5 + (currentLevel - 1); // Total balls increase with each level
        redBalls = Mathf.Clamp(currentLevel, 1, 5); // Max 5 red balls

        // Clear existing balls if any
        foreach (var ball in balls)
        {
            Destroy(ball);
        }
        balls.Clear();

        // Spawn balls
        SpawnBalls();
        StartCoroutine(MoveBalls());
        questionText.text = "Remember the red balls!"; // Set question prompt
    }

    void SpawnBalls()
    {
        for (int i = 0; i < totalBalls; i++)
        {
            // Apply a forward offset (Z axis) for spawn position
            float spawnX = Random.Range(-movementThreshold.x, movementThreshold.x);
            float spawnY = Random.Range(-movementThreshold.y, movementThreshold.y);
            float spawnZ = Random.Range(-movementThreshold.z, movementThreshold.z) + spawnOffset;

            Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);
            GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            balls.Add(ball);

            // Randomly set the ball color
            if (i < redBalls)
            {
                ball.GetComponent<Renderer>().material.color = Color.red; // Make red balls
                ball.tag = "Red"; // Tag red balls for identification
            }
            else
            {
                ball.GetComponent<Renderer>().material.color = Color.blue; // Set non-red balls to blue
            }
        }
    }

    IEnumerator MoveBalls()
    {
        while (true)
        {
            foreach (var ball in balls)
            {
                // Add random movement in all three axes (X, Y, Z)
                Vector3 randomMovement = new Vector3(Random.Range(-1f, 51f), Random.Range(-1f, 51f), Random.Range(-5f, 1f)).normalized;
                ball.transform.position += randomMovement * movementSpeed * Time.deltaTime;

                // Apply threshold movement limits to each axis (X, Y, Z)
                Vector3 pos = ball.transform.position;
                pos.x = Mathf.Clamp(pos.x, -movementThreshold.x, movementThreshold.x);
                pos.y = Mathf.Clamp(pos.y, -movementThreshold.y, movementThreshold.y);
                pos.z = Mathf.Clamp(pos.z, -movementThreshold.z + spawnOffset, movementThreshold.z + spawnOffset);
                ball.transform.position = pos;
            }
            yield return null; // Wait for the next frame
        }
    }

    public void CheckTracking(List<GameObject> identifiedBalls)
    {
        int correctCount = 0;
        int mistakeCount = 0;

        // Check the identified balls for correctness
        foreach (GameObject ball in identifiedBalls)
        {
            if (ball.CompareTag("Red")) // Correct identification
            {
                correctCount++;
            }
            else // Mistake
            {
                mistakeCount++;
            }
        }

        // Update score based on correct tracking and mistakes
        score += correctCount;
        score -= mistakeCount; // Deduct points for mistakes

        // Handle consecutive answers
        if (correctCount > 0)
        {
            consecutiveCorrect++;
            score += consecutiveCorrect; // Bonus for consecutive correct answers
        }
        else
        {
            consecutiveCorrect = 0; // Reset if a mistake is made
        }

        // Update score display
        scoreText.text = "Score: " + score;

        // Check if level completed
        if (totalQuestions <= 0)
        {
            EndLevel();
        }
    }

    void EndLevel()
    {
        // Level completed, you can add any end level logic here
        currentLevel++; // Move to the next level
        StartLevel(); // Restart the level
    }

    public void AnswerQuestion(List<GameObject> selectedBalls)
    {
        CheckTracking(selectedBalls); // Check the player's answer
        totalQuestions--; // Decrease question count
    }
}
