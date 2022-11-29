using System.Collections;


public static class Extensions
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