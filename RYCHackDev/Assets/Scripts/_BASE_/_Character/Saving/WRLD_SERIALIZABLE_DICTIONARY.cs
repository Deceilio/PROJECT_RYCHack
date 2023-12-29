using System.Collections.Generic;
using UnityEngine;

namespace Deceilio.Psychain 
{
    [System.Serializable]
    public class WRLD_SERIALIZABLE_DICTIONARY<Tkey, TValue> : Dictionary<Tkey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<Tkey> keys = new List<Tkey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        // BELOW CODE: Called right before serialization & saves the dictionary to the list
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach(KeyValuePair<Tkey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // BELOW CODE: Called right after serialisation & load the dictionary from the lists
        public void OnAfterDeserialize()
        {
            Clear();

            if(keys.Count != values.Count)
            {
                Debug.LogError("Deserialization Failed. The Amount of Keys doesn't match the amount of Values" % WRLD_COLORIZE_EDITOR.DarkRed % WRLD_FONT_FORMAT.Bold);
            }

            for(int i = 0; i < keys.Count; i++)
            {
                Add(keys[i], values[i]);
            }
        }
    }
}