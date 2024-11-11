using UnityEngine;
using UnityEngine.UI;

namespace floatgrids
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Toggle))]
    [ExecuteInEditMode]
    public class FGToggle : FGTokenizable
    {
        private const float _colorMultiplier = 1;
        private const float _fadeDuration = 0.2f;

        [SerializeField]
        private ColorBlock _onColorBlock;
        private Color _normalOnColor;
        private Color _highlightedOnColor;
        private Color _bulletOnColor;
        private Color _backgroundDisabledColor;
        private Color _bulletDisabledColor;

        [SerializeField]
        private ColorBlock _offColorBlock;
        private Color _normalOffColor;
        private Color _highlightedOffColor;
        private Color _bulletOffColor;

        private Toggle _toggle;
        private bool _interactable;

        private float _xPos = 6.9f;

        public override void Setup()
        {
            // ON Colors
            _normalOnColor = ColorPaletteManager.GetColorByTokenName("Primary", "Main");
            _highlightedOnColor = ColorPaletteManager.GetColorByTokenName("Primary", "Dark");
            _bulletOnColor = ColorPaletteManager.GetColorByTokenName("On surface", "On Dark");
            
            _backgroundDisabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");
            _bulletDisabledColor = ColorPaletteManager.GetColorByTokenName("Transparent", "White 8%");

            _toggle = GetComponent<Toggle>();
            _toggle.transition = Selectable.Transition.ColorTint;
            _toggle.targetGraphic.GetComponent<Image>().color = Color.white;
            _interactable = _toggle.interactable;

            _onColorBlock = new ColorBlock();
            _onColorBlock.normalColor = _normalOnColor;
            _onColorBlock.highlightedColor = _highlightedOnColor;
            _onColorBlock.selectedColor = _normalOnColor;
            _onColorBlock.pressedColor = _normalOnColor;
            _onColorBlock.disabledColor = _backgroundDisabledColor;

            _onColorBlock.colorMultiplier = _colorMultiplier;
            _onColorBlock.fadeDuration = _fadeDuration;
            
            // OFF Colors
            _normalOffColor = ColorPaletteManager.GetColorByTokenName("Transparent", "White 24%");
            _highlightedOffColor = ColorPaletteManager.GetColorByTokenName("Transparent", "White 56%");
            _bulletOffColor = _bulletOnColor;
            
            _offColorBlock = new ColorBlock();
            _offColorBlock.normalColor = _normalOffColor;
            _offColorBlock.highlightedColor = _highlightedOffColor;
            _offColorBlock.selectedColor = _normalOffColor;
            _offColorBlock.pressedColor = _normalOffColor;
            _offColorBlock.disabledColor = _backgroundDisabledColor;

            _offColorBlock.colorMultiplier = _colorMultiplier;
            _offColorBlock.fadeDuration = _fadeDuration;

            _toggle.colors = _toggle.isOn ? _onColorBlock : _offColorBlock;
        }

        void Start()
        {
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
            {
                _toggle.graphic.transform.parent.GetComponent<Image>().color = _bulletOnColor;
            }
            else
            {
                _toggle.graphic.transform.parent.GetComponent<Image>().color = _bulletOffColor;
            }
        }

        private void SetDisabledColors()
        {
            _toggle.graphic.transform.parent.GetComponent<Image>().color = _bulletDisabledColor;
            _toggle.graphic.color = _normalOffColor;
        }

        public void OnDisable()
        {
            //_toggle.onValueChanged.RemoveListener(OnToggle);
        }

        public void OnToggle()
        {
            _toggle.colors = _toggle.isOn ? _onColorBlock : _offColorBlock;
            _toggle.graphic.transform.parent.GetComponent<RectTransform>().localPosition = _toggle.isOn ? new Vector3(_xPos, 0, 0) : new Vector3(-_xPos, 0, 0);
        }
    }
}