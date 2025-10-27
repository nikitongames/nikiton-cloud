using UnityEngine;
using System;

namespace Nikiton.Core.Events
{
    public static class EventSystem
    {
        public static Action<string> OnEventTriggered;

        public static void Trigger(string eventName)
        {
            Debug.Log("Event triggered: " + eventName);
            OnEventTriggered?.Invoke(eventName);
        }
    }
}
