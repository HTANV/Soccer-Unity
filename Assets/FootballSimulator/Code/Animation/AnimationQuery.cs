using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace FStudio.Animation 
{
    public class AnimationQuery {
        /// <summary>
        /// Game speed. You may not want to change this.
        /// If you want to change the speed of all the animation queries, change this value.
        /// </summary>
        private const float GameAnimationSpeed = 1;

        [Serializable]
        public struct ProgressEvent {
            [Range (0, 1)]
            public float Time;
            public Action Event;
            public bool Achieved;

            public ProgressEvent (float Time, Action Event) {
                this.Time = Time;
                this.Event = Event;
                Achieved = false;
            }
        }

        public AnimationQuery () { }

        /// <summary>
        /// Construct animation query with progress events.
        /// </summary>
        public AnimationQuery (ProgressEvent [] progressEvents) {
            this.progressEvents = progressEvents;
            eventsLength = progressEvents.Length;
        }

        /// <summary>
        /// List of the query.
        /// </summary>
        private List<BaseAction> queryList = new List<BaseAction>();

        private readonly ProgressEvent[] progressEvents;

        private int eventsLength;

        private Coroutine currentCoroutine;

        private MonoBehaviour coroutineHolder;

        private Action onCompleted;

        /// <summary>
        /// Add an animation to the query.
        /// </summary>
        /// <param name="animation"></param>
        public void AddToQuery(BaseAction animation) {
            queryList.Add(animation);
        }

        /// <summary>
        /// Start playing the animation query.
        /// </summary>
        /// <param name="coroutineHolder">You must assign a monobehaviour to hold the coroutine.</param>
        /// <param name="onCompleted">What you gonna do when it's finished?</param>
        public void Start (MonoBehaviour coroutineHolder, Action onCompleted = null) {
            this.onCompleted = onCompleted;

            if (queryList.Count == 0) {
                this.onCompleted?.Invoke();
                return;
            }

            this.coroutineHolder = coroutineHolder;

            if (!coroutineHolder.isActiveAndEnabled) {
                return;
            }

            currentCoroutine = coroutineHolder.StartCoroutine(LoopQuery(onCompleted));
        }

        /// <summary>
        /// Stops the animation playing, but will be instantly transited to the end.
        /// </summary>
        public void StopWithInstantFinish () {
            if (currentCoroutine != null) {
                foreach (var q in queryList) {
                    q.Finish(); // finish all.
                }

                Stop();

                onCompleted?.Invoke();
            }
        }

        /// <summary>
        /// Stop playing.
        /// </summary>
        public void Stop () {
            if (currentCoroutine != null) {
                coroutineHolder.StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
        }

        private IEnumerator LoopQuery(Action onCompleted) {
            int queryCount = queryList.Count;
            bool[] completed = new bool[queryCount];

            while (true) {
                bool killed = false;
                
                float deltaTime = Time.unscaledDeltaTime * GameAnimationSpeed;

                float totalProgress = 0;
                for (int i = 0; i < queryCount; i++) {
                    try {
                        completed[i] = queryList[i].Update(in deltaTime);
                        totalProgress += queryList[i].Progress;
                    } catch {
                        killed = true;
                        break;
                    }
                }

                if (killed) {
                    break;
                }

                // calculate progress and trigger progress events.
                float progress = totalProgress / queryCount;
                for (int i=0; i<eventsLength; i++) {
                    if (!progressEvents[i].Achieved) {
                        if (progressEvents[i].Time <= progress) {
                            progressEvents[i].Achieved = true;
                            progressEvents[i].Event?.Invoke();
                        }
                    }
                }
                //

                bool isFinished = true;

                for (int i = 0; i < queryCount; i++) { 
                    if (!completed[i]) {
                        isFinished = false;
                        break;
                    }
                }

                if (isFinished) {
                    onCompleted?.Invoke();
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}