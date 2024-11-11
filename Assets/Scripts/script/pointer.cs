using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointer : MonoBehaviour
{
  /*  public float m_DefultLength = 5.0f;
    public GameObject m_dot;
    public VRInputModule m_inputmodule;

    private LineRenderer m_linrenderer = null; 
    
          private void Awake()
    {
           m_linrenderer = GetComponent<LineRenderer>();
    }


    private void Update()

    {
       UpdateLine();
    }
    private void UpdateLine()
    {
    // ues default or distance
    float targetlength = m_DefultLength;
;

    // raycast
    RaycastHit hit = CreateRaycast(targetlength);

    // default
    Vector3 endPosition = transform.position + (transform.forward * targetlength);

    // or based on hit
    if (hit.collider != null)
        endPosition = hit.point;

    // set position of the dot 
    m_dot.transform.position = endPosition;

        // set linerenderer
        m_linrenderer.SetPosition(0, transform.position);
        m_linrenderer.SetPosition(1, endPosition);
    }

    private RaycastHit CreateRaycast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, m_DefultLength );


    }*/
}
