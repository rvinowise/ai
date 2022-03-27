
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.unity {

public class Figure_provider:
    IFigure_provider 
{
    
    private readonly ai.simple.Figure_provider figure_provider = new ai.simple.Figure_provider();
    private readonly Figure figure_prefab;
    private readonly Mode_selector mode_selector;

    public Figure_provider(
        Figure figure_prefab
    ) {
        this.figure_prefab = figure_prefab;
    }
    
    public Figure_provider(
        Figure figure_prefab,
        Mode_selector mode_selector
    ) {
        this.figure_prefab = figure_prefab;
        this.mode_selector = mode_selector;
    }
    
  
    

    #region IFigure_provider

    public IReadOnlyList<IFigure> get_known_figures() => figure_provider.get_known_figures();
    
    public IFigure provide_sequence_for_pair(
        IFigure beginning_figure,
        IFigure ending_figure
    ) => figure_provider.provide_sequence_for_pair(beginning_figure, ending_figure);
    
    public IFigure create_figure(string prefix = "") {
        Figure new_figure = figure_prefab.provide_new<Figure>();
        new_figure.id = figure_provider.get_next_id_for_prefix(prefix);
        new_figure.header.mode_selector = mode_selector;
        return new_figure;
    }
    
    public IFigure create_base_signal(string id = "") {
        Figure signal = create_figure("signal") as Figure;
        signal.name = $"signal {signal.id}";
        return signal;
    }
    
    public IFigure find_figure_with_id(string id) =>
        figure_provider.find_figure_with_id(id);

    public void remove_figure(IFigure figure) => figure_provider.remove_figure(figure);

    #endregion IFigure_provider



}
}