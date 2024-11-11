using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace floatgrids 
{    
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(TMP_InputField))]
    [ExecuteInEditMode]
    public abstract class FGTextInput : FGTokenizable
    {
        protected const float _colorMultiplier = 1;
        protected const float _fadeDuration = 0.2f;

        [HideInInspector]
        [SerializeField]
        protected ColorBlock _defaultColorBlock;

        [HideInInspector]
        [SerializeField]
        protected ColorBlock _errorColorBlock;

        protected Color _normalColor;
        protected Color _highlightedColor;
        protected Color _selectedColor;
        protected Color _pressedColor;
        protected Color _disabledColor;

        [HideInInspector]
        [SerializeField]
        protected Color _primaryTextColor;
        [HideInInspector]
        [SerializeField]
        protected Color _secondaryTextColor;
        [HideInInspector]
        [SerializeField]
        protected Color _disabledTextColor;

        protected TMP_InputField _textInput;

        protected TextMeshProUGUI _textPlaceholder;
        protected TextMeshProUGUI _text;

        private bool _interactable;

        public enum Corner { Four = 4, Six = 6, Eight = 8, Twelve = 12, Fourteen = 14, Sixteen = 16, Eighteen = 18, Twenty = 20 }

        [SerializeField]
        public Corner corner = Corner.Four;

        private string _prefix = "corners-";
        private string _normalSufix = "px-Outlined";
        private string _selectedSufix = "px-Outlined-Selected";

        protected Sprite _normalOutlineSprite;
        protected Sprite _selectedOutlineSprite;

        public override void Setup()
        {
            _normalOutlineSprite = Resources.Load<Sprite>(_prefix + (int)corner + _normalSufix);
            _selectedOutlineSprite = Resources.Load<Sprite>(_prefix + (int)corner + _selectedSufix);
            
            _normalColor = ColorPaletteManager.GetColorByTokenName("Outline border", "Outline border");
            _highlightedColor = ColorPaletteManager.GetColorByTokenName("Transparent", "White 56%");
            _pressedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Main");
            _selectedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Main");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            // Setup colors for square frame
            GetComponent<Image>().color = Color.white;

            _textInput = GetComponent<TMP_InputField>();
            _textInput.transition = Selectable.Transition.ColorTint;

            _defaultColorBlock = new ColorBlock();
            _defaultColorBlock.normalColor = _normalColor;
            _defaultColorBlock.highlightedColor = _highlightedColor;
            _defaultColorBlock.selectedColor = _selectedColor;
            _defaultColorBlock.pressedColor = _pressedColor;
            _defaultColorBlock.disabledColor = _disabledColor;

            _defaultColorBlock.colorMultiplier = _colorMultiplier;
            _defaultColorBlock.fadeDuration = _fadeDuration;

            _textInput.colors = _defaultColorBlock;

            // Initialize text components
            _textPlaceholder = _textInput.placeholder.GetComponent<TextMeshProUGUI>();
            _text = _textInput.textComponent.GetComponent<TextMeshProUGUI>();
            _interactable = _textInput.interactable;

            // Initialize text colors
            _primaryTextColor = ColorPaletteManager.GetColorByTokenName("Text", "Text Primary");
            _secondaryTextColor = ColorPaletteManager.GetColorByTokenName("Text", "Text Secondary");
            _disabledTextColor = ColorPaletteManager.GetColorByTokenName("Text", "Text Disabled");
        }

        public void Start()
        {
            _normalOutlineSprite = Resources.Load<Sprite>(_prefix + (int)corner + _normalSufix);
            _selectedOutlineSprite = Resources.Load<Sprite>(_prefix + (int)corner + _selectedSufix);
            _textInput = GetComponent<TMP_InputField>();
            _textPlaceholder = _textInput.placeholder.GetComponent<TextMeshProUGUI>();
            _text = _textInput.textComponent.GetComponent<TextMeshProUGUI>();
            _interactable = _textInput.interactable;
        }

        public void Update()
        {
            if (_interactable != _textInput.interactable)
            {
                if (!_textInput.interactable)
                {
                    SetDisabledColors();
                }
                else
                {
                    SetEnabledColors();
                }

                _interactable = _textInput.interactable;
            }
        }

        public abstract void OnSelect();
        public abstract void OnDeselect();

        public abstract void SetEnabledColors();

        public abstract void SetDisabledColors();
    }
}
