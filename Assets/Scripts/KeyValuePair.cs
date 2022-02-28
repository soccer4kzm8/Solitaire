using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyValuePair<TKey, TValue>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }

    public KeyValuePair(TKey key, TValue val)
    {
        this.Key = key;
        this.Value = val;
    }
}
