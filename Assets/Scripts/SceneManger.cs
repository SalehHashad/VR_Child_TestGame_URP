using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{
    public void Scene_Loader(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
