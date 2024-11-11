using UnityEngine;
using UnityEngine.UI;

public class MaterialChanger : MonoBehaviour
{
    //public Toggle colorToggle;
    public Material targetMaterial; // The material to change the color of
    public Material targetMaterial2;
    public Material targetMaterial3;
    public Material targetMaterial4;
    public Material targetMaterial5;
    public Material targetMaterial6;
    public Material targetMaterial7;
    public Material targetMaterial8;
    public Material targetMaterial9;
    //public Material targetMaterial10;
    //public Material targetMaterial11;
    //public Material targetMaterial12;
    //public Material targetMaterial13;
    private Color originalColor; // The original color of the material
    private Color originalColor2;
    private Color originalColor3;
    private Color originalColor4;
    private Color originalColor5;
    private Color originalColor6;
    private Color originalColor7;
    private Color originalColor8;
    private Color originalColor9;
    private bool isMaterialChanged = false; // Whether the material has been changed or not

    void Start()
    {
        //colorToggle.isOn = true;
        // Store the original color of the material
        originalColor = targetMaterial.color;
        originalColor2 = targetMaterial2.color;
        originalColor3 = targetMaterial3.color;
        originalColor4 = targetMaterial4.color;
        originalColor5 = targetMaterial5.color;
        originalColor6 = targetMaterial6.color;
        originalColor7 = targetMaterial7.color;
        originalColor8 = targetMaterial8.color;
        originalColor9 = targetMaterial9.color;
    }

    private void OnApplicationQuit()
    {
        targetMaterial.color  =originalColor;
        targetMaterial2.color = originalColor2;
        targetMaterial3.color = originalColor3;
        targetMaterial4.color = originalColor4;
        targetMaterial5.color = originalColor5;
        targetMaterial6.color = originalColor6;
        targetMaterial7.color = originalColor7;
        targetMaterial8.color = originalColor8;
        targetMaterial9.color = originalColor9;
    }
    public void ChangeMaterialColor()
    {
        if (isMaterialChanged)
        {
            // Revert back to the original color
            targetMaterial.color = originalColor;
            targetMaterial2.color = originalColor2;
            targetMaterial3.color = originalColor3;
            targetMaterial4.color = originalColor4;
            targetMaterial5.color = originalColor5;
            targetMaterial6.color = originalColor6;
            targetMaterial7.color = originalColor7;
            targetMaterial8.color = originalColor8;
            targetMaterial9.color = originalColor9;
            isMaterialChanged = false;
        }
        else
        {
            // Change the material color to red
            targetMaterial.color = new Color(1f, 0f, 0.6551642f);//pink
            targetMaterial2.color = new Color(1f, 0.4895964f, 0f);//orange
            targetMaterial3.color = new Color(0.1433821f, 1f, 0f);//green
            targetMaterial4.color = new Color(1f, 0.4895964f, 0f);//orange
            targetMaterial5.color = new Color(1f, 0.9733431f, 0f);//yellow
            targetMaterial6.color = new Color(0f, 0.7648076f, 1f);//smaoy
            targetMaterial7.color = new Color(0.6185346f, 0f, 1f);//vilot
            targetMaterial8.color = new Color(0.6185346f, 0f, 1f);//vilot
            targetMaterial9.color = new Color(0.4834641f, 0f, 0.6037736f);//dark vilot
            //targetMaterial10.color = new Color(0.6320754f, 0.282645f, 0.3078213f);

            isMaterialChanged = true;
        }
    }
}