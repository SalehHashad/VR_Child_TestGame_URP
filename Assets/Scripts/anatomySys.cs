using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anatomySys : MonoBehaviour
{
    public GameObject artial;
    public GameObject Fasciae;
    public GameObject Joints;
    public GameObject Lymphoid_organs;
    public GameObject Muscular_system;
    public GameObject systemSenseOrgans;
    public GameObject Regionsofhumanbody;
    public GameObject Skeletalsystem;
    public GameObject Venoussystem;
    public GameObject Visceralsystems;
    public GameObject upperLimp;
    public GameObject lowerLimp;

    public void artial_sys()
    {
        if (artial.activeInHierarchy == true)
            artial.SetActive(false);
        else
            artial.SetActive(true);
    }
    public void Fasciae_sys()
    {
        if (Fasciae.activeInHierarchy == true)
            Fasciae.SetActive(false);
        else
            Fasciae.SetActive(true);
    }
    public void Joints_sys()
    {
        if (Joints.activeInHierarchy == true)
            Joints.SetActive(false);
        else
            Joints.SetActive(true);
    }
    public void Lymphoid_organs_sys()
    {
        if (Lymphoid_organs.activeInHierarchy == true)
            Lymphoid_organs.SetActive(false);
        else
            Lymphoid_organs.SetActive(true);
    }
    public void Muscular_system_sys()
    {
        if (Muscular_system.activeInHierarchy == true)
            Muscular_system.SetActive(false);
        else
            Muscular_system.SetActive(true);
    }
    public void systemSenseOrgans_sys()
    {
        if (systemSenseOrgans.activeInHierarchy == true)
            systemSenseOrgans.SetActive(false);
        else
            systemSenseOrgans.SetActive(true);
    }
    public void Regionsofhumanbody_sys()
    {
        if (Regionsofhumanbody.activeInHierarchy == true)
            Regionsofhumanbody.SetActive(false);
        else
            Regionsofhumanbody.SetActive(true);
    }
    public void Skeletalsystem_sys()
    {
        if (Skeletalsystem.activeInHierarchy == true)
            Skeletalsystem.SetActive(false);
        else
            Skeletalsystem.SetActive(true);
    }
    public void Venoussystem_sys()
    {
        if (Venoussystem.activeInHierarchy == true)
            Venoussystem.SetActive(false);
        else
            Venoussystem.SetActive(true);
    }
    public void Visceralsystems_sys()
    {
        if (Visceralsystems.activeInHierarchy == true)
            Visceralsystems.SetActive(false);
        else
            Visceralsystems.SetActive(true);
    }
    public void upper()
    {
        if (upperLimp.activeInHierarchy == true)
            upperLimp.SetActive(false);
        else
            upperLimp.SetActive(true);
    }
    public void lower()
    {
        if (lowerLimp.activeInHierarchy == true)
            lowerLimp.SetActive(false);
        else
            lowerLimp.SetActive(true);
    }
}
