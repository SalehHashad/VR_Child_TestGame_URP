using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class booleanValueSwitch : MonoBehaviour
{
    public bool Switch=false;
    private bool state=true;
    //public bool et=false;
    //public timerAgian timer;
    
  
    // Start is called before the first frame update
    public void Switcher()
    {
        if(state==true)
        {
            Switch=true;
            //et=true;
            state=false;
            //Debug.Log("switcher=1");
        }
        else
        {
          Switch=false;
          //et=false;
          state=true; 
          //Debug.Log("switcher=0"); 
        }

    }
    public void SwitcherTime()
    {
        if(state==true&&Switch==true)
        {
            Switch=true;
            state=false;
            //Debug.Log("switcher=1");
        }
        else
        {
          Switch=false;
          state=true; 
          //Debug.Log("switcher=0"); 
        }

    }

  
}
