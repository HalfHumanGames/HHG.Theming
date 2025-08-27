using System.Collections.Generic;
using UnityEngine;

namespace HHG.ThemeSystem.Runtime
{
    [CreateAssetMenu(fileName = "Theme", menuName = "HHG/Theme System/Theme")]
    public class ThemeAsset : ScriptableObject
    {
        [SerializeField] private ThemeAsset baseTheme;
        [SerializeField, ReorderableList, LabelByChild("name")] private List<NamedStyle<ButtonStyle>> buttons = new List<NamedStyle<ButtonStyle>>();
        [SerializeField, ReorderableList, LabelByChild("name")] private List<NamedStyle<DropdownStyle>> dropdowns = new List<NamedStyle<DropdownStyle>>();
        [SerializeField, ReorderableList, LabelByChild("name")] private List<NamedStyle<ImageStyle>> images = new List<NamedStyle<ImageStyle>>();
        [SerializeField, ReorderableList, LabelByChild("name")] private List<NamedStyle<InputFieldStyle>> inputFields = new List<NamedStyle<InputFieldStyle>>();
        [SerializeField, ReorderableList, LabelByChild("name")] private List<NamedStyle<LabelStyle>> labels = new List<NamedStyle<LabelStyle>>();
        [SerializeField, ReorderableList, LabelByChild("name")] private List<NamedStyle<ScrollbarStyle>> scrollbars = new List<NamedStyle<ScrollbarStyle>>();
        [SerializeField, ReorderableList, LabelByChild("name")] private List<NamedStyle<ScrollViewStyle>> scrollViews = new List<NamedStyle<ScrollViewStyle>>();
        [SerializeField, ReorderableList, LabelByChild("name")] private List<NamedStyle<SliderStyle>> sliders = new List<NamedStyle<SliderStyle>>();
        [SerializeField, ReorderableList, LabelByChild("name")] private List<NamedStyle<ToggleStyle>> toggles = new List<NamedStyle<ToggleStyle>>();
        [SerializeField, ReorderableList, LabelByChild("name")] private List<NamedStyle<TransitionStyle>> transitions = new List<NamedStyle<TransitionStyle>>();

        private HashSet<ThemeAsset> themes = new HashSet<ThemeAsset>();
        private List<NamedStyle> styles = new List<NamedStyle>();
        private Dictionary<StyleNameAsset, Style> styleMap = new Dictionary<StyleNameAsset, Style>();

        public Style GetStyle(StyleNameAsset id)
        {
            return GetStylesMap().TryGetValue(id, out Style entry) ? entry : null;
        }

        public bool TryGetStyle(StyleNameAsset id, out Style entry)
        {
            return GetStylesMap().TryGetValue(id, out entry);
        }

        public IReadOnlyDictionary<StyleNameAsset, Style> GetStylesMap()
        {
            if (styleMap.Count == 0)
            {
                ForceRebuildStylesMap();
            }

            return styleMap;
        }

        public void ForceRebuildStylesMap()
        {
            themes.Clear();
            styles.Clear();
            styleMap.Clear();

            GetAllStyles(themes, styles);

            for (int i = 0; i < styles.Count; i++)
            {
                NamedStyle style = styles[i];

                if (style.Name == null)
                {
                    continue;
                }

                style.Initialize(this);

                styleMap[style.Name] = style;
            }
        }

        private void GetAllStyles(HashSet<ThemeAsset> themes, List<NamedStyle> styles)
        {
            // Track themes to prevent stack overflow exceptions
            // caused from cyclic base theme referencing
            if (themes.Contains(this))
            {
                return;
            }

            themes.Add(this);

            // Add the base theme first so this
            // theme overrides any shared styles
            if (baseTheme)
            {
                baseTheme.GetAllStyles(themes, styles);
            }

            styles.AddRange(buttons);
            styles.AddRange(dropdowns);
            styles.AddRange(images);
            styles.AddRange(inputFields);
            styles.AddRange(labels);
            styles.AddRange(scrollbars);
            styles.AddRange(scrollViews);
            styles.AddRange(sliders);
            styles.AddRange(toggles);
            styles.AddRange(transitions);
        }

        private void OnValidate()
        {
            ForceRebuildStylesMap();

            Theme.ApplyAllThemes(t => t.Current == this);
        }
    }
}