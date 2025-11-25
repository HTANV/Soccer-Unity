
using System;
using System.Collections.Generic;
using System.Linq;

namespace FStudio.MatchEngine.Utilities {
    /// <summary>
    /// This is a 'readonly' class to store any kind of object with fixed size.
    /// <para>
    /// When you add a member to this, it will push all items forward in collection and put the new member to the first slot.
    /// <para>When you remove a member from this, it will pull all the next items to the index of removed object.</para>
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LimitedCollection<T> {
        private readonly int limit;
        private readonly T[] array;

        public IEnumerable<T> Members => array.Where (x=>x!=null);

        /// <summary>
        /// Does contain any members?
        /// </summary>
        /// <returns></returns>
        public bool IsAny () {
            for (int i=0; i<limit; i++) {
                if (array[i] != null) {
                    return true;
                }
            }

            return false;
        }

        public LimitedCollection (int limit) {
            this.limit = limit;
            array = new T[limit];
        }

        public void Add (T member) {
            // move array 
            for (int i=limit - 1; i>0; i--) {
                array[i] = array[i - 1];
            }

            array[0] = member;
        }

        public void Remove (T member) {
            int index = -1;

            for (int i=0; i<limit; i++) {
                if (member.Equals (array[i])) {
                    index = i;
                    break;
                }
            }

            if (index == -1) {
                return; // not found.
            }

            for (int i = index + 1; i<limit; i++) {
                array[i - 1] = array[i];
            }
        }

        public void Clear () {
            for (int i=0; i<limit; i++) {
                array[i] = default;
            }
        }
    }
}
