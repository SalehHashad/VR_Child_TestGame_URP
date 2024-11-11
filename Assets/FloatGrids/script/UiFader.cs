using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiFader : MonoBehaviour
{
   [SerializeField] private CanvasGroup UIGRub;
   [SerializeField] private GameObject UIGRubGameobject;
   [SerializeField] private bool fadein=false;
   [SerializeField] private bool fadeout=false;

   public void showUI()
   {
       fadein=true;
   }
   public void hideUI()
   {
       fadeout=true;
   }
    void Update()
    {
        if(fadein)
        {
            UIGRubGameobject.SetActive(true);
            if(UIGRub.alpha <1)
            {
                
                UIGRub.alpha += Time.deltaTime;
                if(UIGRub.alpha >=1)
                {fadein=false;
                
                }
            }
        }
        if(fadeout)
        {
            if(UIGRub.alpha >=0)
            {
                UIGRub.alpha -= Time.deltaTime;
                if(UIGRub.alpha ==0)
                {
                fadeout=false;
                UIGRubGameobject.SetActive(false);
                }
            }
        }
    }
}
