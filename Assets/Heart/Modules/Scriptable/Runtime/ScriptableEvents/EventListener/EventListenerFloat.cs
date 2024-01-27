﻿using UnityEngine;
using UnityEngine.Events;

namespace Pancake.Scriptable
{
    /// <summary>
    /// A listener for a ScriptableEventFloat
    /// </summary>
    [AddComponentMenu("Scriptable/EventListeners/EventListenerFloat")]
    [EditorIcon("scriptable_event_listener")]
    public class EventListenerFloat : EventListenerGeneric<float>
    {
        [SerializeField] private EventResponse[] eventResponses;
        protected override EventResponse<float>[] EventResponses => eventResponses;

        [System.Serializable]
        public class EventResponse : EventResponse<float>
        {
            [SerializeField] private ScriptableEventFloat scriptableEvent;
            public override ScriptableEvent<float> ScriptableEvent => scriptableEvent;

            [SerializeField] private Pancake.FloatUnityEvent response;
            public override UnityEvent<float> Response => response;
        }
    }
}