using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mySceneTranstion : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;
    //public GameObject options;
    //public GameObject about;

    //[Header("Main Menu Buttons")]
    //public Button startButton;
   // public Button optionButton;
    //public Button aboutButton;
//    public Button quitButton;

    //public List<Button> returnButtons;

    // Start is called before the first frame update
    void Start()
    {
        EnableMainMenu();

        //Hook events
        //startButton.onClick.AddListener(StartGame);
        //optionButton.onClick.AddListener(EnableOption);
        //aboutButton.onClick.AddListener(EnableAbout);
        //quitButton.onClick.AddListener(QuitGame);

        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        HideAll();
        SceneTransitionManager.singleton.GoToSceneAsync(1);
    }

    public void HideAll()
    {
        mainMenu.SetActive(false);
        //options.SetActive(false);
        //about.SetActive(false);
    }

    public void EnableMainMenu()
    {
        mainMenu.SetActive(true);
        //options.SetActive(false);
        //about.SetActive(false);
    }

}