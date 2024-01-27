﻿using System.Linq;
using Pancake.ExLibEditor;
using Pancake.Scriptable;
using UnityEditor;
using UnityEngine;

namespace Pancake.ScriptableEditor
{
    public class RenamePopUpWindow : PopupWindowContent
    {
        private string _newName;
        private readonly Rect _position;
        private readonly Vector2 _dimensions = new Vector2(300, 170);
        private readonly ScriptableBase _scriptableBase;

        public override Vector2 GetWindowSize() => _dimensions;

        public RenamePopUpWindow(Rect origin, ScriptableBase scriptableBase)
        {
            _position = origin;
            _scriptableBase = scriptableBase;
            _newName = _scriptableBase.name;
        }

        public override void OnGUI(Rect rect)
        {
            editorWindow.position = Uniform.CenterInWindow(editorWindow.position, _position);

            Uniform.DrawHeader("Rename");
            GUILayout.Space(12);

            _newName = EditorGUILayout.TextField(_newName, EditorStyles.textField);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Rename", GUILayout.MaxHeight(ScriptableEditorSetting.BUTTON_HEIGHT)))
            {
                EditorCreator.RenameAsset(_scriptableBase, _newName);
                editorWindow.Close();
            }

            if (GUILayout.Button("Cancel", GUILayout.MaxHeight(ScriptableEditorSetting.BUTTON_HEIGHT))) editorWindow.Close();
        }
    }
}