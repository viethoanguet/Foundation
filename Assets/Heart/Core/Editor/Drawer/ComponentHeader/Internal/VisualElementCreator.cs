﻿using System;
using System.Collections.Generic;
using Pancake.ExLibEditor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace PancakeEditor.ComponentHeader
{
    internal static class VisualElementCreator
    {
        private static readonly Dictionary<ButtonType, Texture2D> IconCaches = new();
        private static readonly Dictionary<ButtonType, Texture2D> IconDarkCaches = new();

        private static Texture2D GetIcon(ButtonType buttonType)
        {
            if (EditorGUIUtility.isProSkin)
            {
                if (IconCaches.TryGetValue(buttonType, out var result)) return result;
                switch (buttonType)
                {
                    case ButtonType.Remove:
                        IconCaches[buttonType] = EditorResources.IconRemoveComponent;
                        break;
                    case ButtonType.MoveUp:
                        IconCaches[buttonType] = EditorResources.IconMoveUp;
                        break;
                    case ButtonType.MoveDown:
                        IconCaches[buttonType] = EditorResources.IconMoveDown;
                        break;
                    case ButtonType.PasteComponentValue:
                        IconCaches[buttonType] = EditorResources.IconPasteComponentValues;
                        break;
                    case ButtonType.CopyComponent:
                        IconCaches[buttonType] = EditorResources.IconCopyComponent;
                        break;
                }

                return IconCaches[buttonType];
            }
            else
            {
                if (IconDarkCaches.TryGetValue(buttonType, out var result)) return result;
                switch (buttonType)
                {
                    case ButtonType.Remove:
                        IconDarkCaches[buttonType] = EditorResources.IconRemoveComponentDark;
                        break;
                    case ButtonType.MoveUp:
                        IconDarkCaches[buttonType] = EditorResources.IconMoveUpDark;
                        break;
                    case ButtonType.MoveDown:
                        IconDarkCaches[buttonType] = EditorResources.IconMoveDownDark;
                        break;
                    case ButtonType.PasteComponentValue:
                        IconDarkCaches[buttonType] = EditorResources.IconPasteComponentValuesDark;
                        break;
                    case ButtonType.CopyComponent:
                        IconDarkCaches[buttonType] = EditorResources.IconCopyComponentDark;
                        break;
                }

                return IconDarkCaches[buttonType];
            }
        }

        public static VisualElement CreateContainer(string headerElementName, Action onRefresh)
        {
            var container = new VisualElement {pickingMode = PickingMode.Ignore, style = {flexDirection = FlexDirection.RowReverse, height = 22,}};

            var imageCreator = new ImageCreator(headerElementName, onRefresh);
            var removeComponentImage = imageCreator.CreateImage(ButtonType.Remove, Undo.DestroyObjectImmediate);
            var moveUpImage = imageCreator.CreateImage(ButtonType.MoveUp, x => ComponentUtility.MoveComponentUp(x));
            var moveDownImage = imageCreator.CreateImage(ButtonType.MoveDown, x => ComponentUtility.MoveComponentDown(x));
            var copyComponentImage = imageCreator.CreateImage(ButtonType.CopyComponent, CopyComponent);
            var pasteComponentValuesImage = imageCreator.CreateImage(ButtonType.PasteComponentValue, PasteComponentValues);

            container.Add(removeComponentImage);
            container.Add(moveUpImage);
            container.Add(moveDownImage);
            container.Add(pasteComponentValuesImage);
            container.Add(copyComponentImage);

            return container;
        }

        private static void CopyComponent(Component component)
        {
            ComponentUtility.CopyComponent(component);
            Debug.Log($"Copied! '{component.GetType().Name}'");
            TooltipWindow.Show("Copied!");
        }

        private static void PasteComponentValues(Component component)
        {
            ComponentUtility.PasteComponentValues(component);
            Debug.Log($"Pasted! '{component.GetType().Name}'");
            TooltipWindow.Show("Pasted!");
        }

        private sealed class ImageCreator
        {
            private readonly string _headerElementName;
            private readonly Action _onRefresh;

            public ImageCreator(string headerElementName, Action onRefresh)
            {
                _headerElementName = headerElementName;
                _onRefresh = onRefresh;
            }

            public Image CreateImage(ButtonType buttonType, Action<Component> action)
            {
                var image = new Image
                {
                    style =
                    {
                        position = Position.Relative,
                        backgroundImage = GetIcon(buttonType),
                        top = 3,
                        right = 63 + (int) buttonType * 3,
                        width = 16,
                        height = 16,
                    }
                };

                image.RegisterCallback<ClickEvent>(_ =>
                {
                    var componentName = _headerElementName switch
                    {
                        "TextMeshPro - TextHeader" => "TextMeshPro",
                        "TextMeshPro - Text (UI)Header" => "TextMeshProUGUI",

                        _ => _headerElementName.Remove(_headerElementName.Length - 6, 6).Replace(" ", "").Replace("(Script)", "")
                    };

                    foreach (var gameObject in Selection.gameObjects)
                    {
                        var component = gameObject.GetComponent(componentName);

                        action(component);
                    }

                    _onRefresh();
                });

                return image;
            }
        }
    }
}