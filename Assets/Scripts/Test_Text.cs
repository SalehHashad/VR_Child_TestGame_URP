using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Test_Text : MonoBehaviour
{
    public TextMeshPro textMeshPro;

    public void Change(int  s)
    {
        if (s == 0 )
        {
            textMeshPro.text = "English";
        }

        else if ( s == 1 )
        {
            textMeshPro.text = "ﻲﺑﺮﻋ";
        }
    }
}
