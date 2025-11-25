using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using Shared;

namespace FStudio.Loaders {
    public class StaticPool<T, ValueMember> 
        where T : MonoBehaviour,
        IStaticPoolMember<ValueMember>
        where ValueMember : class, 
        IUnique<ValueMember> {

        private AssetReference assetReference;
        private Transform parent;

        private readonly bool updateCurrentsWhenSet;

        private readonly List<T> list = new List<T>();

        private int step;

        public StaticPool(
            AssetReference assetReference,
            Transform parent,
            bool updateCurrentsWhenSet = false
            ) {
            this.assetReference = assetReference;
            this.parent = parent;
            this.updateCurrentsWhenSet = updateCurrentsWhenSet;
        }

        public void Add (T target) {
            if (list.Contains(target)) {
                return;
            }

            list.Add(target);
        }

        public void Remove (T target) {
            list.Remove(target);
        }

        public IEnumerable<T> Members => list.AsEnumerable();

        public T Get (T[] except = null) {
            var targets = list.AsEnumerable();

            if (except != null) {
                targets = targets.Where(x => !except.Contains(x));
            }

            int i = 0;
            int length = targets.Count ();
            while (i < length) {
                if (++step >= length) {
                    step = 0;
                }

                if (!targets.ElementAt (step).IsActive) {
                    return targets.ElementAt (step);
                }

                i++;
            }

            return default;
        }

        public async Task SetMembers (ValueMember[] values) {
            Debug.Log("[StaticPool] SetMembers with count of: " + values.Count());

            if (values.Contains (null)) {
                Debug.LogError("SetMembers has null value in values.");
            }

            var notExistAnymore = Members.Where(x =>
            x.Member != null && values.FirstOrDefault (e=>e.IsSame (x.Member)) == null);

            foreach (var removal in notExistAnymore) {
                await removal.SetMember(null);
                removal.MarkAsDeactive();
            }

            var missing = values.Where(x => 
                Members.FirstOrDefault (e=>
                e.Member != null && e.Member.IsSame (x)) == null);

            if (updateCurrentsWhenSet) {
                // update currents;
                var currents = values.Where(x =>
                    Members.FirstOrDefault(e =>
                   e.Member != null && e.Member.IsSame(x)) != null);

                foreach (var currentValue in currents) {
                    var targetMember = Members.FirstOrDefault (x => x.Member != null && x.Member.IsSame(currentValue));
                    await targetMember.SetMember(currentValue);
                }
            }

            var willActivate = new T[missing.Count()];

            int index = 0;

            foreach (var newValue in missing) {
                var target = Get(willActivate);

                if (target == null) {
                    var asset = await assetReference.InstantiateAsync(parent).Task;
                    target = asset.GetComponent<T>();
                    list.Add(target);
                }

                await target.SetMember(newValue);

                willActivate[index] = target;

                index++;
            }

            foreach (var activate in willActivate) {
                if (activate != null) {
                    activate.MarkAsActive();
                }
            }
        }
    }
}
