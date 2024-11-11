using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonBehiver : MonoBehaviour
{
    int n;
    public void OnMouseUpAsButton()
    {
        n++;
        Debug.Log("mmmmmmmmm");
    }
    public void OnMouseUpAsButton2()
    {
        n++;
        Debug.Log("eyad");
    }
    public void sceneChanger()
    {
        SceneManager.LoadScene("anatomy");
    }
    public void sceneChanger2()
    {
        SceneManager.LoadScene("home");
    }
    public void quitgame()
    {
        Application.Quit();
    }
}
