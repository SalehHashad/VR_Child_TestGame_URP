using UnityEngine;
using UnityEngine.UI;

namespace floatgrids
{
    [ExecuteInEditMode]
    public class FGSlider : FGTokenizable
    {
        private const float _colorMultiplier = 1;
        private const float _fadeDuration = 0.2f;

        [HideInInspector]
        [SerializeField]
        private Color _backgroundColor;
        [HideInInspector]
        [SerializeField]
        private Color _fillColor;

        [HideInInspector]
        [SerializeField]
        private Color _backgroundDisabledColor;
        [HideInInspector]
        [SerializeField]
        private Color _fillDisabledColor;

        protected Slider _slider = null;

        [SerializeField]
        protected Image _background = null;

        private bool _wasInteractible = false;

        public override void Setup()
        {
            _slider = this.GetComponent<Slider>();
                
            _slider.transition = Selectable.Transition.ColorTint;
            ColorBlock colorBlock = new ColorBlock();

            colorBlock.normalColor = ColorPaletteManager.GetColorByTokenName("Primary", "Main");
            colorBlock.highlightedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Dark");
            colorBlock.selectedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Darker");
            colorBlock.pressedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Main");
            colorBlock.disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Dark");

            colorBlock.colorMultiplier = _colorMultiplier;
            colorBlock.fadeDuration = _fadeDuration;

            _slider.colors = colorBlock;

            _backgroundColor = ColorPaletteManager.GetColorByTokenName("Transparent", "White 24%");
            _fillColor = ColorPaletteManager.GetColorByTokenName("Primary", "Light");

            _backgroundDisabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");
            _fillDisabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Neutral");

            PaintAdditionalObjects();
        }

        void Start()
        {
            _slider = this.GetComponent<Slider>();

        }

        private void OnDisable()
        {
            PaintAdditionalObjects();
        }

        private void PaintAdditionalObjects()
        {
            if (this.GetComponent<Slider>().interactable)
            {
                _background.color = _backgroundColor;
                _slider.fillRect.GetComponent<Image>().color = _fillColor;
            }
            else
            {
                _background.color = _backgroundDisabledColor;
                _slider.fillRect.GetComponent<Image>().color = _fillDisabledColor;
            }
        }

        void Update()
        {
            if (_wasInteractible != this.GetComponent<Slider>().interactable)
            {
                PaintAdditionalObjects();
                _wasInteractible = this.GetComponent<Slider>().interactable;
            }
        }
    }   
}