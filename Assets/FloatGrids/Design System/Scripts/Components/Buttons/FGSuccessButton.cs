namespace floatgrids
{
    public class FGSuccessButton : FGButton
    {
        public override void Setup()
        {
            _normalColor = ColorPaletteManager.GetColorByTokenName("Success", "Main");
            _highlightedColor = ColorPaletteManager.GetColorByTokenName("Success", "Dark");
            _pressedColor = ColorPaletteManager.GetColorByTokenName("Success", "Darker");
            _selectedColor = ColorPaletteManager.GetColorByTokenName("Success", "Main");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            base.Setup();
        }
    }
}