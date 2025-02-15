using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using UnityEngine.Events;

public class PlayerInstruction : MonoBehaviour
{
    public GameObject task;
    public float timeToStart;
    public GameObject instructionCanvus;

    public AudioSource instructionAudio;
    public AudioClip arabicClip, englishClip;

[Header("Audio Events")]
    public UnityEvent OnAudioFinished;

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
        // OnTaskStart?.Invoke(); // Event is triggered here

    }
    
    
    // public void SetAudioClip(string clipName)
    // {
    //     if(clipName == "en")
    //     {
    //         instructionAudio.PlayOneShot(englishClip);
    //     }
    //     else if (clipName == "ar")
    //     {
    //         instructionAudio.PlayOneShot(arabicClip);
    //     }
    //     instructionCanvus.SetActive(false);
    //     Invoke("StartTask", timeToStart);
    // }
    public void SetAudioClip(string clipName)
    {
        if (clipName == "en")
        {
            instructionAudio.clip = englishClip;
        }
        else if (clipName == "ar")
        {
            instructionAudio.clip = arabicClip;
        }

        instructionCanvus.SetActive(false);
        instructionAudio.Play();

        // Start coroutine to wait until the audio ends
        StartCoroutine(WaitForAudioToEnd());
    }

    private IEnumerator WaitForAudioToEnd()
    {
        yield return new WaitUntil(() => !instructionAudio.isPlaying);

        // Invoke the UnityEvent when the audio finishes
        OnAudioFinished?.Invoke();

        StartTask();
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

