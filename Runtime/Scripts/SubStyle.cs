using UnityEngine;

namespace HHG.ThemeSystem.Runtime
{
    public enum StyleReferenceMode
    {
        Reference,
        Embedded,
    }

    [System.Serializable]
    public class SubStyle<T> where T : Style
    {
        [SerializeField] private StyleReferenceMode mode;
        [SerializeField, ShowIf(nameof(mode), StyleReferenceMode.Reference)] private StyleNameAsset name;
        [SerializeField, ShowIf(nameof(mode), StyleReferenceMode.Embedded)] private T style;

        public void ApplyTheme(ThemeAsset theme, Component component)
        {
            switch (mode)
            {
                case StyleReferenceMode.Reference: 
                    if (name)
                    {
                        name.ApplyTheme(theme, component);
                    }
                    break;
                case StyleReferenceMode.Embedded:
                    style.Initialize(theme);
                    style.ApplyTheme(component); 
                    break;
            }
        }
    }
}