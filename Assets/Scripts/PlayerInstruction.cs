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
    // Start is called before the first frame update
    void Start()
    {
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
}
