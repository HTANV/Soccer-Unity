using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FStudio.Utilities {
    public class Rotate : MonoBehaviour {
        [SerializeField] private float speed;
        [SerializeField] private Vector3 axis;
        // Update is called once per frame
        void Update() {
            transform.Rotate(axis, speed * Time.deltaTime);
        }
    }
}
