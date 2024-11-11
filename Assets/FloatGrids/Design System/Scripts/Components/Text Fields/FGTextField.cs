
using UnityEngine;
using UnityEngine.UI;

namespace floatgrids
{
    public class FGTextField : FGTextInput
    {
        private Color _errorColor;
        private bool _isTextValid;
       
        [HideInInspector]
        [SerializeField]
        private Image _icon;

        public bool IsTextValid { 
            get 
            { 
                return _isTextValid; 
            } 
        }

        public new void Start()
        {
            base.Start();
            Validate();
        }

        public override void Setup()
        {
            base.Setup();

            Image[] images = GetComponentsInChildren<Image>(true);
            
            foreach (Image image in images)
            {
                if (image.name.Equals("Icon"))
                {
                    _icon = image;
                }
            }
            
            _icon.color = _secondaryTextColor;

            _errorColor = ColorPaletteManager.GetColorByTokenName("Error", "Main");

            _errorColorBlock = new ColorBlock();
            _errorColorBlock.normalColor = _errorColor;
            _errorColorBlock.highlightedColor = _errorColor;
            _errorColorBlock.selectedColor = _errorColor;
            _errorColorBlock.pressedColor = _pressedColor;
            _errorColorBlock.disabledColor = _disabledColor;

            _errorColorBlock.colorMultiplier = _colorMultiplier;
            _errorColorBlock.fadeDuration = _fadeDuration;

            Validate();
        }

        public void Validate()
        {
            // Put your validation code here;
            // Set a value to _isTextValid and call SetTranstionColorBlock()

            string text = _text.text.ToString();

            if (text.Length > 1 && (text.Contains("Float") || text.Equals("Float")))
            {
                _isTextValid = false;
            }
            else
            {
                _isTextValid = true;
            }

            SetTransitionColorBlock(); 
        }

        private void SetTransitionColorBlock()
        {
            if (!_isTextValid)
            {
                _textInput.colors = _errorColorBlock;
                GetComponent<Image>().sprite = _selectedOutlineSprite;
            }
            else
            {
                _textInput.colors = _defaultColorBlock;
            }
        }

        public override void OnSelect()
        {
            Validate();
            
            if (_textInput.interactable)
            {
                _textPlaceholder.color = _primaryTextColor;
                _icon.color = _primaryTextColor;
                GetComponent<Image>().sprite = _selectedOutlineSprite;
            }
        }

        public override void OnDeselect()
        {
            Validate();

            if (_isTextValid)
            {
                GetComponent<Image>().sprite = _normalOutlineSprite;
            }

            if (_textInput.interactable)
            {
                _textPlaceholder.color = _secondaryTextColor;
                _icon.color = _secondaryTextColor;
                _textInput.colors = _defaultColorBlock;
            }
        }

        public override void SetEnabledColors()
        {
            _text.color = _primaryTextColor;
            _textPlaceholder.color = _secondaryTextColor;
            _icon.color = _primaryTextColor;
        }

        public override void SetDisabledColors()
        {
            _text.color = _disabledTextColor;
            _textPlaceholder.color = _disabledTextColor;
            _icon.color = _disabledTextColor;
        }
    }
}
