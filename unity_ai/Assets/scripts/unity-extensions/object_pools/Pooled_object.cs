using System;
using System.Collections.Generic;
using UnityEngine;
using rvinowise.contracts;
using rvinowise.unity.extensions.attributes;


namespace rvinowise.unity.extensions.pooling {

public class Pooled_object: MonoBehaviour {
    

    public Object_pool pool {
        get => _pool;
        set {
            Contract.Requires(
                _pool == null,
                "pool of a pooled object can only be set once"
            );
            _pool = value;
        }
    }
    private Object_pool _pool;

    [HideInInspector]
    public GameObject prefab;
    public List<Component> reset_components = new List<Component>();
    
    private static int count = 0;


    #region creation
    [called_by_prefab]
    public TComponent get_from_pool<TComponent>(
        Vector2 in_position,
        Quaternion in_rotation
    ) 
        where TComponent: Component 
    {
        GameObject game_object = get_from_pool();
        game_object.transform.position = in_position;
        game_object.transform.rotation = in_rotation;
        game_object.SetActive(true);
        TComponent component = game_object.GetComponent<TComponent>();
        return component;
    }

    [called_by_prefab]
    public TComponent get_from_pool<TComponent>() 
    where TComponent: Component
    {
        TComponent component = get_from_pool().GetComponent<TComponent>();
        component.gameObject.SetActive(true);
        return component;
    }

    [called_by_prefab]
    public GameObject get_from_pool() {
        Contract.Requires(
            gameObject.is_prefab(),
            "it should be called from a prefab"
        );
        ensure_pool_created();
        GameObject game_object = pool.get();
        return game_object;
    }
    
    [called_by_prefab]
    public GameObject instantiate() {
        if (pool == null) {
            count++;
																								  
            pool = new Object_pool(this.gameObject);
            Debug.Log(string.Format("amount of prefabs with pooled Component '{0}' = {1}", this, count));
        }
        return pool.get();
    }
    
    [called_by_prefab]
    public TComponent instantiate<TComponent>() where TComponent: MonoBehaviour {
        GameObject game_object = get_from_pool();
        TComponent component = game_object.GetComponent<TComponent>();
        return component;
    }
    
    [called_by_prefab]		  
    public TComponent instantiate<TComponent>(
        Vector2 in_position,
        Quaternion in_rotation
    ) 
        where TComponent: MonoBehaviour 
    {
        GameObject game_object = get_from_pool();
        game_object.transform.position = in_position;
        game_object.transform.rotation = in_rotation;
									
        TComponent component = game_object.GetComponent<TComponent>();
        return component;
    }
    #endregion creation


    [called_by_prefab]
    public void init_reset_components(GameObject game_object) {
        foreach(Component src_component in reset_components) {
            Component dst_component = game_object.GetComponent(src_component.GetType());
            dst_component.copy_fields_from(src_component);
        } 
    }

    [called_by_prefab]
    public void prefill_pool(int qty) {
        ensure_pool_created();
        pool.prefill(qty);
    }

    public void destroy() {
        if (pool != null) {
            gameObject.SetActive(false);
            pool.return_to_pool(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    
    private void ensure_pool_created() {
        if (pool == null) {
            count++;
            gameObject.SetActive(false); // so that OnEnable is called as the instance constructor
            pool = new Object_pool(this.gameObject);
            Debug.Log(String.Format("pooled_prefab: {0}, total: {1}", gameObject.name, count));
        }
    }
    
}


}