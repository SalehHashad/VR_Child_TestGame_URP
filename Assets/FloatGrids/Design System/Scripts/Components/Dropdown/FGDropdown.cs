using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace floatgrids {
    public class FGDropdown : FGTokenizable, ISelectHandler, IDeselectHandler
    {
        private const float _colorMultiplier = 1;
        private const float _fadeDuration = 0.2f;
        
        protected TMP_Dropdown _dropdown = null;

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

            _dropdown = this.GetComponent<TMP_Dropdown>();

            _dropdown.transition = Selectable.Transition.ColorTint;
            ColorBlock colorBlock = new ColorBlock();

            colorBlock.normalColor = ColorPaletteManager.GetColorByTokenName("Outline border", "Outline border");
            colorBlock.highlightedColor = ColorPaletteManager.GetColorByTokenName("Transparent", "White 56%");
            colorBlock.pressedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Main");
            colorBlock.selectedColor = ColorPaletteManager.GetColorByTokenName("Outline border", "Outline border");
            colorBlock.disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            colorBlock.colorMultiplier = _colorMultiplier;
            colorBlock.fadeDuration = _fadeDuration;

            _dropdown.colors = colorBlock;
        }

        void Start()
        {
            _normalOutlineSprite = Resources.Load<Sprite>(_prefix + (int)corner + _normalSufix);
            _selectedOutlineSprite = Resources.Load<Sprite>(_prefix + (int)corner + _selectedSufix);
            _dropdown = this.GetComponent<TMP_Dropdown>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (_dropdown != null)
            {
                _dropdown.GetComponent<Image>().sprite = _selectedOutlineSprite;
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (_dropdown != null)
            {
                _dropdown.GetComponent<Image>().sprite = _normalOutlineSprite;
            }
        }
    }
}
