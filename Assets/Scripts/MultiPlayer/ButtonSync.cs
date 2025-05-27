using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class ButtonSync : MonoBehaviourPun
{
    [System.Serializable]
    public class ButtonData
    {
        public string name;
        public Button button;
        public bool clicked = false;
    }
    [SerializeField] ButtonData[] buttons;
    private List<UnityAction> clickListeners = new List<UnityAction>();
    // Start is called before the first frame update
    void Start()
    {
        
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    void PressButtons(int index)
    {
        photonView.RPC("OnClickRemote", RpcTarget.OthersBuffered, index);
    }
    [PunRPC]
    void OnClickRemote(int index)
    {
      StartCoroutine(PressButtonsAsyc(index));
    }

    IEnumerator PressButtonsAsyc(int index)
    {
        //chaneg the color of the button during the runtime
        if(buttons[index] != null) 
        {
            ColorBlock colorBlock = buttons[index].button.colors;
            colorBlock.normalColor = colorBlock.pressedColor;
            buttons[index].button.colors = colorBlock;
            yield return new WaitForSeconds(0.5f);
            buttons[index].button.onClick.Invoke();
            Debug.Log("pressed successfull");
        }
        
    }

    public void MakeButtonsAsyc()
    {
        if (photonView.IsMine && !PhotonNetwork.OfflineMode)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                int index = i;
                UnityAction action = () => PressButtons(index);
                buttons[i].button.onClick.AddListener(action);
               clickListeners.Add(action);
            }

        }
    }

    public void MakeButtonsLocal()
    {


        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < clickListeners.Count)
            {
                buttons[i].button.onClick.RemoveListener(clickListeners[i]);
            }
        }
        clickListeners.Clear();

    }
}
