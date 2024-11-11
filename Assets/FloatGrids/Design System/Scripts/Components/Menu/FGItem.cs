using UnityEngine;
using UnityEngine.UI;

namespace floatgrids
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Toggle))]
    public class FGItem : FGTokenizable
    {
        private const float _colorMultiplier = 1;
        private const float _fadeDuration = 0.2f;

        [HideInInspector]
        [SerializeField]
        private ColorBlock _onColorBlock;
        [HideInInspector]
        [SerializeField]
        private ColorBlock _offColorBlock;

        private Color _normalColor;
        private Color _highlightedColor;
        private Color _pressedColor;
        private Color _selectedColor;
        private Color _disabledColor;

        private Toggle _toggle;

        public override void Setup()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.transition = Selectable.Transition.ColorTint;

            _normalColor = ColorPaletteManager.GetColorByTokenName("Transparent", "Neutral");
            _highlightedColor = ColorPaletteManager.GetColorByTokenName("Grey", "Hover");
            _pressedColor = ColorPaletteManager.GetColorByTokenName("Grey", "Pressed");
            _selectedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Main");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            // ON Colors
            _onColorBlock = new ColorBlock();
            _onColorBlock.normalColor = _selectedColor;
            _onColorBlock.highlightedColor = _selectedColor;
            _onColorBlock.selectedColor = _selectedColor;
            _onColorBlock.pressedColor = _selectedColor;
            _onColorBlock.disabledColor = _disabledColor;

            _onColorBlock.colorMultiplier = _colorMultiplier;
            _onColorBlock.fadeDuration = _fadeDuration;

            // OFF Colors
            _offColorBlock = new ColorBlock();
            _offColorBlock.normalColor = _normalColor;
            _offColorBlock.highlightedColor = _highlightedColor;
            _offColorBlock.selectedColor = _selectedColor;
            _offColorBlock.pressedColor = _pressedColor;
            _offColorBlock.disabledColor = _disabledColor;

            _offColorBlock.colorMultiplier = _colorMultiplier;
            _offColorBlock.fadeDuration = _fadeDuration;

            _toggle.colors = _toggle.isOn ? _onColorBlock : _offColorBlock;
        }

        void Start()
        {
            _toggle = GetComponent<Toggle>();
        }

        public void OnToggle()
        {
            _toggle.colors = _toggle.isOn ? _onColorBlock : _offColorBlock;
        }
    }
}
