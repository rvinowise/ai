using System.Collections.Generic;


namespace rvinowise.ai.general {

public interface IFigure_provider<out TFigure> 
     where TFigure: class?, IFigure 
{

     IReadOnlyList<TFigure> get_known_figures();
     public TFigure provide_figure(string id);

     TFigure provide_sequence_for_pair(
          IFigure beginning_figure,
          IFigure ending_figure
     );

     TFigure find_figure_with_id(string id);
     public string get_next_id_for_prefix(string prefix);
    
     void remove_figure(IFigure figure);
}
}