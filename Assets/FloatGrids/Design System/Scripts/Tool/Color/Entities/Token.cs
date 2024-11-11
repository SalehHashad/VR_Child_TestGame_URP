namespace floatgrids
{
    public class Token
    {
        private string _name;
        private string _parent;
        private float _alpha;
        private bool _editable;

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Parent
        {
            get
            {
                return _parent;
            }
        }

        public float Alpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                _alpha = value;
            }
        }

        public bool Editable
        {
            get
            {
                return _editable;
            }
        }

        public Token(TokenDAO tokenColorDAO)
        {
            _name = tokenColorDAO.name;
            _parent = tokenColorDAO.parent;
            _alpha = tokenColorDAO.alpha;
            _editable = tokenColorDAO.editable;
        }
    }
}