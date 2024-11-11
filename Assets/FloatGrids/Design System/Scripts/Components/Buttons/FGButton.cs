using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace floatgrids
{
    [RequireComponent(typeof(Button))]
    public abstract class FGButton : FGTokenizable
    {
        private const float _colorMultiplier = 1;
        private const float _fadeDuration = 0.2f;

        protected Color _normalColor;
        protected Color _highlightedColor;
        protected Color _selectedColor;
        protected Color _pressedColor;
        protected Color _disabledColor;

        private Button _button;

        public override void Setup() 
        {
            _button = GetComponent<Button>();
            _button.targetGraphic.color = Color.white;
            _button.transition = Selectable.Transition.ColorTint;

            ColorBlock colorBlock = new ColorBlock();
            colorBlock.normalColor = _normalColor;
            colorBlock.highlightedColor = _highlightedColor;
            colorBlock.selectedColor = _selectedColor;
            colorBlock.pressedColor = _pressedColor;
            colorBlock.disabledColor = _disabledColor;

            colorBlock.colorMultiplier = _colorMultiplier;
            colorBlock.fadeDuration = _fadeDuration;

            _button.colors = colorBlock;
        }
    }
}
