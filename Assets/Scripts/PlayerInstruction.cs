using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstruction : MonoBehaviour
{
    public GameObject task;
    public float timeToStart;
    public GameObject instructionCanvus;

    public AudioSource instructionAudio;
    public AudioClip arabicClip, englishClip;

    [SerializeField]
    private List<GameObject> englishInstruction = new List<GameObject>(),
                             arabicInstruction = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < arabicInstruction.Count; i++)
        {
            arabicInstruction[i].SetActive(false);
        }

        for (int i = 0; i < englishInstruction.Count; i++)
        {
            englishInstruction[i].SetActive(false);
        }
        //Invoke("StartTask", timeToStart);
    }
    private void StartTask()
    {
        task.SetActive(true);


    }
    
    public void SetAudioClip(string clipName)
    {
        if(clipName == "en")
        {
            instructionAudio.PlayOneShot(englishClip);
        }
        else if (clipName == "ar")
        {
            instructionAudio.PlayOneShot(arabicClip);
        }
        instructionCanvus.SetActive(false);
        Invoke("StartTask", timeToStart);
    }

    public void Hide_Unsed_Text(string language)
    {
        if(language == "en")
        {
            for (int i = 0;  i < englishInstruction.Count; i++)
            {
                englishInstruction[i].SetActive(true);
            }
        }
        else if (language =="ar")
        {
            for (int i = 0; i < arabicInstruction.Count; i++)
            {
                arabicInstruction[i].SetActive(true);
            }

        }
    }
}
