﻿using Pancake;
using Pancake.ExLibEditor;
using UnityEditor;

namespace PancakeEditor
{
    [CustomEditor(typeof(HeartEditorSettings), true)]
    public class HeartEditorSettingDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            string[] propertiesToHide = {"m_Script"};
            Uniform.DrawInspectorExcept(serializedObject, propertiesToHide);
        }
    }
}