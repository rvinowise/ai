using System;


namespace rvinowise.rvi {

public static partial class Array_extension
{
    

    public static void fill<T>(this Array array, T value)
    {
        if (array == null) {
            throw new NullReferenceException();
        }
        for (int i=0;i< array.Length; i++) {
            array.SetValue(value, i);
        }
    }
}

}
