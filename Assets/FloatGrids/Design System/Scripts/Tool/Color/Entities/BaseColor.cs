using UnityEngine;

namespace floatgrids
{
    public class BaseColor
    {
        private string _name;
        private Color _color;

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        public BaseColor(BaseColorDAO baseColorDAO)
        {
            _name = baseColorDAO.name;
            _color = new Color(baseColorDAO.color[0], baseColorDAO.color[1], baseColorDAO.color[2], baseColorDAO.color[3]);
        }
    }
}
