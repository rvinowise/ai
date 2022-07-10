using Moq;
using UnityEngine;
using NUnit.Framework;
using rvinowise.ai.general;

using rvinowise.ai.simple;
using rvinowise.ai.ui.general;
using rvinowise.ai.ui.unity;
using rvinowise.ai.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;
using UnityEditor;
using Figure = rvinowise.ai.unity.Figure;
using Network = rvinowise.ai.simple.Network;


namespace rvinowise.ai.unit_tests.ui.figure_showcase {

[TestFixture]
public class figure_showcase_is_populated_with_figures {
    
    [Test]
    public void buttons_are_correctly_created_for_figures() {
        buttons_can_be_received_for_the_created_figures();
        buttons_are_different_for_different_figures();
    }
    
    private IVisual_figure figure1;
    private IVisual_figure figure2;
    private Figure_showcase figure_showcase;

    private readonly unity.Figure figure_prefab = 
        AssetDatabase.LoadAssetAtPath<unity.Figure>("Assets/objects/Node/Figure/figure.prefab");
    private readonly Button_table button_table_prefab = 
        AssetDatabase.LoadAssetAtPath<Button_table>("Assets/objects/ui/table/button_table.prefab");
    
    [SetUp]
    public void prepare() {
        figure_showcase = new GameObject().add_component<Figure_showcase>();
        Button_table button_table = button_table_prefab.provide_new<Button_table>();
        figure_showcase.button_table = button_table;
        figure_showcase.figure_prefab = figure_prefab;
        figure1 = figure_showcase.provide_figure("figure1");
        figure2 = figure_showcase.provide_figure("figure2");
        
    }
    

    private void buttons_can_be_received_for_the_created_figures() {
        Assert.That(
            figure_showcase.get_button_for_figure(figure1),
            Is.Not.Null
        );
        Assert.That(
            figure_showcase.get_button_for_figure(figure2),
            Is.Not.Null
        );
    }

    private void buttons_are_different_for_different_figures() {
        Assert.That(
            figure_showcase.get_button_for_figure(figure1),
            Is.Not.EqualTo(
                figure_showcase.get_button_for_figure(figure2)    
            )
        );
        
    }
}

[TestFixture]
public class several_figure_buttons_in_a_showcase_are_clicked_after_each_other {
    
    [Test]
    public void different_figure_details_are_shown_after_each_other() {
        button1.on_click();
        Assert.That(figure1.is_shown, Is.True);
        Assert.That(figure2.is_shown, Is.False);
        button2.on_click();
        Assert.That(figure2.is_shown, Is.True);
        Assert.That(figure1.is_shown, Is.False);
    }

    private IVisual_figure figure1;
    private IVisual_figure figure2;
    private IFigure_button button1;
    private IFigure_button button2;
    private Figure_showcase figure_showcase;

    private readonly unity.Figure figure_prefab = 
        AssetDatabase.LoadAssetAtPath<unity.Figure>("Assets/objects/Node/Figure/figure.prefab");
    private readonly Button_table button_table_prefab =
        AssetDatabase.LoadAssetAtPath<Button_table>("Assets/objects/ui/table/button_table.prefab");

    [SetUp]
    public void prepare() {
        figure_showcase = new GameObject().add_component<Figure_showcase>();
        var button_table = button_table_prefab.provide_new<Button_table>();
        button_table.higher_click_receiver = figure_showcase; // who should link them?
        figure_showcase.button_table = button_table;
        figure_showcase.figure_prefab = figure_prefab;
        figure1 = figure_showcase.provide_figure("figure1");
        figure2 = figure_showcase.provide_figure("figure2");
        button1 = figure_showcase.get_button_for_figure(figure1);
        button2 = figure_showcase.get_button_for_figure(figure2);
    }
}

}