using UnityEngine;

namespace HHG.ThemeSystem.Runtime
{
    [System.Serializable]
    public abstract class NamedStyle
    {
        public StyleNameAsset Name => name;

        public abstract Style StyleWeak { get; }

        [SerializeField] protected StyleNameAsset name;

        public abstract void Initialize(ThemeAsset theme);

        public static implicit operator Style(NamedStyle style) => style.StyleWeak;
    }

    [System.Serializable]
    public class NamedStyle<T> : NamedStyle where T : Style
    {
        public override Style StyleWeak => style;

        public T Style => style;

        [SerializeField] protected T style;

        public override void Initialize(ThemeAsset theme)
        {
            style.Initialize(theme);
        }
    }
}