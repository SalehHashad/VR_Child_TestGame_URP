namespace floatgrids
{
    [System.Serializable]
    public class TokenStyleDAO
    {
        public string name;
        public TokenDAO[] tokens;

        public TokenStyleDAO(TokenStyle tokenStyle)
        {
            name = tokenStyle.Name;
            tokens = new TokenDAO[tokenStyle.TokenCount()];

            for (int i = 0; i < tokens.Length; i++)
            {
                tokens[i] = new TokenDAO(tokenStyle.GetToken(i));
            }
        }
    }
}
