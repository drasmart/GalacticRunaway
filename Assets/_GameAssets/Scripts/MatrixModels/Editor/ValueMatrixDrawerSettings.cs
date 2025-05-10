using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

#nullable enable

namespace MatrixModels.Editor
{
    [FilePath(
        "UserSettings/drasmart.net/" + nameof(ValueMatrixDrawerSettings) + ".data", 
        FilePathAttribute.Location.ProjectFolder)]
    public class ValueMatrixDrawerSettings : ScriptableSingleton<ValueMatrixDrawerSettings>
    {
        [SerializeField] private List<KeyedPrefs> allPrefs = new();
        
        public DisplayPrefsBox this[SerializedProperty property]
        {
            get
            {
                var entry = allPrefs.FirstOrDefault(x =>
                    (x.className == property.serializedObject?.GetType().FullName)
                    && (x.propertyName == property.name)
                );
                if (entry is null)
                {
                    entry = new KeyedPrefs
                    {
                        className = property.serializedObject?.GetType().FullName,
                        propertyName = property.name,
                    };
                    allPrefs.Add(entry);
                }
                return new DisplayPrefsBox(entry);
            }
        }
        
        [Serializable]
        public struct DisplayPrefs : IEquatable<DisplayPrefs>
        {
            public bool showLabels;
            public bool useSpacer;

            public bool Equals(DisplayPrefs other)
            {
                return (showLabels == other.showLabels) && (useSpacer == other.useSpacer);
            }
        }

        [Serializable]
        internal class KeyedPrefs
        {
            public string? className;
            public string propertyName = "";
            public DisplayPrefs displayPrefs;
        }

        public class DisplayPrefsBox : IDisposable
        {
            private readonly KeyedPrefs prefs;
            private DisplayPrefs? newPrefs;

            internal DisplayPrefsBox(KeyedPrefs prefs)
            {
                this.prefs = prefs;
            }

            public DisplayPrefs Value
            {
                get => newPrefs ?? prefs.displayPrefs;
                set => newPrefs = value;
            }
            
            public void Dispose()
            {
                if (newPrefs is {} lastPrefs && !lastPrefs.Equals(prefs.displayPrefs))
                {
                    prefs.displayPrefs = lastPrefs;
                    instance.Save(true);
                }
            }
        }
    }
}
