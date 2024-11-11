namespace floatgrids
{
    public class FGPrimaryButton : FGButton
    {
        public override void Setup()
        {
            _normalColor = ColorPaletteManager.GetColorByTokenName("Primary", "Main");
            _highlightedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Dark");
            _pressedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Darker");
            _selectedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Main");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            base.Setup();
        }
    }
}