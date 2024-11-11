namespace floatgrids
{
    public class FGTertiaryButton : FGButton
    {
        public override void Setup()
        {
            _normalColor = ColorPaletteManager.GetColorByTokenName("Transparent", "White 8%");
            _highlightedColor = ColorPaletteManager.GetColorByTokenName("Transparent", "White 16%");
            _pressedColor = ColorPaletteManager.GetColorByTokenName("Transparent", "White 4%");
            _selectedColor = ColorPaletteManager.GetColorByTokenName("Transparent", "White 8%");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            base.Setup();
        }
    }
}