using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIsystemtype : MonoBehaviour
{
    public GameObject generalsys;
    public GameObject ustsys;
    public void General()
    {
        
            generalsys.SetActive(true);
            ustsys.SetActive(false);

    }
    public void ust()
    {

        generalsys.SetActive(false);
        ustsys.SetActive(true);

    }
}

