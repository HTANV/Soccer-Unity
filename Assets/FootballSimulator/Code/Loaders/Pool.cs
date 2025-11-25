using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FStudio.Loaders {
    public class Pool<T> where T : Object {
        private readonly T[] objectList;
        private readonly int size;

        private int step;

        public IEnumerable<T> Members => objectList.AsEnumerable();

        public Pool(T targetObject, Transform holder, int poolSize) {
            objectList = new T[poolSize];
            for (int i = 0; i < poolSize; i++) {
                objectList[i] = Object.Instantiate(targetObject, holder);
                objectList[i].name = targetObject.name;
            }

            size = poolSize;
        }
		
        public T Get () {
            var select = objectList[step];

            if (++step >= size) {
                step = 0;
            }

            return select;
        }

        public void Reset () {
            step = 0;
        }
    }
}
