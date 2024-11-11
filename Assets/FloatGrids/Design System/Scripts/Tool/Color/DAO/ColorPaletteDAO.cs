namespace floatgrids
{
    [System.Serializable]
    public class ColorPaletteDAO
    {
        public BaseColorFamilyDAO[] baseColorFamilies;
        public TokenStyleDAO[] tokenStyles;

        public ColorPaletteDAO(ColorPalette colorPalette)
        {
            baseColorFamilies = new BaseColorFamilyDAO[colorPalette.BaseColorFamilyCount()];

            for (int i = 0; i < baseColorFamilies.Length; i++)
            {
                baseColorFamilies[i] = new BaseColorFamilyDAO(colorPalette.GetBaseColorFamily(i));
            }

            tokenStyles = new TokenStyleDAO[colorPalette.TokenStyleCount()];

            for (int i = 0; i < tokenStyles.Length; i++)
            {
                tokenStyles[i] = new TokenStyleDAO(colorPalette.GetTokenStyle(i));
            }
        }
    }
}
