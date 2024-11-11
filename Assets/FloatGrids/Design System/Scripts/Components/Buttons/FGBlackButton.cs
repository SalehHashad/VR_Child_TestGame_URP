namespace floatgrids
{
    public class FGBlackButton : FGButton
    {
        public override void Setup()
        {
            _normalColor = ColorPaletteManager.GetColorByTokenName("Grey", "Darker");
            _highlightedColor = ColorPaletteManager.GetColorByTokenName("Grey", "Neutral");
            _pressedColor = ColorPaletteManager.GetColorByTokenName("Grey", "Dark");
            _selectedColor = ColorPaletteManager.GetColorByTokenName("Grey", "Darker");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            base.Setup();
        }
    }
}
