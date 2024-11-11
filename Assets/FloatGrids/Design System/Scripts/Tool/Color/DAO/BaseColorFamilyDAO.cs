namespace floatgrids
{
    [System.Serializable]
    public class BaseColorFamilyDAO
    {
        public string name;
        public BaseColorDAO[] baseColors;

        public BaseColorFamilyDAO(BaseColorFamily baseColorFamily)
        {
            name = baseColorFamily.Name;
            baseColors = new BaseColorDAO[baseColorFamily.BaseColorCount()];

            for (int i = 0; i < baseColors.Length; i++)
            {
                baseColors[i] = new BaseColorDAO(baseColorFamily.GetBaseColor(i));
            }
        }
    }
}
