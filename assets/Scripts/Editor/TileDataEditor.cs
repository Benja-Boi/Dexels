using Core.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TileData))]
    public class TileDataEditor : UnityEditor.Editor
    {
        private SerializedProperty _colorPoolProperty;
        private SerializedProperty _gridSizeProperty;
        private Color _selectedColor;

        private void OnEnable()
        {
            _colorPoolProperty = serializedObject.FindProperty("colorPool");
            _gridSizeProperty = serializedObject.FindProperty("gridSize");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            TileData tileData = (TileData)target;

            // Draw the basic fields
            EditorGUILayout.PropertyField(_colorPoolProperty);
            EditorGUILayout.PropertyField(_gridSizeProperty);

            DrawColorGrid(tileData);

            DrawColorPalette(tileData);

            // Add a button for randomizing colors
            if (GUILayout.Button("Randomize Colors"))
            {
                RandomizeTileColors(tileData);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawColorGrid(TileData tileData)
        {
            GUI.backgroundColor = Color.white; // Reset background color
            // Draw the grid
            int gridSize = tileData.gridSize;
            for (int y = 0; y < gridSize; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < gridSize; x++)
                {
                    int index = y * gridSize + x;
                    if (index < tileData.colors.Length)
                    {
                        GUI.backgroundColor = tileData.colors[index];
                        if (GUILayout.Button("", GUILayout.Width(50), GUILayout.Height(50)))
                        {
                            tileData.colors[index] = _selectedColor;
                            EditorUtility.SetDirty(target); // Mark object as dirty to ensure changes are saved
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawColorPalette(TileData tileData)
        {
            // Ensure tileData has a valid color pool reference
            if (tileData.colorPool != null)
            {
                GUI.backgroundColor = Color.white; // Reset background color
                EditorGUILayout.LabelField("Color Palette:");
                EditorGUILayout.BeginHorizontal();
                foreach (var color in tileData.colorPool.colors)
                {
                    GUI.backgroundColor = color;
                    if (GUILayout.Button("", GUILayout.Width(25), GUILayout.Height(25)))
                    {
                        _selectedColor = color;
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.HelpBox("Color Pool is not set.", MessageType.Warning);
            }
        }

        private void RandomizeTileColors(TileData tileData)
        {
            if (tileData.colorPool != null && tileData.colorPool.colors is { Length: > 0 })
            {
                Undo.RecordObject(tileData, "Randomize Colors"); // To allow undoing the change

                for (int i = 0; i < tileData.colors.Length; i++)
                {
                    tileData.colors[i] = tileData.colorPool.colors[Random.Range(0, tileData.colorPool.colors.Length)];
                }

                EditorUtility.SetDirty(tileData); // Mark the TileData as dirty to ensure changes are saved
            }
        }
    }
}