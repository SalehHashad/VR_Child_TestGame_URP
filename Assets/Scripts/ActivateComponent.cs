using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateComponent : MonoBehaviour
{
    private BoxCollider boxCollider;
    public GameObject menue;

    private void Start()
    {
        // Get the BoxCollider component attached to the game object
        boxCollider = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        if (boxCollider != null)
        {
            if (menue.activeInHierarchy == true)
                boxCollider.enabled = true;
            else
                boxCollider.enabled = false;
        }
        
    }
    
    public void ActivateCollider()
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }
    }

    public void DeactivateCollider()
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
    }
}
