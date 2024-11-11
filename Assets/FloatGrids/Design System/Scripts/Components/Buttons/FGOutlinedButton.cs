namespace floatgrids
{
    public class FGOutlinedButton : FGButton
    {
        public override void Setup()
        {
            _normalColor = ColorPaletteManager.GetColorByTokenName("Transparent", "Neutral");
            _highlightedColor = ColorPaletteManager.GetColorByTokenName("Grey", "Hover");
            _pressedColor = ColorPaletteManager.GetColorByTokenName("Grey", "Pressed");
            _selectedColor = ColorPaletteManager.GetColorByTokenName("Transparent", "Neutral");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            base.Setup();
        }
    }
}