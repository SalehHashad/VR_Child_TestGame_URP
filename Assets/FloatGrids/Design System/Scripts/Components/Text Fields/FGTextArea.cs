using UnityEngine.UI;

namespace floatgrids
{
    public class FGTextArea : FGTextInput
    {
        public override void OnSelect()
        {
            if (_textInput.interactable)
            {
                _textPlaceholder.color = _primaryTextColor;
                GetComponent<Image>().sprite = _selectedOutlineSprite;
            }
        }

        public override void OnDeselect()
        {
            if (_textInput.interactable)
            {
                _textPlaceholder.color = _secondaryTextColor;
                GetComponent<Image>().sprite = _normalOutlineSprite;
            }
        }

        public override void SetEnabledColors()
        {
            _text.color = _primaryTextColor;
            _textPlaceholder.color = _secondaryTextColor;
        }
        
        public override void SetDisabledColors()
        {
            _text.color = _disabledTextColor;
            _textPlaceholder.color = _disabledTextColor;
        }
    }
}
