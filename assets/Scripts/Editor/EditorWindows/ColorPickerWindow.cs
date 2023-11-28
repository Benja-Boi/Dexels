using Core.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Editor.EditorWindows
{
    public class ColorPickerWindow : EditorWindow
    {
        private TileData _tileData;
        private int _selectedIndex;

        public static void Open(TileData tileData, int index)
        {
            var window = GetWindow<ColorPickerWindow>("Color Picker");
            window._tileData = tileData;
            window._selectedIndex = index;
        }

        private void OnGUI()
        {
            // Draw color buttons from the color pool
            foreach (var color in _tileData.colorPool.colors)
            {
                if (GUILayout.Button("", GUILayout.Width(50), GUILayout.Height(50)))
                {
                    _tileData.colors[_selectedIndex] = color;
                    Close(); // Close the window after color selection
                }

                Rect lastRect = GUILayoutUtility.GetLastRect();
                EditorGUI.DrawRect(new Rect(lastRect.x, lastRect.y, 50, 50), color);
            }
        }
    }
}