
using UnityEngine;

namespace FStudio.Utilities {
    public static class Extensions {
        public static void SetActive(this GameObject[] gameobjects, bool value) {
            foreach (var obj in gameobjects) {
                obj.SetActive(value);
            }
        }
    }
}
