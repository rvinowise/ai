using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using rvinowise;
using rvinowise.unity.geometry2d;


namespace rvinowise.unity.extensions {

public class Dictionary_of_lists<TKey, TValue> :
    Multivalue_dictionary<
        TKey,
        TValue,
        Dictionary<TKey, List<TValue>>,
        List<TValue>
    >
    where TValue: class
{
    
}

public class Multivalue_dictionary<
    TKey, 
    TValue,
    TDictionary, 
    TContainer
> 
    where TValue: class
    where TDictionary: IDictionary<TKey, TContainer>, new()
    where TContainer: class, ICollection<TValue>, new()
{

    public TDictionary dictionary = 
        new TDictionary();

    public void add(TKey key, TValue obj) {
        TContainer bases = get_or_create_place_for_key(key);
        bases.Add(obj);
    }
    
    public TContainer get_values(
        TKey key
    ) {
        TContainer bases = get_or_create_place_for_key(key);

        if (bases.Count == 0) {
            return null;
        }
        
        return bases;
    }
    public TContainer this[TKey key] {
        get => get_values(key);
    }
    
    private TContainer get_or_create_place_for_key(TKey key) {
        TContainer container;
        dictionary.TryGetValue(key, out container);
        if (container == null) {
            container = new TContainer();
            dictionary.Add(key, container);
        }
        return container;
    }
}
}