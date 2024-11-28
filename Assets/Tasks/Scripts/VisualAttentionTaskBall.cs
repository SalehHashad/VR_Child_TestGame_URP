using UnityEngine;

public class VisualAttentionTaskBall : MonoBehaviour
{
    private Material ballMaterial;
    private Color originalColor;
    private bool isRed; // Track if the ball is currently red
    private bool hasBeenClicked = false; // Track if the ball has been clicked
    private VisualAttentionTask taskManager; // Reference to the main task manager

    public Color hoverColor = Color.yellow; // Color when hovered over

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            ballMaterial = renderer.material;
            originalColor = ballMaterial.color;
        }

        // Get reference to VisualAttentionTask script
        taskManager = FindObjectOfType<VisualAttentionTask>();
    }
    

    // Method to be called on hover
    public void OnHoverEnter()
    {
        if (ballMaterial != null)
        {
            ballMaterial.color = hoverColor; // Change to hover color
        }
    }

    // Method to be called on unhover
    public void OnHoverExit()
    {
        if (ballMaterial != null && !hasBeenClicked)
        {
            ballMaterial.color = Color.black; // Revert to original color
        }
        else{
            RevertColor();
        }
    }

    // Method to highlight the ball temporarily as red
    public void HighlightRed()
    {
        if (ballMaterial != null)
        {
            ballMaterial.color = Color.red;
            isRed = true;
            Invoke("RevertColor", 2f); // Revert after 2 seconds
        }
    }

    // Method to revert to the original color
    private void RevertColor()
    {
        isRed = false;
        if (ballMaterial != null)
        {
            ballMaterial.color = originalColor;
        }
    }

    public void OnClicked()
    {
        if (hasBeenClicked)
        {
            Debug.Log("This ball has already been clicked.");
            return; // Prevent multiple scoring from the same ball
        }

        if (taskManager != null)
        {
            // Check if the ball has the "Red" tag
            if (CompareTag("Red"))
            {
                taskManager.OnBallClicked(true); // Send true if it's a red ball
                //  RevertColor();
            }
            else
            {
                taskManager.OnBallClicked(false); // Send false if it's a regular ball
            }
        }
         RevertColor();

        hasBeenClicked = true; // Mark the ball as clicked
    }
}
