using System.Collections.Generic;

namespace floatgrids
{
    public class BaseColorFamily
    {
        private string _name;
        private List<BaseColor> _baseColors;

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public BaseColorFamily(BaseColorFamilyDAO baseColorFamilyDAO)
        {
            _name = baseColorFamilyDAO.name;
            _baseColors = new List<BaseColor>();

            foreach (BaseColorDAO baseColorDAO in baseColorFamilyDAO.baseColors)
            {
                _baseColors.Add(new BaseColor(baseColorDAO));
            }
        }

        public BaseColor GetBaseColor(int index)
        {
            return _baseColors[index];
        }

        public int BaseColorCount()
        {
            return _baseColors.Count;
        }
    }
}
