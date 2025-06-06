using rvinowise.ai.general;
using rvinowise.contracts;



namespace rvinowise.ai.simple {
public class Base_signals<TFigure>
where TFigure: class?, IFigure
{
    private readonly IFigure_provider<TFigure> figure_provider;
    
    private readonly string[] symbol_figures = {",",";","=","+","-"};


    public Base_signals(
        IFigure_provider<TFigure> in_figure_provider
    ) {
        figure_provider = in_figure_provider;
        create_base_signals();
    }

    private void create_base_signals() {
        foreach(string pattern_id in symbol_figures) {
            IFigure figure = figure_provider.provide_figure(
                pattern_id
            );
            Contract.Ensures(figure != null);
        }
        for (int i=0;i<=9;i++) {
            IFigure figure = figure_provider.provide_figure(
                get_id_for_index(i)
            );
            Contract.Ensures(figure != null);
        }
    }

    private string get_id_for_index(int in_index) {
        return $"{in_index}";
    }
    
}
}