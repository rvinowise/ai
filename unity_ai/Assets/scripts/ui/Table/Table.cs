using UnityEngine;
using rvinowise.unity.extensions;
using System.Collections.Generic;
using rvinowise.ai.unity.visuals;
using rvinowise.contracts;
using UnityEngine.UI;

namespace rvinowise.unity.ui.table {

[RequireComponent(typeof(Canvas))]
public class Table: MonoBehaviour,
ITable<Component>
{

    public Table_cell table_cell_prefab;
    [SerializeField]private Canvas canvas;

    private List<Table_cell> cells = new List<Table_cell>();
    private GridLayoutGroup layout_group;

    public void Awake() {
        Contract.Assert(canvas != null, "canvas should be assigned in unity editor");
    }
    
    public void init(ICircle stored_object) {
        layout_group = GetComponent<GridLayoutGroup>();
        layout_group.cellSize = new Vector2(stored_object.radius, stored_object.radius);
    }
    public Component add_item(
        Component in_item
    ) {
        Table_cell cell = create_cell();
        cell.put_item(in_item);
        return in_item;
    }

    private Table_cell create_cell() {
        Table_cell cell = table_cell_prefab.get_from_pool<Table_cell>();
        /* cell.name = 
            String.Format("canvas {0}",in_item.name); */
        cell.transform.SetParent(canvas.transform, false);
        cells.Add(cell);
        return cell;
    }

    public void remove_item(
        Component in_item
    ) {
        Table_cell removed_cell = null;
        foreach(Table_cell cell in cells) {
            if (cell.item == in_item) {
                removed_cell = cell;
                
            }
        }
        if (removed_cell != null) {
            cells.Remove(removed_cell);
            removed_cell.destroy();
        }
        
    }
}
} 