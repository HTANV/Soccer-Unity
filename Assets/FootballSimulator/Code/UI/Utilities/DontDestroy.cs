using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FStudio.Utilities {
    public class DontDestroy : MonoBehaviour {
        void Awake () {
            DontDestroyOnLoad(gameObject); 
        }
    }

}
