using Pancake.Localization;
using Pancake.Scriptable;

namespace Pancake.SceneFlow
{
    using UnityEngine;

    /// <summary>
    /// Display Text Current Level
    /// </summary>
    public class CurrentLevelComponent : GameComponent
    {
        [SerializeField] private IntVariable currentLevel;
        [SerializeField] private LocaleTextComponent localeText;
        [SerializeField] private bool subscribe;

        protected void OnEnable()
        {
            OnValueChanged(currentLevel.Value);
            if (subscribe) currentLevel.OnValueChanged += OnValueChanged;
        }

        private void OnValueChanged(int level) { localeText.UpdateArgs($"{level + 1}"); }

        protected void OnDisable()
        {
            if (subscribe) currentLevel.OnValueChanged -= OnValueChanged;
        }
    }
}