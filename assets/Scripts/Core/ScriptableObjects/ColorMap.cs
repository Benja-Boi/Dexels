using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ColorMap", menuName = "Color Mapping/ColorMap")]
    
    [Serializable]
    public class ColorMap : ScriptableObject
    {
        public List<ColorMapEntry> entries = new();

        public void AddEntry(int key, Color value)
        {
            // @TODO: Check for duplicate keys here
            entries.Add(new ColorMapEntry(key, value));
        }
 
        public Color GetColor(int key)
        {
            foreach (var entry in entries.Where(entry => entry.key == key))
            {
                return entry.value;
            }

            return default; // Return a default value if key not found
        }

        public int GetKey(Color color)
        {
            // Return the first key that matches the color
            return (from entry in entries where entry.value == color select entry.key).FirstOrDefault();
        }

        public List<(int, Color)> Pairs()
        {
            // Return all pairs of index and color in the map
            return entries.Select(entry => (entry.key, entry.value)).ToList();
        }

        public List<Color> Colors()
        {
            // Return all unique colors in the map
            return entries.Select(entry => entry.value).Distinct().ToList();
        }

        public Color GetRandomColor()
        {
            // Return a random color from the map
            return entries[Random.Range(0, entries.Count)].value;
        }

        public int GetRandomColorInt()
        {
            // Return a random color from the map
            return entries[Random.Range(0, entries.Count)].key;
        }

        public string[] GetColorTextures()
        {
            throw new NotImplementedException();
        }
    }


    [Serializable]
    public class ColorMapEntry
    {
        public int key;
        public Color value;

        public ColorMapEntry(int key, Color value)
        {
            this.key = key;
            this.value = value;
        }
    }
}