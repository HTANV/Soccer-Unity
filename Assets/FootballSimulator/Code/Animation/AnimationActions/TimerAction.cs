using UnityEngine;

namespace FStudio.Animation {
    public class TimerAction : BaseAction {
        private readonly float speed;

        public TimerAction (float time) {
            speed = 1/time;
        }

        public override void Finish() {
            Progress = 1;
        }

        public override bool Update(in float deltaTime) {
            Progress = Mathf.Min(Progress + deltaTime * speed, 1);
            return Progress == 1;
        }
    }
}
