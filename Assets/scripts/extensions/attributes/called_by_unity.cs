using System;

namespace rvinowise.unity.extensions.attributes {


/*
    on_button_clicked etc.
 */
[
    AttributeUsage(
        AttributeTargets.Method, 
        AllowMultiple = false
    )
]
public class called_by_unity: System.Attribute {
    
}
}