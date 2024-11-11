using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MaterialChangerFinal : MonoBehaviour
{
    public Toggle toggle;
    public Material[] targetMaterialArray;
    private Color[] originalColorArray;
    [SerializeField] private List<Color> colorCodeList= new List<Color>();
    
    
    void Start()
    {
      toggle.isOn =false; 
      originalColorArray= new Color[targetMaterialArray.Length];  
      saveOrginalColor();
      
    }
    public void saveOrginalColor()
    {
        if(targetMaterialArray.Length== originalColorArray.Length)
        {
            for(int i=0;i<targetMaterialArray.Length;i++)
            {
                if(targetMaterialArray[i] != null && targetMaterialArray[i].HasProperty("_Color"))
                originalColorArray[i]= targetMaterialArray[i].GetColor("_Color");
            }
        }
    }

    private void OnApplicationQuit()
    {
        if( originalColorArray.Length == targetMaterialArray.Length )
        {
            for(int i=0;i<originalColorArray.Length;i++)
            {
                targetMaterialArray[i].color=originalColorArray[i];
            }
        }
        
    }

    public void ChangeMaterialColorAtListOfColor()
    {   
        
        if(toggle.isOn==true)
        {
            if( targetMaterialArray.Length == colorCodeList.Count )
            {
                for(int i=0;i<targetMaterialArray.Length;i++)
                {
                    if(targetMaterialArray[i] != null && targetMaterialArray[i].HasProperty("_Color"))
                    targetMaterialArray[i].SetColor("_Color",colorCodeList[i]);
                } 
            }
           
        }
        else
        {
             if( originalColorArray.Length == targetMaterialArray.Length )
                {
                    for(int i=0;i<originalColorArray.Length;i++)
                    {
                        targetMaterialArray[i].color=originalColorArray[i];
                    }
                }
            
            
        }
    }
}