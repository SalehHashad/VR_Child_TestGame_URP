using UnityEditor;
using UnityEngine;

namespace floatgrids
{
    public class FloatGridsColorPalette : EditorWindow
    {
        private static bool _colorPaletteFoldoutState;

        private static bool _baseColorFoldoutState;
        private static bool[] _baseColorFamilyFoldoutState;

        private static bool _tokenFoldoutState;
        private static bool[] _tokenStylesFoldoutState;

        private static float spacing = 7f;
        private static Vector2 scrollPos;

        [MenuItem("Window/FloatGrids Color Palette")]
        static void Setup()
        {
            // Get existing open window or if none, make a new one:
            FloatGridsColorPalette floatGrids = (FloatGridsColorPalette)EditorWindow.GetWindow(typeof(FloatGridsColorPalette));
            floatGrids.titleContent = new GUIContent("FloatGrids Color Palette");
            floatGrids.Show();

            InitializeFoldoutStates();
        }

        private void OnValidate()
        {
            InitializeFoldoutStates();
        }

        private static void InitializeFoldoutStates()
        {
            _colorPaletteFoldoutState = true;
            _baseColorFoldoutState = true;
            _tokenFoldoutState = false;

            _baseColorFamilyFoldoutState = new bool[ColorPaletteManager.ColorPalette.BaseColorFamilyCount()];
            for (int i = 0; i < _baseColorFamilyFoldoutState.Length; i++)
            {
                _baseColorFamilyFoldoutState[i] = false;
            }

            _baseColorFamilyFoldoutState[0] = true;

            _tokenStylesFoldoutState = new bool[ColorPaletteManager.ColorPalette.TokenStyleCount()];
            for (int i = 0; i < _tokenStylesFoldoutState.Length; i++)
            {
                _tokenStylesFoldoutState[i] = false;
            }
        }

        void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            if (_colorPaletteFoldoutState)
            {
                using (new GUILayout.HorizontalScope())
                {
                    using (new GUILayout.VerticalScope())
                    {
                        BaseColorsGUI();

                        TokensGUI();
                    }
                }
            }

            if (GUILayout.Button("Apply Changes"))
            {
                Apply();
            }

            EditorGUILayout.EndScrollView();
        }

        private void BaseColorsGUI()
        {
            EditorGUILayout.Space(spacing * 2);

            _baseColorFoldoutState = EditorGUILayout.Foldout(_baseColorFoldoutState, "Color Palette", EditorStyles.foldoutHeader);

            if (_baseColorFoldoutState)
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Space(spacing * 4);

                    using (new GUILayout.VerticalScope())
                    {
                        EditorGUILayout.Space(spacing * 2);

                        for (int i = 0; i < ColorPaletteManager.ColorPalette.BaseColorFamilyCount(); i++)
                        {
                            _baseColorFamilyFoldoutState[i] = EditorGUILayout.Foldout(_baseColorFamilyFoldoutState[i], ColorPaletteManager.ColorPalette.GetBaseColorFamily(i).Name, EditorStyles.foldoutHeader);

                            if (_baseColorFamilyFoldoutState[i])
                            {
                                EditorGUILayout.Space(spacing * 2);

                                for (int j = 0; j < ColorPaletteManager.ColorPalette.GetBaseColorFamily(i).BaseColorCount(); j++)
                                {
                                    EditorGUILayout.Space(spacing);

                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.Space(spacing);

                                    EditorGUILayout.LabelField(ColorPaletteManager.ColorPalette.GetBaseColorFamily(i).GetBaseColor(j).Name, EditorStyles.label, GUILayout.Width(30));
                                    EditorGUILayout.Space(spacing * 2);

                                    ColorPaletteManager.ColorPalette.GetBaseColorFamily(i).GetBaseColor(j).Color = EditorGUILayout.ColorField(ColorPaletteManager.ColorPalette.GetBaseColorFamily(i).GetBaseColor(j).Color);

                                    EditorGUILayout.Space(spacing * 2);
                                    EditorGUILayout.EndHorizontal();
                                }
                                EditorGUILayout.Space(spacing * 2);
                            }
                            EditorGUILayout.Space(spacing);
                        }
                    }
                }
            }
            DrawUILine(Color.grey);
        }

        private void TokensGUI()
        {
            _tokenFoldoutState = EditorGUILayout.Foldout(_tokenFoldoutState, "Tokens", EditorStyles.foldoutHeader);

            if (_tokenFoldoutState)
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Space(spacing * 4);

                    using (new GUILayout.VerticalScope())
                    {
                        EditorGUILayout.Space(spacing * 2);

                        for (int i = 0; i < ColorPaletteManager.ColorPalette.TokenStyleCount(); i++)
                        {
                            _tokenStylesFoldoutState[i] = EditorGUILayout.Foldout(_tokenStylesFoldoutState[i], ColorPaletteManager.ColorPalette.GetTokenStyle(i).Name, EditorStyles.foldoutHeader);

                            if (_tokenStylesFoldoutState[i])
                            {
                                EditorGUILayout.Space(spacing * 2);

                                for (int j = 0; j < ColorPaletteManager.ColorPalette.GetTokenStyle(i).TokenCount(); j++)
                                {
                                    EditorGUILayout.Space(spacing);
                                    
                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.Space(spacing);

                                    EditorGUILayout.LabelField(ColorPaletteManager.ColorPalette.GetTokenStyle(i).GetToken(j).Name, EditorStyles.label, GUILayout.Width(90));
                                    EditorGUILayout.Space(spacing * 2);

                                    Color tmpColor = ColorPaletteManager.ColorPalette.GetTokenColor(ColorPaletteManager.ColorPalette.GetTokenStyle(i).GetToken(j));
                                    tmpColor = EditorGUILayout.ColorField(tmpColor);
                                    if (ColorPaletteManager.ColorPalette.GetTokenStyle(i).GetToken(j).Editable)
                                    {
                                       
                                        ColorPaletteManager.ColorPalette.UpdateToken(ColorPaletteManager.ColorPalette.GetTokenStyle(i).GetToken(j), tmpColor);
                                    }
    
                                    EditorGUILayout.Space(spacing * 2);
                                    EditorGUILayout.EndHorizontal();
                                }
                                EditorGUILayout.Space(spacing * 2);
                            }
                            EditorGUILayout.Space(spacing);
                        }
                    }
                }
            }
            DrawUILine(Color.grey);
        }

        private void Apply()
        {
            ColorPaletteManager.Save();
            ColorPaletteManager.UpdateComponents();
            EditorWindow view = EditorWindow.GetWindow<SceneView>();
            view.Repaint();
        }

        public static void DrawUILine(Color color, int thickness = 1, int padding = 50)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }
    }
}
