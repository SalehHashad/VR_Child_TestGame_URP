using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    
    [SerializeField] public booleanValueSwitch script;
    
    void start()
    {
        
    }
   
    void Update()
    {
    
      if(script.Switch)
      {
          //Debug.Log("SWitcer on");
          //gameObject.SetActive(false);
      }
        else
       {
           //Debug.Log("SWitcer off");
           
        } 

     

    }
 
  
}
