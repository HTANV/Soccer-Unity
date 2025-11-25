using UnityEngine;

using System;

using System.Linq;
using FStudio.UI.GamepadInput;
using System.Collections.Generic;
using FStudio.Audio;
using UnityEngine.Events;

namespace FStudio.UI.Navigation {
    public class NavigationManager : MonoBehaviour {
        private NavigationMember[] members;
        public int Length { get; private set; }

        public Action<int> OnNavigationUpdate;

        public Action<int> OnNavigationHover;

        [SerializeField] private UnityEvent OnNavUpdate;

        [SerializeField] private string onSelectSoundId;

        public IEnumerable<NavigationMember> Members => members;

        public int CurrentIndex { private set; get; }

        [SerializeField] private int selectOnAwake = -1;

        [Header ("When navigation changes, cursor snaps on him")]
        [SerializeField] private bool snapOnSelection;

        public NavigationMember GetActiveMember () => 
            members == null || CurrentIndex < 0 || CurrentIndex >= members.Length ? null :
            members[CurrentIndex];

        [SerializeField] private bool isClickable = true;

        private void Awake() {
            RefreshMembers();
        }

        private void Start () {
            if (selectOnAwake != -1) {
                OnInteraction(selectOnAwake);
            }
        }

        public void SetInteraction (bool value) {
            if (members == null) {
                return;
            }

            foreach (var member in members) {
                member.IsInteractable = value;
            }
        }

        private void OnHover (int index) {
            OnNavigationHover?.Invoke(index);
        }

        public void RefreshMembers () {
            members = GetComponentsInChildren<NavigationMember>();
            Length = members.Length;

            for (int i=0; i<Length; i++) {
                var temp = i;
                members[i].OnNavigationMemberClick = () => {
                    OnInteraction(temp);
                };

                members[i].onHover.AddListener ( () => {
                    OnHover(temp);
                });

                members[i].onUnHover.AddListener ( () => {
                    OnHover(-1);
                });
            }
        }
        
        private void OnInteraction (int index) {
            if (!string.IsNullOrEmpty (onSelectSoundId)) {
                UINavigationAudio.Current.audioManager.Play(onSelectSoundId);
            }

            SetIndex(index);
        }

        public void SetIndex (int value, bool ignoreSnap) {
            CurrentIndex = Mathf.Clamp(value, -1, Length-1);

            IndexUpdate(false, ignoreSnap);
        }

        public void SetIndex(int value) {
            CurrentIndex = Mathf.Clamp(value, -1, Length - 1);

            IndexUpdate(false, false);
        }

        public void SetIndexSilent (int value) {
            CurrentIndex = Mathf.Clamp(value, -1, Length-1);

            IndexUpdate(true, false);
        }

        private async void IndexUpdate (bool isSilent, bool ignoreSnap) {
            if (!isClickable) {
                return;
            }

            for (int i = 0; i < Length; i++) {
                if (i == CurrentIndex) {
                    members[i].SetAsActiveMember();
                } else {
                    if (members[i].IsCurrentlySelected) {
                        members[i].NoLongerActive();
                    }
                }
            }

            OnNavUpdate?.Invoke();

            if (!isSilent) {
                OnNavigationUpdate?.Invoke(CurrentIndex);

                if (snapOnSelection && CurrentIndex >= 0 && !ignoreSnap) {
                    await SnapManager.AutoSnap(GetActiveMember(), true);
                }
            }
        }
    }
}
