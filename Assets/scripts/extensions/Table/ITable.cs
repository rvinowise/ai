using System;
using UnityEngine;
using rvinowise.unity.extensions;
using System.Collections.Generic;
using rvinowise.ai.unity.visuals;
using UnityEngine.UI;

namespace rvinowise.ui.table {

public interface ITable<TItem> {


    public void add_item(TItem in_item);

    public void remove_item(TItem in_item);
}
} 