using System;


namespace rvinowise.unity.extensions {
public static partial class Unity_extension
{

   

    public static int get_label_as_number(
        this UnityEngine.U2D.Animation.SpriteResolver in_resolver
    ) {
        return Int32.Parse(in_resolver.GetLabel());

    }
    
}
}
