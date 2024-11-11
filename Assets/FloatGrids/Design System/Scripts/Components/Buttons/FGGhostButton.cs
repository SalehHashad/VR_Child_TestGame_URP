namespace floatgrids
{
    public class FGGhostButton : FGButton
    {
        public override void Setup()
        {
            _normalColor = ColorPaletteManager.GetColorByTokenName("Transparent", "Neutral");
            _highlightedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Hover");
            _pressedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Pressed");
            _selectedColor = ColorPaletteManager.GetColorByTokenName("Transparent", "Neutral");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            base.Setup();
        }
    }
}