namespace floatgrids
{
    public class FGClickableImage : FGButton
    {
        public override void Setup()
        {
            _normalColor = ColorPaletteManager.GetColorByTokenName("Transparent", "Neutral");
            _highlightedColor = ColorPaletteManager.GetColorByTokenName("Transparent", "Black 72%");
            _pressedColor = ColorPaletteManager.GetColorByTokenName("Transparent", "Black 32%");
            _selectedColor = ColorPaletteManager.GetColorByTokenName("Transparent", "Black 72%");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Transparent", "Black 72%");

            base.Setup();
        }
    }
}
