﻿using TMPro;
using UnityEngine;

namespace Pancake.Scriptable
{
    /// <summary>
    /// Binds a string variable to an input field
    /// </summary>
    [AddComponentMenu("Scriptable/Bindings/BindInputField")]
    [RequireComponent(typeof(TMP_InputField))]
    public class BindInputField : CacheGameComponent<TMP_InputField>
    {
        [SerializeField] private StringVariable stringVariable;

        protected override void Awake()
        {
            base.Awake();
            component.text = stringVariable;
            component.onValueChanged.AddListener(SetBoundVariable);
        }

        private void OnDestroy() => component.onValueChanged.RemoveListener(SetBoundVariable);

        private void SetBoundVariable(string value) => stringVariable.Value = value;
    }
}