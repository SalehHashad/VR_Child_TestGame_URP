using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace floatgrids
{
    [RequireComponent(typeof(ScrollRect))]
    public class FGScrollbar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private ScrollRect _scrollRect;
        Scrollbar _horizontalScrollBar;
        Scrollbar _verticalScrollBar;
        
        private void OnEnable()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _horizontalScrollBar = _scrollRect.horizontalScrollbar;
            _verticalScrollBar = _scrollRect.verticalScrollbar;

            ToggleScrollbars(false);
        }

        private void ToggleScrollbars(bool value)
        {
            _scrollRect.enabled = value;

            if (_horizontalScrollBar != null)
            {
                _horizontalScrollBar.gameObject.SetActive(value);
            }

            if (_verticalScrollBar != value)
            {
                _verticalScrollBar.gameObject.SetActive(value);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ToggleScrollbars(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ToggleScrollbars(false);
        }
    }
}
