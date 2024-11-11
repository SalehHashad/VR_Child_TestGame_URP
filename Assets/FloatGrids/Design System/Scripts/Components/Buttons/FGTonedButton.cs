namespace floatgrids
{
    public class FGTonedButton : FGButton
    {
        public override void Setup()
        {
            _normalColor = ColorPaletteManager.GetColorByTokenName("Primary", "Background");
            _highlightedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Hover");
            _pressedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Pressed");
            _selectedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Background");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            base.Setup();
        }
    }
}