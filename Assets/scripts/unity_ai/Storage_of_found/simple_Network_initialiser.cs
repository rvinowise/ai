using rvinowise.ai.general;
using rvinowise.rvi.contracts;



namespace rvinowise.ai.simple {
public class Network_initialiser:
    INetwork_initialiser
{
    private readonly IFigure_provider figure_provider;
    
    private readonly string[] symbol_figures = {",",";","=","+","-"};


    public Network_initialiser(
        IFigure_provider in_figure_provider
    ) {
        figure_provider = in_figure_provider;
    }

    #region INetwork_initialiser

    public void create_base_signals() {
        foreach(string pattern_id in symbol_figures) {
            IFigure figure = figure_provider.create_figure(
                pattern_id
            );
            Contract.Ensures(figure != null);
        }
        for (int i=0;i<=9;i++) {
            IFigure figure = figure_provider.create_figure(
                get_id_for_index(i)
            );
            Contract.Ensures(figure != null);
        }
    }

    #endregion INetwork_initialiser

    private string get_id_for_index(int in_index) {
        return $"{in_index}";
    }
    
}
}