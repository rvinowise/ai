using Moq;
using UnityEngine;
using NUnit.Framework;
using rvinowise.ai.general;

using rvinowise.ai.simple;
using rvinowise.ai.ui.general;
using rvinowise.ai.ui.unity;
using rvinowise.ai.unity;
using rvinowise.unity.ui.input;
using Network = rvinowise.ai.simple.Network;


namespace rvinowise.ai.unit_tests.ui.figure_observer {

[TestFixture]
public class figure_showcase_is_populated_with_figures {
    
    private IVisual_figure figure1;
    private IVisual_figure figure2;
    private Figure_showcase figure_showcase;

    [SetUp]
    public void prepare() {
        figure_showcase = new GameObject().add_component<Figure_showcase>();
        figure_showcase.Awake();
        figure1 = figure_showcase.provide_figure("figure1");
        figure2 = figure_showcase.provide_figure("figure2");
        
    }

    [Test]
    public void buttons_can_be_received_for_the_created_figures() {
        Assert.That(
            figure_showcase.get_button_for_figure(figure1),
            Is.Not.Null
        );
        Assert.That(
            figure_showcase.get_button_for_figure(figure2),
            Is.Not.Null
        );
    }
}

[TestFixture]
public class several_figure_buttons_in_a_showcase_are_clicked_after_each_other {
    
    private IVisual_figure figure1;
    private IVisual_figure figure2;
    private IFigure_button button1;
    private IFigure_button button2;
    private IFigure_showcase<unity.Figure> figure_showcase;

    [SetUp]
    public void prepare() {
        figure_showcase = new GameObject().add_component<Figure_showcase>();
        figure1 = figure_showcase.provide_figure("figure1");
        figure2 = figure_showcase.provide_figure("figure2");
        button1 = figure_showcase.get_button_for_figure(figure1);
        button2 = figure_showcase.get_button_for_figure(figure2);
    }


    [Test]
    public void different_figure_details_are_shown_after_each_other() {
        button1.on_click();
        Assert.That(figure1.is_shown, Is.True);
        Assert.That(figure2.is_shown, Is.False);
        button2.on_click();
        Assert.That(figure2.is_shown, Is.True);
        Assert.That(figure1.is_shown, Is.False);
    }


}

}