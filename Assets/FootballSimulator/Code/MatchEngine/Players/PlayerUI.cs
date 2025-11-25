using UnityEngine;
using TMPro;
using FStudio.Utilities;
using System.Linq;
using System.Collections.Generic;
using FStudio.Graphics.Cameras;

namespace FStudio.MatchEngine.Players {
    public class PlayerUI : MonoBehaviour {
        public enum UIAnimatorVariable {
            ShowName,
            ShowOffside,
            ParameterCount // Parameter count of the animator.
        }

        private static readonly List<PlayerUI> list = new List<PlayerUI>();
        public static IEnumerable<PlayerUI> Members => list.AsEnumerable ();

        private int[] animatorVariableHashes;

        private MainCamera mainCamera;

        [SerializeField] private Animator animator;

        public TextMeshPro nameText = default;

        private void OnEnable() {
            list.Add(this);
        }

        private void OnDisable() {
            list.Remove(this);
        }

        private void Awake () {
            animatorVariableHashes = 
                AnimatorEnumHasher.GetHashes<UIAnimatorVariable>(animator).Values.ToArray ();
        }

        public void SetBool(UIAnimatorVariable prop, bool value) {
            if (!animator.isActiveAndEnabled) {
                return;
            }

            animator.SetBool(animatorVariableHashes[(int)prop], value);
        }

        public void SetName (string name) {
            nameText.text = name;
        }

        public void ShowOffside (bool value) {
            SetBool(UIAnimatorVariable.ShowOffside, value);
        }

        public void ShowName (bool value) {
            SetBool(UIAnimatorVariable.ShowName, value);
        }

        private void LateUpdate () {
            if (mainCamera == null) {
                mainCamera = MainCamera.Current;
            }
            
            var toCamera = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
            toCamera.eulerAngles = new Vector3(toCamera.eulerAngles.x, toCamera.eulerAngles.y, 0);
            transform.rotation = toCamera;
        }
    }
}
