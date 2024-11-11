namespace floatgrids
{
    [System.Serializable]
    public class TokenDAO
    {
        public string name;
        public string parent;
        public float alpha;
        public bool editable;

        public TokenDAO(Token tokenColor)
        {
            name = tokenColor.Name;
            parent = tokenColor.Parent;
            alpha = tokenColor.Alpha;
            editable = tokenColor.Editable;
        }
    }
}
