using UnityEngine;

namespace FStudio.MatchEngine.Utilities {
    public class PlayerBallPoint : MonoBehaviour {
        public enum Situation {
            GK,
            ThrowIn,
            Normal
        }

        [SerializeField] private Transform[] ballPoints;

        public Vector3 GetPosition(Situation situation) => ballPoints[(int)situation].position;
        public Quaternion GetRotation (Situation situation) => ballPoints[(int)situation].rotation;
    }
}
