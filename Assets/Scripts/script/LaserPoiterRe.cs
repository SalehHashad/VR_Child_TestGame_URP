using UnityEngine;
using UnityEngine.EventSystems;

public class LaserPoiterRe : MonoBehaviour
{
    //public GameObject rt;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(eventData.pointerCurrentRaycast.gameObject.CompareTag("Button"))
        {
            //rt.SetActive(false);
        }
    }
    
}
