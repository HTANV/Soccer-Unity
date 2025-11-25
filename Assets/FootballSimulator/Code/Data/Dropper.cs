using UnityEngine;

namespace Shared {
    public interface IDroppable {
        /// <summary>
        /// Drop rate
        /// </summary>
        int Rate { get; }
    }

    public class Dropper<T> where T : class, IDroppable {
        private T[] randomizables;
        private int total;
        private int count;

        public Dropper(T[] randomizables) {
            this.count = randomizables.Length;

            this.randomizables = randomizables;

            for (int i = 0; i < count; i++) {
                total += randomizables[i].Rate;
            }
        }

        /// <summary>
        /// Randomly select
        /// </summary>
        /// <returns></returns>
        public T Select() {
            // Get a random integer from 0 to PoolSize.
            int randomNumber = Random.Range(0, total);

            // Detect the item, which corresponds to current random number.
            int accumulatedProbability = 0;
            for (int i = 0; i < count; i++) {
                accumulatedProbability += randomizables[i].Rate;

                if (randomNumber <= accumulatedProbability)
                    return randomizables[i];
            }

            return null;
        }
    }
}
