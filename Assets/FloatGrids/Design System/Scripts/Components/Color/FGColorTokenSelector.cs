using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace floatgrids
{
    public class ListToPopupAttribute : PropertyAttribute
    {
        public Type myType;
        public string propertyName;

        public ListToPopupAttribute(Type _myType, string _propertyName)
        {
            myType = _myType;
            propertyName = _propertyName;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ListToPopupAttribute))]
    public class ListToPopupDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ListToPopupAttribute atb = attribute as ListToPopupAttribute;
            List<string> stringList = null;

            if (atb.myType.GetField(atb.propertyName) != null)
            {
                stringList = atb.myType.GetField(atb.propertyName).GetValue(atb.myType) as List<string>;
            }

            if (stringList != null && stringList.Count != 0)
            {
                int selectedIndex = Mathf.Max(stringList.IndexOf(property.stringValue), 0);
                selectedIndex = EditorGUI.Popup(position, property.name, selectedIndex, stringList.ToArray());
                property.stringValue = stringList[selectedIndex];
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
#endif

    public class FGColorTokenSelector : FGTokenizable, ISerializationCallbackReceiver
    {
        // BASE COLOR DROPDOWN
        public static List<string> TMPTokenStyleList;
        [HideInInspector] public List<string> TokenStyleList;

        [ListToPopup(typeof(FGColorTokenSelector), "TMPTokenStyleList")]
        public string TokenFamily;

        // TOKEN STYLE DROPDOWN
        public static List<string> TMPTokenList;
        [HideInInspector] public List<string> TokenList;

        [ListToPopup(typeof(FGColorTokenSelector), "TMPTokenList")]
        public string ColorToken;


        [ContextMenu("Populate with color Palette")]
        public void FillDropdownLists()
        {
            //Token Styles
            TokenStyleList = new List<string>();

            for (int i = 0; i < ColorPaletteManager.ColorPalette.TokenStyleCount(); i++)
            {
                TokenStyleList.Add(ColorPaletteManager.ColorPalette.GetTokenStyle(i).Name);
            }

            //Tokens
            TokenList = new List<string>();
            TokenStyle tokenStyle = ColorPaletteManager.ColorPalette.GetTokenStyleByName(TokenFamily);
            if (tokenStyle != null) {
                for (int i = 0; i < tokenStyle.TokenCount(); i++)
                {
                    TokenList.Add(tokenStyle.GetToken(i).Name);
                }
            }           
        }

        private void SetColor()
        {
            Image image = GetComponent<Image>();
            TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

            if(ColorToken != null && TokenFamily != null && ColorToken.Length > 0 && TokenFamily.Length > 0)
            {
                Token token = ColorPaletteManager.ColorPalette.GetTokenStyleByName(TokenFamily).GetTokenByName(ColorToken);

                if (image != null)
                {
                    image.color = ColorPaletteManager.ColorPalette.GetTokenColor(token);
                }
                else
                {
                    text.color = ColorPaletteManager.ColorPalette.GetTokenColor(token);
                }
            }
        }

        [ContextMenu("UpdateList")]
        public void OnBeforeSerialize()
        {
            TMPTokenStyleList = TokenStyleList;
            TMPTokenList = TokenList;
        }

        public void OnAfterDeserialize()
        {
        }

        public override void Setup()
        {
            FillDropdownLists();
            SetColor();
        }
    }
}