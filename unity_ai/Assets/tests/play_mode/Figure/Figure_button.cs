using NUnit.Framework;
using rvinowise.ai.general;
using rvinowise.ai.ui.general;
using rvinowise.ai.ui.unity;
using rvinowise.unity.extensions;
using UnityEditor;


namespace rvinowise.ai.unity.unit_tests.figure_button {

/* makes sense for testing prefabs of unity, since simple C# classes
 should conform to the interface at compile time */
[TestFixture]
public class figure_button_is_constructed_for_a_figure {
    
    [Test]
    public void figure_button_has_been_constructed_well() {

        Assert.NotNull(figure_button_prefab.create_for_figure(visual_figure));
    }
    
    private IFigure_button figure_button;
    private IVisual_figure visual_figure;

    private readonly IFigure_button figure_button_prefab = 
        AssetDatabase.LoadAssetAtPath<Figure_button>("Assets/objects/Node/Figure/figure_button.prefab");
    private readonly IVisual_figure figure_prefab = 
        AssetDatabase.LoadAssetAtPath<Figure>("Assets/objects/Node/Figure/figure.prefab");
    
    [SetUp]
    public void prepare() {
        visual_figure = ((unity.Figure)figure_prefab).provide_new<Figure>();
    }
    

}


}