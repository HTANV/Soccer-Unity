using System;

namespace FStudio.Utilities {
    [Serializable]
    public class Entry<Key, Value> {
        public Key Id;
        public Value Val;
    }

    [Serializable]
    public class SerializableCollection <Key, Value> : Object {
        public Entry<Key, Value>[] Entries;

        public Value Find (Key key) {
            for (int i=0, length = Entries.Length; i<length; i++) {
                if (Entries[i].Id.Equals ( key )) {
                    return Entries[i].Val;
                }
            }

            return Entries[0].Val;
        }
    }
}
