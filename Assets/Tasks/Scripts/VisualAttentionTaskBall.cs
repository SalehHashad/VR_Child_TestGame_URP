using UnityEngine;

public class VisualAttentionTaskBall : MonoBehaviour
{
    private Material ballMaterial;
    private Color originalColor;
    private bool isRed; // Track if the ball is currently red
    private VisualAttentionTask taskManager; // Reference to the main task manager

    public Color hoverColor = Color.yellow; // Color when hovered over
    

    // void Start()
    // {
    //     // Find the Renderer component and store the material
    //     Renderer ballRenderer = GetComponent<Renderer>() ?? GetComponentInChildren<Renderer>();
        
    //     if (ballRenderer != null)
    //     {
    //         ballMaterial = ballRenderer.material; // Assign the material
    //         originalColor = ballMaterial.color; // Store the original color
    //     }
    //     else
    //     {
    //         Debug.LogError("Renderer component not found on " + gameObject.name + " or its children.");
    //     }
    // }
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
        if (ballMaterial != null)
        {
            ballMaterial.color = Color.black; // Revert to original color
        }
    }

    // Method to highlight the ball temporarily as red
    public void HighlightRed()
    {
        if (ballMaterial != null)
        {
            ballMaterial.color = Color.red;
            // isRed = true;
            Invoke("RevertColor", 2f); // Revert after 2 seconds
        }
    }

    // Method to revert to the original color
    private void RevertColor()
    {
        if (originalColor == Color.red)
        {
            ballMaterial.color = originalColor;
            isRed = true;
        }
        else{
            isRed = false;
            ballMaterial.color = originalColor;
        }
    }

    public void OnClicked()
    {
        if (taskManager != null)
        {
            // Check if the ball has the "Red" tag
            if (CompareTag("Red"))
            {
                taskManager.OnBallClicked(true); // Send true if it's a red ball
            }
            else
            {
                taskManager.OnBallClicked(false); // Send false if it's a regular ball
            }
        }
    }
}
