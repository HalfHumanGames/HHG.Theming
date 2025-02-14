using HHG.Common.Runtime;
using UnityEngine;

namespace HHG.ThemeSystem.Runtime
{
    [CreateAssetMenu(fileName = "Style Name", menuName = "HHG/Theme System/Style Name")]
    public class StyleNameAsset : StringNameAsset
    {
        public void ApplyTheme(ThemeAsset theme, Component component)
        {
            if (theme.TryGetStyle(this, out Style style))
            {
                style.ApplyTheme(component);
            }
        }
    }
} 