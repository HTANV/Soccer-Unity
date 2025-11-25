using System.Collections.Generic;
using System;
using UnityEngine;

namespace FStudio.Events {
    public static class EventManager {
        private interface ICallback {
            public void Trigger(object callbackEvent);
            public bool Compare(object callbackEvent);
        }

        private class GenericCallback<T> : ICallback where T : IBaseEvent {
            
            private readonly Action<T> Callback;

            public GenericCallback(Action<T> callback) {
                this.Callback = callback;
            }

            public void Trigger (object callbackEvent) {
                Callback?.Invoke((T)callbackEvent);
            }

            public bool Compare (object callbackEvent) {
                return Callback == (Action<T>)callbackEvent;
            }
        }

        private static readonly Dictionary<Type, List<ICallback>> callbacks = new Dictionary<Type, List<ICallback>>();

        public static void Subscribe <T> (Action<T> callback) where T : IBaseEvent {
            var callbackType = typeof(T);

            if (!callbacks.ContainsKey(callbackType)) {
                callbacks.Add(callbackType, new List<ICallback>());
            }

            var callbackList = callbacks[callbackType];

            var genericCallback = new GenericCallback<T>(callback);

            callbackList.Add(genericCallback);
        }

        public static void UnSubscribe<T> (Action<T> callback) where T : IBaseEvent {
            var callbackType = typeof(T);

            if (!callbacks.ContainsKey(callbackType)) {
                return;
            }

            var callbackList = callbacks[callbackType];

            var current = callbackList.Find (x=>x.Compare (callback));

            if (current != null) {
                callbackList.Remove(current);
            }
        }

        public static void Trigger<T>(T eventObject) where T : IBaseEvent {
            var callbackType = typeof(T);

            if (!callbacks.ContainsKey(callbackType)) {
                Debug.Log($"No callbacks found for typeof {callbackType}");
                return;
            }

            /// create a safe array before call.
            var tempCallbacks = new ICallback[callbacks[callbackType].Count];
            callbacks[callbackType].CopyTo(tempCallbacks);

            foreach (var callback in tempCallbacks) {
                callback.Trigger(eventObject);
            }
        }
    }
}
