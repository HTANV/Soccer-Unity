using FStudio.Events;
using FStudio.MatchEngine.Events;
using UnityEngine;

namespace FStudio.MatchEngine.Balls {
    public partial class Ball {
        private void OnTriggerStay (Collider other) {
            if (!MatchManager.Current.MatchFlags.HasFlag (Enums.MatchStatus.Playing)) {
                return;
            }

            var tag = other.tag;
        
            var sizeOfField = MatchManager.Current.SizeOfField;
            var position = transform.position;

            bool validTag = false;

            switch (tag) {
                case "GoalAction":
                    var goal = position.x > sizeOfField.x / 2;

                    EventManager.Trigger(new GoalEvent(!goal));

                    validTag = true;
                    break;
                
                case "OutAction":
                    var index = GetCornerIndex(position);
                    if (index != -1) {
                        EventManager.Trigger(new OutEvent(index));
                    }

                    validTag = true;
                    break;

                case "ThrowInAction":
                    var fixedPos = position;
                    if (position.z > sizeOfField.y / 2) {
                        position.z -= 0.1f;
                    } else {
                        position.z += 0.1f;
                    }

                    EventManager.Trigger(new ThrowInEvent(position));

                    validTag = true;
                    break;
            }

            if (validTag){
                Release();
            }
        }

        private static int GetCornerIndex(in Vector3 position) {
            var sizeOfField = MatchManager.Current.SizeOfField;
            bool xSmall = position.x < 0;
            bool xHigher = position.x > sizeOfField.x;
            int zPart = position.z > sizeOfField.y / 2 ? 1 : 0;

            if (xSmall || xHigher) {
                // corner found.
                if (xSmall) {
                    return zPart;
                } else {
                    return zPart + 2;
                }
            }

            return -1;
        }
    }
}
