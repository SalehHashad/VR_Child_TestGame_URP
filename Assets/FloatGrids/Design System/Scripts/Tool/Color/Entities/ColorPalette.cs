using System.Collections.Generic;
using UnityEngine;

namespace floatgrids
{
    public class ColorPalette
    {
        private List<BaseColorFamily> _baseColorFamilies;
        private List<TokenStyle> _tokenStyles;

        public ColorPalette(ColorPaletteDAO colorPaletteDAO)
        {
            _baseColorFamilies = new List<BaseColorFamily>();

            foreach (BaseColorFamilyDAO baseColorFamilyDAO in colorPaletteDAO.baseColorFamilies)
            {
                _baseColorFamilies.Add(new BaseColorFamily(baseColorFamilyDAO));
            }

            _tokenStyles = new List<TokenStyle>();

            foreach (TokenStyleDAO tokenStyleDAO in colorPaletteDAO.tokenStyles)
            {
                _tokenStyles.Add(new TokenStyle(tokenStyleDAO));
            }
        }

        public BaseColorFamily GetBaseColorFamily(int index)
        {
            return _baseColorFamilies[index];
        }

        private Color GetBaseColorByNames(string baseColorFamilyName, string baseColorName)
        {
            Color output = Color.black;

            for (int i = 0; i < BaseColorFamilyCount(); i++)
            {
                if (GetBaseColorFamily(i).Name.Equals(baseColorFamilyName))
                {
                    BaseColorFamily baseColorFamily = GetBaseColorFamily(i);

                    for (int j = 0; j < baseColorFamily.BaseColorCount(); j++)
                    {
                        if (baseColorFamily.GetBaseColor(j).Name.Equals(baseColorName))
                        {
                            BaseColor baseColor = baseColorFamily.GetBaseColor(j);
                            output = baseColor.Color;
                            continue;
                        }
                    }
                    continue;
                }
            }

            return output;
        }

        public int BaseColorFamilyCount()
        {
            return _baseColorFamilies.Count;
        }

        public TokenStyle GetTokenStyle(int index)
        {
            return _tokenStyles[index];
        }

        public int TokenStyleCount()
        {
            return _tokenStyles.Count;
        }

        public Color GetTokenColor(Token token)
        {
            Color output = Color.black;

            string baseColorFamilyName = token.Parent.Split('_')[0];
            string baseColorName = token.Parent.Split('_')[1];

            output = GetBaseColorByNames(baseColorFamilyName, baseColorName);
            output = new Color(output.r, output.g, output.b, token.Alpha);

            return output;
        }

        public void UpdateToken(Token token, Color updatedColor)
        {
            token.Alpha = updatedColor.a;
        }

        public TokenStyle GetTokenStyleByName(string name)
        {
            TokenStyle output = null;

            foreach (TokenStyle tokenStyle in _tokenStyles)
            {
                if (tokenStyle.Name.Equals(name))
                {
                    output = tokenStyle;
                    continue;
                }
            }

            return output;
        }
    }
}
