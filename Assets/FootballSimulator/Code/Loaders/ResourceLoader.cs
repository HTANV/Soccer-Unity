using UnityEngine;
using System.Collections.Generic;

namespace FStudio.Loaders {
    public class ResourceLoader<T> where T : Object {
        private Dictionary<string, T> resources = new Dictionary<string, T>();

        public T Get (string resourceName) {
            if (!resources.ContainsKey(resourceName)) {
                var loadedResource = Resources.Load<T>(resourceName);
                resources.Add(resourceName, loadedResource);
            }

            return resources[resourceName];
        }
    }
}
