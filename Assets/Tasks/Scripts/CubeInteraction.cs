using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CubeInteraction : MonoBehaviour
{
    private MemoryRecallGame gameScript;
    
    // Store the cube index to identify the sequence
    public int cubeIndex;

    // This method is called when the cube is initialized with the game logic
    public void Setup(MemoryRecallGame game, int index)
    {
        gameScript = game;
        cubeIndex = index;
    }

    // This method will be called when the cube is clicked/activated
    public void OnCubeActivated()
    {
        if (gameScript != null)
        {
            // Inform the MemoryRecallGame that this cube was clicked
            gameScript.OnCubeClicked(cubeIndex);
            Debug.Log("cubeIndex:"+cubeIndex);
        }
        Debug.Log("cubeIndex:"+cubeIndex);
    }
}
