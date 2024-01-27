﻿using UnityEngine;
using UnityEngine.Events;

namespace Pancake.Scriptable
{
    /// <summary>
    /// A listener for a ScriptableEventGameObject
    /// </summary>
    [AddComponentMenu("Scriptable/EventListeners/EventListenerGameObject")]
    [EditorIcon("scriptable_event_listener")]
    public class EventListenerGameObject : EventListenerGeneric<GameObject>
    {
        [SerializeField] private EventResponse[] eventResponses;
        protected override EventResponse<GameObject>[] EventResponses => eventResponses;

        [System.Serializable]
        public class EventResponse : EventResponse<GameObject>
        {
            [SerializeField] private ScriptableEventGameObject scriptableEvent;
            public override ScriptableEvent<GameObject> ScriptableEvent => scriptableEvent;

            [SerializeField] private Pancake.GameObjectUnityEvent response;
            public override UnityEvent<GameObject> Response => response;
        }
    }
}