using Core.ScriptableObjects;
using Editor.EditorWindows;
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

            // Ensure tileData has a valid color pool reference
            if (tileData.colorPool == null)
            {
                EditorGUILayout.HelpBox("Color Pool is not set.", MessageType.Warning);
            }

            // Draw the default inspector for other fields
            // DrawDefaultInspector();

            EditorGUILayout.PropertyField(_colorPoolProperty);
            EditorGUILayout.PropertyField(_gridSizeProperty);

            // Draw the grid
            int gridSize = tileData.gridSize;
            for (int y = 0; y < gridSize; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < gridSize; x++)
                {
                    DrawCell(x, y, tileData);
                }

                EditorGUILayout.EndHorizontal();
            }
            
            // Add a button for randomizing colors
            if (GUILayout.Button("Randomize Colors"))
            {
                RandomizeTileColors(tileData);
            }

            serializedObject.ApplyModifiedProperties();
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

        private void DrawCell(int x, int y, TileData tileData)
        {
            int index = y * tileData.gridSize + x;
            var color = tileData.colors[index];

            // Set button background color
            GUI.backgroundColor = color;

            // Create a button for each cell
            if (GUILayout.Button("", GUILayout.Width(50), GUILayout.Height(50)))
            {
                // Open color picker when cell is clicked
                ColorPickerWindow.Open(tileData, index);
            }
        }
    }
}