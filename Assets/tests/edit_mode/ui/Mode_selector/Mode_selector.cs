using UnityEngine;
using NUnit.Framework;
using rvinowise.ai.general;

using rvinowise.ai.simple;
using rvinowise.ai.ui.general;
using rvinowise.ai.ui.unity;
using rvinowise.ai.unity;
using rvinowise.unity.ui.input;
using Figure = rvinowise.ai.unity.Figure;
using Network = rvinowise.ai.simple.Network;


namespace rvinowise.ai.unit_tests.ui.mode_selector {


[TestFixture]
public class new_figure_is_created {

   
    
    [Test]
    public void shows_figure_graph(
        
    ) {
        
    }


}


namespace observation_mode {

[TestFixture]
public class figure_button_is_clicked {
    
    private IVisual_figure figure1;
    private IVisual_figure figure2;
    //private IFigure_button button;
    private IFigure_button button1;
    private IFigure_button button2;

    [SetUp]
    public void prepare() {
        figure1 = new GameObject().AddComponent<unity.Figure>();
        figure2 = new GameObject().AddComponent<unity.Figure>();
        button1 = create_button_for_figure(figure1);
        button2 = create_button_for_figure(figure2);
    }

    private IFigure_button create_button_for_figure(
        IVisual_figure figure
    ) {
        IFigure_button button = new GameObject().AddComponent<Figure_button>().create_for_figure(figure);
        figure.button = button;
        return button;
    }
    
    [Test]
    public void shows_figure_details(

    ) {
        button1.on_click();
        Assert.That(figure1.is_shown, Is.True);
        Assert.That(figure2.is_shown, Is.False);
    }


}

[TestFixture]
public class several_figure_buttons_in_a_showcase_are_switched {
    
    private IVisual_figure figure1;
    private IVisual_figure figure2;
    private IFigure_button button1;
    private IFigure_button button2;

    [SetUp]
    public void prepare() {
        figure1 = new GameObject().AddComponent<unity.Figure>();
        figure2 = new GameObject().AddComponent<unity.Figure>();
        button1 = create_button_for_figure(figure1);
        button2 = create_button_for_figure(figure2);
    }

    private IFigure_button create_button_for_figure(
    IVisual_figure figure
    ) {
        IFigure_button button = new GameObject().AddComponent<Figure_button>().create_for_figure(figure);
        figure.button = button;
        return button;
    }
    
    [Test]
    public void shows_figure_details(

    ) {
        button1.on_click();
        Assert.That(figure1.is_shown, Is.True);
        Assert.That(figure2.is_shown, Is.False);
    }


}

}

}