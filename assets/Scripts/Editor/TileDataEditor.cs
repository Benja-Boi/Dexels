using System;
using Core.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TileData))]
    public class TileDataEditor : UnityEditor.Editor
    {
        private TileData _tileData;
        private int _selectedColorIndex;

        private void OnEnable()
        {
            _tileData = (TileData)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // 1. Slot for ColorMap
            EditorGUILayout.PropertyField(serializedObject.FindProperty("colorMap"), new GUIContent("Color Map"));
            if (_tileData.ColorMap == null)
            {
                EditorGUILayout.HelpBox("Please assign a Color Map.", MessageType.Warning);
                //return;
            }

            // 2. Grid Display
            DrawGrid();

            // 3. Color Palette
            DrawColorPalette();

            // 4. Grid Size Options
            DrawGridSizeOptions();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawGrid()
        {
            EditorGUILayout.LabelField("Grid", EditorStyles.boldLabel);

            var gridData = _tileData.ColorGridData;
            for (var i = 0; i < gridData.GridSize; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (var j = 0; j < gridData.GridSize; j++)
                {
                    DrawGridCell(i, j);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawGridCell(int x, int y)
        {
            var gridData = _tileData.ColorGridData;
            var color = _tileData.GetColor(x, y);
            if (GUILayout.Button("", GUILayout.Width(40), GUILayout.Height(40)))
            {
                gridData.SetColor(x, y, _selectedColorIndex);
                EditorUtility.SetDirty(target);
            }
            EditorGUI.DrawRect(GUILayoutUtility.GetLastRect(), color);
        }

        private void DrawColorPalette()
        {
            // Ensure tileData has a valid color pool reference
            if (_tileData.ColorMap != null)
            {
                GUI.backgroundColor = Color.white; // Reset background color
                EditorGUILayout.LabelField("Color Palette:");
                EditorGUILayout.BeginHorizontal();
                foreach (var color in _tileData.ColorMap.Colors())
                {
                    GUI.backgroundColor = color;
                    if (GUILayout.Button("", GUILayout.Width(25), GUILayout.Height(25)))
                    {
                        _selectedColorIndex = _tileData.ColorMap.GetKey(color);
                    }
                }

                EditorGUILayout.EndHorizontal();
                GUI.backgroundColor = Color.white; // Reset background color
            }
            else
            {
                EditorGUILayout.HelpBox("Color Pool is not set.", MessageType.Warning);
            }
        }


        private void DrawGridSizeOptions()
        {
            EditorGUILayout.LabelField("Grid Size Options", EditorStyles.boldLabel);
    
            // Define the grid size options and their corresponding labels
            int[] gridSizeOptions = { 3, 5, 7 };
            string[] gridSizeLabels = { "3x3", "5x5", "7x7" };

            // Find the index of the current grid size in the options array
            var currentGridSizeIndex = Array.IndexOf(gridSizeOptions, _tileData.GridSize);
            if (currentGridSizeIndex < 0) currentGridSizeIndex = 0; // Default to the first option if not found

            // Display the radio buttons and get the selected index
            var selectedGridSizeIndex = GUILayout.Toolbar(currentGridSizeIndex, gridSizeLabels);

            // Check if the grid size has been changed
            if (selectedGridSizeIndex == currentGridSizeIndex) return;
            _tileData.GridSize = gridSizeOptions[selectedGridSizeIndex];
            EditorUtility.SetDirty(target);
        }

    }
}
