using HHG.Common.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static HHG.ThemeSystem.Runtime.ToggleStyle;

namespace HHG.ThemeSystem.Runtime
{
    [System.Serializable]
    public abstract class Style
    {
        protected ThemeAsset theme;

        public void Initialize(ThemeAsset newTheme)
        {
            theme = newTheme;
        }

        public abstract void ApplyTheme(GameObject gameObject);
        public abstract void ApplyTheme(Component component);
    }

    [System.Serializable]
    public abstract class Style<T> : Style where T : Component
    {
        public override void ApplyTheme(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out T component))
            {
                ApplyTheme(component);
            }
        }

        public override void ApplyTheme(Component component)
        {
            if (component is T typed)
            {
                ApplyTheme(typed);
            }
        }

        public abstract void ApplyTheme(T component);
    }

    [System.Serializable]
    public abstract class SelectableStyle<T> : Style<T> where T : Selectable
    {
        [SerializeField] protected SubStyle<ImageStyle> image;
        [SerializeField] protected SubStyle<TransitionStyle> transition;

        public override void ApplyTheme(T selectable)
        {
            if (selectable.image)
            {
                image.ApplyTheme(theme, selectable.image);
            }

            transition.ApplyTheme(theme, selectable);
        }
    }

    [System.Serializable]
    public class ButtonStyle : SelectableStyle<Button>
    {
        [SerializeField] protected SubStyle<LabelStyle> label;

        public override void ApplyTheme(Button selectable)
        {
            base.ApplyTheme(selectable);

            if (selectable.TryGetComponentInChildren(out TMP_Text text))
            {
                label.ApplyTheme(theme, text);
            }
        }
    }

    [System.Serializable]
    public class DropdownStyle : SelectableStyle<TMP_Dropdown>
    {
        [SerializeField] protected SubStyle<ImageStyle> arrowImage;
        [SerializeField] protected SubStyle<LabelStyle> captionLabel;
        [SerializeField] protected SubStyle<LabelStyle> itemLabel;
        [SerializeField] protected SubStyle<ToggleStyle> itemToggle;
        [SerializeField] protected SubStyle<ScrollViewStyle> scrollView;

        public override void ApplyTheme(TMP_Dropdown dropdown)
        {
            base.ApplyTheme(dropdown);

            if (dropdown.transform.Find("Arrow") is Transform transform && transform.TryGetComponent(out Image image))
            {
                arrowImage.ApplyTheme(theme, image);
            }

            if (dropdown.captionText)
            {
                captionLabel.ApplyTheme(theme, dropdown.captionText);
            }

            if (dropdown.itemText)
            {
                itemLabel.ApplyTheme(theme, dropdown.itemText);
            }

            if (dropdown.template && dropdown.template.TryGetComponentInChildren(out Toggle toggle))
            {
                itemToggle.ApplyTheme(theme, toggle);
            }

            if (dropdown.template && dropdown.template.TryGetComponentInChildren(out ScrollRect scrollRect))
            {
                scrollView.ApplyTheme(theme, scrollRect);
            }
        }
    }

    [System.Serializable]
    public class ImageStyle : Style<Image>
    {
        [SerializeField] protected Sprite sprite;
        [SerializeField] protected Material material;
        [SerializeField] protected Color color = Color.white;

        public override void ApplyTheme(Image image)
        {
            image.sprite = sprite;
            image.material = material;
            image.color = color;
        }
    }

    [System.Serializable]
    public class InputFieldStyle : SelectableStyle<TMP_InputField>
    {
        private static readonly Color defaultCaretColor = new Color(50f, 50f, 50f, 255f);
        private static readonly Color defaultSelectionColor = new Color(168f, 206f, 255f, 192f);

        [SerializeField] protected SubStyle<LabelStyle> inputLabel;
        [SerializeField] protected SubStyle<LabelStyle> placeholderLabel;
        [SerializeField] protected SubStyle<ScrollbarStyle> verticalScrollbar;
        [SerializeField] protected bool customCaretColor;
        [SerializeField, ShowIf(nameof(customCaretColor), true)] protected Color caretColor = defaultCaretColor;
        [SerializeField] protected Color selectionColor = defaultSelectionColor;

        public override void ApplyTheme(TMP_InputField inputField)
        {
            base.ApplyTheme(inputField);

            inputField.customCaretColor = customCaretColor;
            inputField.caretColor = caretColor;
            inputField.selectionColor = selectionColor;

            if (inputField.textComponent)
            {
                inputLabel.ApplyTheme(theme, inputField.textComponent);
            }

            if (inputField.placeholder && inputField.placeholder is TMP_Text placeholder)
            {
                placeholderLabel.ApplyTheme(theme, placeholder);
            }

            if (inputField.verticalScrollbar)
            {
                verticalScrollbar.ApplyTheme(theme, inputField.verticalScrollbar);
            }
        }
    }

    [System.Serializable]
    public class LabelStyle : Style<TMP_Text>
    {
        [SerializeField] protected TMP_FontAsset font;
        [SerializeField] protected Material overrideMaterial;
        [SerializeField] protected FontStyles fontStyle;
        [SerializeField] protected int fontSize;
        [SerializeField] protected bool enableAutoSizing;
        [SerializeField, ShowIf(nameof(enableAutoSizing), true)] protected float fontSizeMin;
        [SerializeField, ShowIf(nameof(enableAutoSizing), true)] protected float fontSizeMax;
        [SerializeField] protected Color color = Color.white;
        [SerializeField] protected bool enableGradient;
        [SerializeField, ShowIf(nameof(enableGradient), true)] protected TMP_ColorGradient colorGarientPreset;
        [SerializeField, ShowIf(nameof(enableGradient), true)] protected VertexGradient colorGarient;
        [SerializeField, ShowIf(nameof(enableGradient), true)] protected bool overrideTags;

        public override void ApplyTheme(TMP_Text label)
        {
            label.font = font;
            label.fontSharedMaterial = overrideMaterial != null ? overrideMaterial : font.material;
            label.fontStyle = fontStyle;
            label.fontSize = fontSize;
            label.enableAutoSizing = enableAutoSizing;
            label.fontSizeMin = fontSizeMin;
            label.fontSizeMax = fontSizeMax;
            label.color = color;
            label.enableVertexGradient = enableGradient;
            label.colorGradientPreset = colorGarientPreset;
            label.colorGradient = colorGarient;
            label.overrideColorTags = overrideTags;
        }
    }

    [System.Serializable]
    public class ScrollbarStyle : SelectableStyle<Scrollbar>
    {
        [SerializeField] protected SubStyle<ImageStyle> backgroundImage;

        public override void ApplyTheme(Scrollbar scrollbar)
        {
            base.ApplyTheme(scrollbar);

            if (scrollbar.TryGetComponent(out Image image))
            {
                backgroundImage.ApplyTheme(theme, image);
            }
        }
    }

    [System.Serializable]
    public class ScrollViewStyle : Style<ScrollRect>
    {
        [SerializeField] protected SubStyle<ImageStyle> backgroundImage;
        [SerializeField] protected SubStyle<ScrollbarStyle> horizontalScrollbar;
        [SerializeField] protected SubStyle<ScrollbarStyle> verticalScrollbar;

        public override void ApplyTheme(ScrollRect scrollRect)
        {
            if (scrollRect.TryGetComponent(out Image image))
            {
                backgroundImage.ApplyTheme(theme, image);
            }

            if (scrollRect.horizontalScrollbar)
            {
                horizontalScrollbar.ApplyTheme(theme, scrollRect.horizontalScrollbar);
            }

            if (scrollRect.verticalScrollbar)
            {
                verticalScrollbar.ApplyTheme(theme, scrollRect.verticalScrollbar);
            }
        }
    }

    [System.Serializable]
    public class SliderStyle : SelectableStyle<Slider>
    {
        [SerializeField] protected SubStyle<ImageStyle> background;
        [SerializeField] protected SubStyle<ImageStyle> fill;

        public override void ApplyTheme(Slider slider)
        {
            base.ApplyTheme(slider);

            if (slider.transform.Find("Background") is Transform backgroundTransform && backgroundTransform.TryGetComponent(out Image backgroundImage))
            {
                background.ApplyTheme(theme, backgroundImage);
            }

            if (slider.transform.Find("Fill Area/Fill") is Transform fillTransform && fillTransform.TryGetComponent(out Image fillImage))
            {
                fill.ApplyTheme(theme, fillImage);
            }
        }
    }

    [System.Serializable]
    public class ToggleStyle : SelectableStyle<Toggle>
    {
        [SerializeField] protected SubStyle<ImageStyle> checkmark;
        [SerializeField] protected SubStyle<LabelStyle> label;

        public override void ApplyTheme(Toggle toggle)
        {
            base.ApplyTheme(toggle);

            if (toggle.graphic is Image image)
            {
                checkmark.ApplyTheme(theme, image);
            }

            if (toggle.TryGetComponentInChildren(out TMP_Text text))
            {
                label.ApplyTheme(theme, text);
            }
        }

        [System.Serializable]
        public class TransitionStyle : Style<Selectable>
        {
            // The ShowIf attribute does not work for subclasses unless the referenced source handle field is protected
            [SerializeField] protected Selectable.Transition transition = Selectable.Transition.ColorTint;
            [SerializeField, ShowIf(nameof(transition), Selectable.Transition.ColorTint)] protected ColorBlock colors = ColorBlock.defaultColorBlock;
            [SerializeField, ShowIf(nameof(transition), Selectable.Transition.SpriteSwap)] protected SpriteState spriteState;
            [SerializeField, ShowIf(nameof(transition), Selectable.Transition.Animation)] protected AnimationTriggers animationTriggers;

            public override void ApplyTheme(Selectable selectable)
            {
                selectable.transition = transition;
                selectable.colors = colors;
                selectable.spriteState = spriteState;
                selectable.animationTriggers = animationTriggers;
            }
        }
    }
}