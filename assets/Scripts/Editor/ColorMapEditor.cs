using Core.Tile_Structure.Scriptable_Objects;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ColorMap), true), CanEditMultipleObjects]
    public class ColorMapEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ColorMap colorMap = (ColorMap)target;

            // Check for changes and start a horizontal layout
            EditorGUI.BeginChangeCheck();
            
            for (int i = 0; i < colorMap.entries.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                // Delete button
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    colorMap.entries.RemoveAt(i);
                    break; // Exit the loop to prevent invalid index access after removal
                }

                // Draw key field
                colorMap.entries[i].key = EditorGUILayout.IntField(colorMap.entries[i].key, GUILayout.MaxWidth(50));

                // Draw color field
                colorMap.entries[i].value = EditorGUILayout.ColorField(colorMap.entries[i].value);

                EditorGUILayout.EndHorizontal();
            }

            // Handle adding a new entry
            if (GUILayout.Button("Add New Entry"))
            {
                colorMap.entries.Add(new ColorMapEntry(colorMap.entries.Count, Color.white));
            }

            if (GUILayout.Button("Reset Keys"))
            {
                for (int i = 0; i < colorMap.entries.Count; i++)
                {
                    colorMap.entries[i].key = i;
                }
            }
            
            // Apply changes
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(colorMap);
            }
        }
    }
}