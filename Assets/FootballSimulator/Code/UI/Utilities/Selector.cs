using UnityEngine;

using System;

namespace FStudio.UI {
    public class Selector : MonoBehaviour {
        public int CurrentSelected { get; private set; }

        public Action<int> OnSelectionUpdate;

        public int Max;

        private bool updateSilently;

        private void UpdateSelection () {
            OnSelectionUpdate?.Invoke(CurrentSelected);
        }

        public void Next () {
            var target = CurrentSelected + 1;
            if (target >= Max) {
                target = 0;
            }

            CurrentSelected = target;

            if (updateSilently) {
                return;
            }

            UpdateSelection();
        }

        public void Back () {
            var target = CurrentSelected - 1;
            if (target < 0) {
                target = Max - 1;
            }

            CurrentSelected = target;

            if (updateSilently) {
                return;
            }

            OnSelectionUpdate?.Invoke(CurrentSelected);
        }

        public void SetSelected (int index) {
            CurrentSelected = index - 1;
            Next();
        }

        public void SetSelectedSilent (int index) {
            updateSilently = true;
            CurrentSelected = index - 1;
            Next();
            updateSilently = false;
        }
    }
}