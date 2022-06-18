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
public class a_figure_button_is_pressed {

    private IMode_selector mode_selector;
    private IManual_figure_builder manual_figure_builder;
    private IFigure_observer figure_observer;
    private IFigure_provider<IVisual_figure> figure_provider;
    
    [SetUp]
    public void create_ui_controller() {
        mode_selector = new GameObject().AddComponent<Mode_selector>();
        manual_figure_builder = new GameObject().AddComponent<Manual_figure_builder>();
        figure_observer = new GameObject().AddComponent<Figure_observer>();
        figure_provider = new Figure_provider<IVisual_figure>(create_figure);
    }

    private IVisual_figure create_figure(string id) {
        var mock_figure_header = new Mock<IFigure_header>();
        mock_figure_header.Setup(
            header => header.on_finish_building()
        );
        
        var mock_figure = new Mock<IVisual_figure>();
        mock_figure.Setup(
            figure => figure.header
        ).Returns(mock_figure_header.Object);
        
        return new 
    }
    
    [Test]
    public void shows_the_structure_of_the_figure(
        
    ) {

        IVisual_figure figure = figure_provider.provide_figure("tested_figure");
        IFigure_button figure_button = ;
        
        figure_button.on_click();
        
        mode_selector.on_start_editing_figure();
        figure_observer.observe(figure);
        
        
        Assert.Thatfigure.is_shown

        manual_figure_builder.
    }


}

[TestFixture]
public class connections_of_an_existing_figure_are_edited {
    private Manual_figure_builder ui_builder;
    
    [SetUp]
    public void create_ui_controller() {
        ui_builder = new GameObject().AddComponent<Manual_figure_builder>();
    }
}

}