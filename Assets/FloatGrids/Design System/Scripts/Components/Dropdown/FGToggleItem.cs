using UnityEngine.UI;

namespace floatgrids { 
    public class FGToggleItem : FGTokenizable
    {
        private const float _colorMultiplier = 1;
        private const float _fadeDuration = 0.2f;

        private ColorBlock _onColorBlock;

        private Toggle _toggle;

        public override void Setup()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.transition = Selectable.Transition.ColorTint;

            _onColorBlock = new ColorBlock();
            _onColorBlock.normalColor = ColorPaletteManager.GetColorByTokenName("Transparent", "Neutral");
            _onColorBlock.highlightedColor = ColorPaletteManager.GetColorByTokenName("Grey", "Pressed");
            _onColorBlock.selectedColor = ColorPaletteManager.GetColorByTokenName("Grey", "Selected");
            _onColorBlock.pressedColor = ColorPaletteManager.GetColorByTokenName("Grey", "Selected");
            _onColorBlock.disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            _onColorBlock.colorMultiplier = _colorMultiplier;
            _onColorBlock.fadeDuration = _fadeDuration;

            _toggle.colors = _onColorBlock;
        }
    }
}
