using System;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.ai.simple;
using rvinowise.ai.ui.general;
using rvinowise.ai.ui.unity;
using rvinowise.rvi.contracts;
using rvinowise.ai.unity;
using rvinowise.ai.unity.simple;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.table;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace rvinowise.ai.ui.unity {
public interface IButton_table<out TButton>
    where TButton: class?, new() 
{

    IFigure_button_click_receiver click_receiver { get; }


}
}