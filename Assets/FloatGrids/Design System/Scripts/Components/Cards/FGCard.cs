namespace floatgrids
{
    public class FGCard : FGButton 
    {
        public override void Setup()
        {
            _normalColor = ColorPaletteManager.GetColorByTokenName("Surface", "Card");
            _highlightedColor = ColorPaletteManager.GetColorByTokenName("Surface", "Card Hover");
            _pressedColor = ColorPaletteManager.GetColorByTokenName("Surface", "Card Pressed");
            _selectedColor = ColorPaletteManager.GetColorByTokenName("Surface", "Card");
            _disabledColor = ColorPaletteManager.GetColorByTokenName("Grey", "Disabled");

            base.Setup();
        }
    }
}

