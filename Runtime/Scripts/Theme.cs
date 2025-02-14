using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace HHG.ThemeSystem.Runtime
{
    public class Theme : MonoBehaviour
    {
        private static List<Theme> themes = new List<Theme>();

        public ThemeAsset Current => current;

        [SerializeField] private ThemeAsset current;

        private List<ThemeElement> elements = new List<ThemeElement>();

        private void Awake()
        {
            themes.Add(this);
        }

        public void Register(ThemeElement element)
        {
            if (!elements.Contains(element))
            {
                element.ApplyTheme(current);
                elements.Add(element);
            }
        }

        public void Unregister(ThemeElement element)
        {
            elements.Remove(element);
        }

        public void ApplyTheme(ThemeAsset theme, Func<ThemeElement, bool> filter = null)
        {
            current = theme;
            ApplyTheme(filter);
        }

        public void ApplyTheme(Func<ThemeElement, bool> filter = null)
        {
#if UNITY_EDITOR
            if (EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
#endif

            if (current == null)
            {
                Debug.LogError("Theme cannot be null.", this);
                return;
            }

            if (!Application.isPlaying)
            {
                current.ForceRebuildStylesMap();

                elements.Clear();
                elements.AddRange(GetComponentsInChildren<ThemeElement>(true));
            }

            foreach (ThemeElement element in elements)
            {
                if (element.Id == null || (filter != null && !filter(element)))
                {
                    continue;
                }

                element.ApplyTheme(current);
            }
        }

        public static void ApplyAllThemes(Func<Theme, bool> filter = null)
        {
#if UNITY_EDITOR
            if (EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
#endif

            if (!Application.isPlaying)
            {
                themes.AddRange(FindObjectsByType<Theme>(FindObjectsInactive.Include, FindObjectsSortMode.None));
            }

            foreach (Theme theme in themes)
            {
                if (theme.Current == null || (filter != null && !filter(theme)))
                {
                    continue;
                }

                theme.ApplyTheme();
            }
        }

        private void OnDestroy()
        {
            themes.Remove(this);
        }

        private void OnValidate()
        {
            if (current)
            {
                ApplyTheme();
            }
        }
    }
}