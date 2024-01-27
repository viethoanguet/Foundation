﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Pancake.Scriptable
{
    [Serializable]
    public abstract class ScriptableEvent<T> : ScriptableEventBase, IDrawObjectsInInspector
    {
        [Tooltip("Value used when raising the event in editor.")] [SerializeField]
        protected T debugValue = default(T);

        private readonly List<EventListenerGeneric<T>> _eventListeners = new();
        private readonly List<Object> _listenersObjects = new();
        private Action<T> _onRaised = null;

        /// <summary>
        /// Action triggered when this event has been raised.
        /// Subscribe to this action if you want to bind methods to this event.
        /// </summary>
        public event Action<T> OnRaised
        {
            add
            {
                _onRaised += value;

                var listener = value.Target as Object;
                if (listener != null && !_listenersObjects.Contains(listener)) _listenersObjects.Add(listener);
            }
            remove
            {
                _onRaised -= value;

                var listener = value.Target as Object;
                if (_listenersObjects.Contains(listener)) _listenersObjects.Remove(listener);
            }
        }

        public override Type GetGenericType => typeof(T);

        /// <summary>
        /// Raise the event
        /// </summary>
        public void Raise(T param)
        {
            if (!Application.isPlaying)
                return;

            for (var i = _eventListeners.Count - 1; i >= 0; i--)
                _eventListeners[i].OnEventRaised(this, param, debugLogEnabled);

            _onRaised?.Invoke(param);

#if UNITY_EDITOR
            //As this uses reflection, I only allow it to be called in Editor.
            //If you want to display debug in builds, delete the #if UNITY_EDITOR
            if (debugLogEnabled) Debug();
#endif
        }

        /// <summary>
        /// Registers the listener to this event.
        /// </summary>
        internal void RegisterListener(EventListenerGeneric<T> listener)
        {
            if (!_eventListeners.Contains(listener)) _eventListeners.Add(listener);
        }

        /// <summary>
        /// Unregisters the listener from this event.
        /// </summary>
        internal void UnregisterListener(EventListenerGeneric<T> listener)
        {
            if (_eventListeners.Contains(listener)) _eventListeners.Remove(listener);
        }

        /// <summary>
        /// Get all objects that are listening to this event.
        /// </summary>
        public List<Object> GetAllObjects()
        {
            var allObjects = new List<Object>(_eventListeners);
            allObjects.AddRange(_listenersObjects);
            return allObjects;
        }

        private void Debug()
        {
            if (_onRaised == null) return;
            var delegates = _onRaised.GetInvocationList();
            foreach (var del in delegates)
            {
                var sb = new StringBuilder();
                sb.Append("<color=#f75369>[Event] </color>");
                sb.Append(name);
                sb.Append(" => ");
                sb.Append(del.GetMethodInfo().Name);
                sb.Append("()");
                var monoBehaviour = del.Target as MonoBehaviour;
                UnityEngine.Debug.Log(sb.ToString(), monoBehaviour?.gameObject);
            }
        }

        public override void Reset()
        {
            debugLogEnabled = false;
            debugValue = default;
        }
    }
}