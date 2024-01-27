using System;
using System.Collections.Generic;
using Pancake.Greenery;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace Pancake.GreeneryEditor
{
    [EditorTool("Greenery")]
    public class GreeneryToolEditor : EditorTool
    {
        //Panels
        [SerializeField] private GreeneryManagerModule managerModule;
        [SerializeField] private GreeneryScatteringModule scatteringModule;
        [SerializeField] private GreeneryItemsModule itemsModule;
        [SerializeField] private GreeneryPointControlsModule pointControlsModule;

        private const float WINDOW_WIDTH = 260;
        private const float WINDOW_HEIGHT_PADDING = 26;

        public override GUIContent toolbarIcon => EditorGUIUtility.IconContent("d_TreeEditor.Leaf On");

        public Action<Rect> onToolHandles;
        public Action onGUI;

        private void OnEnable()
        {
            managerModule = new GreeneryManagerModule();
            scatteringModule = new GreeneryScatteringModule();
            itemsModule = new GreeneryItemsModule();
            pointControlsModule = new GreeneryPointControlsModule();

            managerModule.Initialize(this);
            itemsModule.Initialize(this);
            scatteringModule.Initialize(this);
            pointControlsModule.Initialize(this);
        }

        private void OnDisable()
        {
            managerModule.Release();
            itemsModule.Release();
            scatteringModule.Release();
            pointControlsModule.Release();
        }

        public override void OnToolGUI(EditorWindow window)
        {
            Rect sceneViewRect = SceneView.lastActiveSceneView.position;
            bool hasSelectedItem = itemsModule.itemsModuleSettings.selectedItems.Count > 0;
            //Get height of all modules
            float windowHeight = managerModule.GetHeight() + itemsModule.GetHeight();
            windowHeight += hasSelectedItem ? scatteringModule.GetHeight() + pointControlsModule.GetHeight() : 0;

            //Editor rect
            Rect toolGUIRect = new Rect(sceneViewRect.width - WINDOW_WIDTH, sceneViewRect.height - windowHeight - WINDOW_HEIGHT_PADDING, WINDOW_WIDTH, windowHeight);
            //Drag and drop area to add Greenery Items
            itemsModule.ItemDropArea(toolGUIRect);

            onToolHandles?.Invoke(toolGUIRect);

            Handles.BeginGUI();
            EditorGUI.DrawRect(toolGUIRect, new Color(0.1f, 0.1f, 0.1f, toolGUIRect.Contains(Event.current.mousePosition) ? 0.7f : 0.3f));
            var r = new Rect(toolGUIRect.x, toolGUIRect.y + 6, toolGUIRect.width, toolGUIRect.height);
            GUILayout.BeginArea(r);

            onGUI?.Invoke();

            GUILayout.EndArea();
            Handles.EndGUI();

            window.Repaint();
        }

        public List<GreeneryItem> GetLoadedItems() { return itemsModule.itemsModuleSettings.greeneryItems; }

        public List<GreeneryItem> GetSelectedItems() { return itemsModule.itemsModuleSettings.selectedItems; }

        public GreeneryScatteringModule.ScaterringModuleSettings GetScatteringModuleSettings() { return scatteringModule.scatteringModuleSettings; }
    }
}