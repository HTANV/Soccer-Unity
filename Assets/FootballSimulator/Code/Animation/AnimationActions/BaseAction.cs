
using static FStudio.Animation.AnimationQuery;

namespace FStudio.Animation
{
    public abstract class BaseAction {
        public float Progress { protected set; get; }

        /// <summary>
        /// Update the action, returns true when completed.
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="animationSettings"></param>
        /// <returns></returns>
        public abstract bool Update(in float deltaTime);

        /// <summary>
        /// Finished the animation directly.
        /// </summary>
        public abstract void Finish();

        public AnimationQuery GetQuery () {
            var query = new AnimationQuery();
            query.AddToQuery(this);
            return query;
        }

        public AnimationQuery GetQuery(ProgressEvent[] progressEvents) {
            var query = new AnimationQuery(progressEvents);
            query.AddToQuery(this);
            return query;
        }
    }
}