using System;

namespace FStudio.Utilities {
    [Serializable]
    public class EnumEntry <Key, Value> where Key : Enum {
        public Key Id;
        public Value Val;
    }

    [Serializable]
    public class SerializableEnumDictionary <Key, Value> where Key : Enum {
        public EnumEntry<Key, Value>[] Entries;

        /// <summary>
        /// Finds and loads value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Value Find(Key key) {
            for (int i = 0, length = Entries.Length; i < length; i++) {
                if (Entries[i].Id.Equals(key)) {
                    return Entries[i].Val;
                }
            }

            return default;
        }
    }
}
