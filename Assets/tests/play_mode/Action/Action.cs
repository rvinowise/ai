using NUnit.Framework;
using rvinowise.ai.general;
using rvinowise.unity.extensions;
using UnityEditor;


namespace rvinowise.ai.unity.unit_tests.action {


[TestFixture]
public class action_is_created {
    
    [Test]
    public void fields_can_be_assigned_to() {
        Assert.DoesNotThrow(()=>action.set_label("a"));
    }
    
    private IVisual_action action;

    private readonly unity.Action action_prefab =
        AssetDatabase.LoadAssetAtPath<Action>("Assets/objects/Node/Action/action.prefab");
    
    [SetUp]
    public void prepare() {
        action = action_prefab.provide_new<unity.Action>();
    }
    

}


}