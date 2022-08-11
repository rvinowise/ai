using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Unity_extension
{


    public static void SetToFirst(this IEnumerator enumerator) {
        enumerator.Reset();
        enumerator.MoveNext();
    }
    public static void SetToFirst<T>(this IEnumerator<T> enumerator) {
        enumerator.Reset();
        enumerator.MoveNext();
    }
    
    
}