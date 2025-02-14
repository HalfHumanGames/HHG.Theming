using HHG.Common.Runtime;
using UnityEngine;

namespace HHG.ThemeSystem.Runtime
{
    public class ThemeElement : MonoBehaviour
    {
        public StyleNameAsset Id => id;

        [SerializeField] private StyleNameAsset id;

        private Theme theme;

        private void Awake()
        {
            theme = GetComponentInParent<Theme>(true);

            if (theme)
            {
                theme.Register(this);
            }
        }

        public void ApplyTheme(ThemeAsset theme)
        {
            if (id == null)
            {
                Debug.LogError("Id cannot be null.", gameObject);
                return;
            }

            if (theme == null)
            {
                Debug.LogError("Theme cannot be null.", theme);
                return;
            }

            if (!theme.TryGetStyle(id, out Style style))
            {
                Debug.LogError($"Id '{id.Value}' not found in Theme: {theme}", theme);
                return;
            }

            style.ApplyTheme(gameObject);
        }

        private void OnDestroy()
        {
            if (theme)
            {
                theme.Unregister(this);
            }
        }

        private void OnValidate()
        {
            if (this.TryGetComponentInParent(out Theme theme))
            {
                theme.ApplyTheme();
            }
        }
    }
}