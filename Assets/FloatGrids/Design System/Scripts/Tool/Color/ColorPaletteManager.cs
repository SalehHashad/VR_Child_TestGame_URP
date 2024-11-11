using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace floatgrids
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class ColorPaletteManager : MonoBehaviour
    {
        private static string s_colorPresetPath = "Assets/FloatGrids/Design System/Editor/FloatGridsPalette.json";

        private static ColorPaletteDAO s_colorPaletteDAO;
        private static ColorPalette s_colorPalette;

        public static ColorPalette ColorPalette
        {
            get
            {
                return s_colorPalette;
            }
        }

        static ColorPaletteManager()
        {
            Init();
        }

        public static void Init() {
            StreamReader reader = new StreamReader(s_colorPresetPath);
            s_colorPaletteDAO = JsonUtility.FromJson<ColorPaletteDAO>(reader.ReadToEnd());
            s_colorPalette = new ColorPalette(s_colorPaletteDAO);
            reader.Close();
        }

        public static void Save()
        {
            s_colorPaletteDAO = new ColorPaletteDAO(s_colorPalette);
            string json = JsonUtility.ToJson(s_colorPaletteDAO);
            StreamWriter writer = new StreamWriter(s_colorPresetPath, false);
            writer.WriteLine(json);
            writer.Close();
        }

        public static void UpdateComponents()
        {
            foreach (FGTokenizable fGTokenizable in FindObjectsOfType<FGTokenizable>(true))
            {
                fGTokenizable.Setup();
            }
        }

        public static Color GetColorByTokenName(string tokenStyleName, string tokenName)
        {
            Color output = Color.black;

            TokenStyle tokenStyle = s_colorPalette.GetTokenStyleByName(tokenStyleName);
            
            if (tokenStyle != null)
            {
                output = s_colorPalette.GetTokenColor(tokenStyle.GetTokenByName(tokenName));
            }

            return output;
        }
    }
}
