using rvinowise.unity.extensions.pooling;
using rvinowise.contracts;
using UnityEngine;

namespace rvinowise.unity.extensions {


public static partial class Unity_extension {


    public static TComponent get_from_pool<TComponent>(
        this MonoBehaviour prefab_component
    )
        where TComponent : MonoBehaviour {
        Pooled_object pooled_object = prefab_component.GetComponent<Pooled_object>();
        Contract.Requires(pooled_object != null, "pooled prefabs must have the Pooled_object component");
        TComponent returned_component = pooled_object.instantiate().GetComponent<TComponent>();
        return returned_component;
    }
    
    

}
}