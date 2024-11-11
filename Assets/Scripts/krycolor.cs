using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class krycolor : MonoBehaviour
{
    public Material ren;
    //private Renderer ren;
    void Start()
    {
        //ren= GetComponent<Renderer>();
    }

    // Update is called once per frame
    public void color()
    {
        ren.color= Color.red;
    }
}
