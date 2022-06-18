using System.Collections.Generic;
using System.IO.Abstractions;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using rvinowise.ai.general;
using System.Linq;
using System.Numerics;
using rvinowise.ai.simple;
using UnityEngine;

namespace rvinowise.ai.unit_tests.file_input {

[TestFixture]
public class text_file_is_loaded
{
    private readonly INetwork<Figure> network = simple.Network.get_empty_network();

    private File_input<Figure> file_input;
    private const string filename = "./input.txt";

    private const string input_signals = "0102310302010";
    private IDictionary<string, BigInteger[]> expected_appearances = new Dictionary<string, BigInteger[]>
    {
        ["0"] = new BigInteger[]{0, 2, 6, 8, 10, 12},
        ["1"] = new BigInteger[]{1, 5, 11},
        ["2"] = new BigInteger[]{3,9},
        ["3"] = new BigInteger[]{4,7},
    }; 
    
    [Test]
    public void history_is_loaded_to_network() {
        
        var mock = new Mock<IFileSystem>();
        mock.Setup(
            fs => fs.File.ReadAllText(
                It.IsAny<string>()
            )
        ).Returns(input_signals);
        mock.Setup(
            fs => fs.File.Exists(
                It.IsAny<string>()
            )
        ).Returns(true);
        
        file_input = new File_input<Figure>(
            network.action_history,
            network.figure_provider,
            mock.Object
        );
        
        file_input.read_file(filename);

        verify_loaded_action_groups(network.action_history, input_signals);
        verify_loaded_figures_appearances(network.figure_provider, expected_appearances);
    }

    private void verify_loaded_action_groups(
        IAction_history action_history,
        string text_signals
    ) {
        var action_groups = 
            action_history.get_action_groups(0, action_history.last_moment);

        Assert.AreEqual(
            action_groups.Count,
            text_signals.Length
        );
    }
    
    private void verify_loaded_figures_appearances(
        IFigure_provider<Figure> figure_provider,
        IDictionary<string, BigInteger[]> expected_moments
    ) {
        
        
        int i_signal = 0;
        foreach (IFigure figure in figure_provider.get_known_figures()) {
            verify_loaded_figure_appearances(
                figure,
                expected_moments[figure.id]
            );
        }
    }
    
    private void verify_loaded_figure_appearances(
        IFigure figure,
        BigInteger[] expected_moments
    ) {
        
        
        int i_appearance = 0;
        foreach (IFigure_appearance appearance in figure.get_appearances()) {
            
            Assert.AreEqual(
                appearance.start_moment,
                appearance.end_moment
            );
            Assert.AreEqual(
                appearance.start_moment,
                expected_moments[i_appearance]
            );
            
            i_appearance++;
        }
    }
}



}