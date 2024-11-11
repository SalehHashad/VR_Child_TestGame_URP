using System.Linq;
using UnityEngine;

public class MaterialChanger2: MonoBehaviour
{
    public Material targetMaterial; // The target material that we want to change the color of
    public Color newColor; // The new color that we want to change the material to
    private Color originalColor; // The original color of the target material
    private Renderer[] objectsWithMaterial; // An array to store all the objects that use the target material

    void Start()
    {
        // Save the original color of the target material
        originalColor = targetMaterial.color;

        // Get all the objects that use the target material and store them in the array
        objectsWithMaterial = FindObjectsOfType<Renderer>().Where(x => x.material == targetMaterial).ToArray();
    }

    public void ChangeMaterialColor()
    {
        if (targetMaterial.color == originalColor)
        {
            // Change the material color to the specified color for all objects that use the target material
            foreach (Renderer renderer in objectsWithMaterial)
            {
                renderer.material.color = newColor;
            }
        }
        else
        {
            // Revert back to the original color for all objects that use the target material
            foreach (Renderer renderer in objectsWithMaterial)
            {
                renderer.material.color = originalColor;
            }
        }
    }
}