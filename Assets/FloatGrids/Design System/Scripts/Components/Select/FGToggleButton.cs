using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace floatgrids
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Toggle))]
    [ExecuteInEditMode]
    public class FGToggleButton : FGTokenizable, IPointerEnterHandler, IPointerExitHandler
    {
        private const float _colorMultiplier = 1;
        private const float _fadeDuration = 0.2f;

        [SerializeField]
        private ColorBlock _onColorBlock;
        private Color _normalOnColor;
        private Color _highlightedOnColor;
        private Color _disabledColor;

        [SerializeField]
        private ColorBlock _offColorBlock;
        private Color _normalOffColor;
        private Color _highlightedOffColor;

        private Color _primaryTextColor;
        private Color _disabledTextColor;

        private Toggle _toggle;
        private bool _interactable;

        public enum Corner { Four = 4, Six = 6, Eight = 8, Twelve = 12, Fourteen = 14, Sixteen = 16, Eighteen = 18, Twenty = 20 }

        [SerializeField]
        public Corner corner = Corner.Four;

        private string _prefix = "corners-";
        private string _normalSufix = "px-Outlined";
        private string _selectedSufix = "px-Outlined-Selected";
        private string _filledSufix = "px-Filled";

        private Sprite _normalOutlineSprite;
        private Sprite _selectedOutlineSprite;
        private Sprite _filledSprite;

        public override void Setup()
        {
            _normalOutlineSprite = Resources.Load<Sprite>(_prefix + (int)corner + _normalSufix);
            _selectedOutlineSprite = Resources.Load<Sprite>(_prefix + (int)corner + _selectedSufix);
            _filledSprite = Resources.Load<Sprite>(_prefix + (int)corner + _filledSufix);

            // ON Colors
            _normalOnColor = ColorPaletteManager.GetColorByTokenName("Primary", "Main");
            _highlightedOnColor = ColorPaletteManager.GetColorByTokenName("Primary", "Dark");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            _toggle = GetComponent<Toggle>();
            _toggle.transition = Selectable.Transition.ColorTint;
            _interactable = _toggle.interactable;

            _onColorBlock = new ColorBlock();
            _onColorBlock.normalColor = _normalOnColor;
            _onColorBlock.highlightedColor = _highlightedOnColor;
            _onColorBlock.selectedColor = _highlightedOnColor;
            _onColorBlock.pressedColor = _highlightedOnColor;
            _onColorBlock.disabledColor = _disabledColor;

            _onColorBlock.colorMultiplier = _colorMultiplier;
            _onColorBlock.fadeDuration = _fadeDuration;

            // OFF Colors
            _normalOffColor = ColorPaletteManager.GetColorByTokenName("Outline border", "Outline border");
            _highlightedOffColor = ColorPaletteManager.GetColorByTokenName("Transparent", "White 56%");

            _offColorBlock = new ColorBlock();
            _offColorBlock.normalColor = _normalOffColor;
            _offColorBlock.highlightedColor = _highlightedOffColor;
            _offColorBlock.selectedColor = _highlightedOffColor;
            _offColorBlock.pressedColor = _highlightedOffColor;
            _offColorBlock.disabledColor = _disabledColor;

            _offColorBlock.colorMultiplier = _colorMultiplier;
            _offColorBlock.fadeDuration = _fadeDuration;

            _toggle.colors = _toggle.isOn ? _onColorBlock : _offColorBlock;

            // Text Colors
            _primaryTextColor = ColorPaletteManager.GetColorByTokenName("Text", "Text Primary");
            _disabledTextColor = ColorPaletteManager.GetColorByTokenName("Text", "Text Disabled");
        }

        void Start()
        {
            _normalOutlineSprite = Resources.Load<Sprite>(_prefix + (int)corner + _normalSufix);
            _selectedOutlineSprite = Resources.Load<Sprite>(_prefix + (int)corner + _selectedSufix);
            _filledSprite = Resources.Load<Sprite>(_prefix + (int)corner + _filledSufix);

            _toggle = GetComponent<Toggle>();
            _interactable = _toggle.interactable;
            _toggle.colors = _toggle.isOn ? _onColorBlock : _offColorBlock;
        }

        public void Update()
        {
            if (_toggle != null && _interactable != _toggle.interactable)
            {
                if (!_toggle.interactable)
                {
                    SetDisabledColors();
                }
                else
                {
                    SetEnabledColors();
                }

                _interactable = _toggle.interactable;
            }
        }

        private void SetEnabledColors()
        {
            if (_toggle.isOn)
                _toggle.graphic.GetComponent<Image>().color = _primaryTextColor;
        }

        private void SetDisabledColors()
        {
            if (_toggle.isOn)
                _toggle.graphic.GetComponent<Image>().color = _disabledTextColor;
        }

        public void OnToggle()
        {
            _toggle.colors = _toggle.isOn ? _onColorBlock : _offColorBlock;
            _toggle.targetGraphic.GetComponent<Image>().sprite = _toggle.isOn ? _filledSprite : _normalOutlineSprite;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_toggle.isOn)
                _toggle.targetGraphic.GetComponent<Image>().sprite = _normalOutlineSprite;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_toggle.isOn)
                _toggle.targetGraphic.GetComponent<Image>().sprite = _selectedOutlineSprite;
        }
    }
}