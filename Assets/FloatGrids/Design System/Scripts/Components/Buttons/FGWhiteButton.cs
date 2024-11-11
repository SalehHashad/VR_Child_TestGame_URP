namespace floatgrids
{
    public class FGWhiteButton : FGButton
    {
        public override void Setup()
        {
            _normalColor = ColorPaletteManager.GetColorByTokenName("On surface", "On Dark");
            _highlightedColor = ColorPaletteManager.GetColorByTokenName("Grey", "Light");
            _pressedColor = ColorPaletteManager.GetColorByTokenName("Grey", "Lighter");
            _selectedColor = ColorPaletteManager.GetColorByTokenName("On surface", "On Dark");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            base.Setup();
        }
    }
}
