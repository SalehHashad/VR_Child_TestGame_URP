using System.Collections.Generic;

namespace floatgrids
{
    public class TokenStyle
    {
        private string _name;
        private List<Token> _tokens;

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public TokenStyle(TokenStyleDAO tokenStyleDAO)
        {
            _name = tokenStyleDAO.name;
            _tokens = new List<Token>();

            foreach (TokenDAO tokenDAO in tokenStyleDAO.tokens)
            {
                _tokens.Add(new Token(tokenDAO));
            }
        }

        public Token GetToken(int index)
        {
            return _tokens[index];
        }

        public int TokenCount()
        {
            return _tokens.Count;
        }

        public Token GetTokenByName(string name)
        {
            Token output = null;

            foreach (Token token in _tokens)
            {
                if (token.Name.Equals(name))
                {
                    output = token;
                    continue;
                }
            }

            return output;
        }
    }
}
