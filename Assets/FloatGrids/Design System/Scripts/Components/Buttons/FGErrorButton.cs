namespace floatgrids
{
    public class FGErrorButton : FGButton
    {
        public override void Setup()
        {
            _normalColor = ColorPaletteManager.GetColorByTokenName("Error", "Main");
            _highlightedColor = ColorPaletteManager.GetColorByTokenName("Error", "Dark");
            _pressedColor = ColorPaletteManager.GetColorByTokenName("Error", "Darker");
            _selectedColor = ColorPaletteManager.GetColorByTokenName("Error", "Main");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            base.Setup();
        }
    }
}