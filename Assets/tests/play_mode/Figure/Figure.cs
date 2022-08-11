using NUnit.Framework;
using rvinowise.ai.general;
using rvinowise.unity.extensions;
using UnityEditor;


namespace rvinowise.ai.unity.unit_tests.figure {

/* makes sense for testing prefabs of unity, since simple C# classes
 should conform to the interface at compile time */
[TestFixture]
public class figure_prefab_is_instantiated {
    
    [Test]
    public void figure_has_necessary_parts() {
        figure.id = "a";
        Assert.That(figure.id, Is.EqualTo("a"));
        Assert.NotNull(figure.header);
    }
    
    private IVisual_figure figure;

    private readonly unity.Figure figure_prefab = 
        AssetDatabase.LoadAssetAtPath<unity.Figure>("Assets/objects/Node/Figure/figure.prefab");
    
    [SetUp]
    public void prepare() {
        figure = figure_prefab.provide_new<unity.Figure>();
    }
    

}


}