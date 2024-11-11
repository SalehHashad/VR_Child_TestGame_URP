namespace floatgrids {
    public class FGSecondaryButton : FGButton
    {
        public override void Setup()
        {
            _normalColor = ColorPaletteManager.GetColorByTokenName("Transparent", "Black 8%");
            _highlightedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Hover");
            _pressedColor = ColorPaletteManager.GetColorByTokenName("Primary", "Pressed");
            _selectedColor = ColorPaletteManager.GetColorByTokenName("Transparent", "Black 8%");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            base.Setup();
        }
    }
}