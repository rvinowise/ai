using UnityEngine;
using NUnit.Framework;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.table;
using UnityEditor;


namespace rvinowise.ai.unity.unit_tests.ui.table {

/* makes sense for testing prefabs of unity, since simple C# classes
 should conform to the interface at compile time */
[TestFixture]
public class table_prefab_is_instantiated {
    
    [Test]
    public void fields_are_assigned_in_editor() {
        Assert.That(table_prefab.table_cell_prefab is Component);
    }
    
    private readonly Table table_prefab = 
        AssetDatabase.LoadAssetAtPath<Table>("Assets\\objects\\ui\\Table\\button_table.prefab");

}

/* makes sense for testing prefabs of unity, since simple C# classes
 should conform to the interface at compile time */
[TestFixture]
public class an_item_is_added_to_the_table {
    
    [Test]
    public void a_cell_is_created() {
        var item = new GameObject();
        Assert.NotNull(
            table.add_item(item.transform)
        );
    }

    private ITable<Component> table; 
    private readonly Table table_prefab = 
        AssetDatabase.LoadAssetAtPath<Table>("Assets\\objects\\ui\\Table\\button_table.prefab");
    

    [SetUp]
    public void create_table() {
        table = table_prefab.provide_new<Button_table>();
    }
    

}


}