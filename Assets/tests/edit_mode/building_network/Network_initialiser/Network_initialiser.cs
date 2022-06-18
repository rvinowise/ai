using System.Collections.Generic;
using NUnit.Framework;
using rvinowise.ai.general;

using rvinowise.ai.simple;

namespace rvinowise.ai.unit_tests.network_initialiser {

[TestFixture]
public class network_needs_to_be_initialised
{
    private readonly INetwork<Figure> network = simple.Network.get_empty_network();

    private readonly IFigure_provider<Figure> figure_provider;
    private readonly IBase_signals_initializer figure_provider_initialiser;


    private ISet<string> expected_base_signals = new HashSet<string> {
        ",", ";", "=", "+", "-",
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
    };
    
    [Test]
    public void base_signals_are_created_in_network() {
        IFigure_provider<Figure> figure_provider = new Figure_provider<Figure>(create_figure);
        IBase_signals_initializer figure_provider_initialiser = new Base_signals_initializer<Figure>(figure_provider);
        figure_provider_initialiser.create_base_signals();
        Assert.AreEqual(
            expected_base_signals.Count,
            figure_provider.get_known_figures().Count
        );
        validate_figures_ids(figure_provider.get_known_figures(), expected_base_signals);
    }

    private void validate_figures_ids(
        IEnumerable<IFigure> checked_figures,
        ISet<string> expected_base_signals
    ) {
        foreach (var figure in checked_figures) {
            Assert.IsTrue(expected_base_signals.Contains(figure.id));
        }
    }

    private Figure create_figure(string id) {
        return new simple.Figure(id);
    }
}


}