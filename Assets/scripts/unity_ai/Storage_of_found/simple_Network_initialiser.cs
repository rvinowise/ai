using rvinowise.ai.general;
using rvinowise.rvi.contracts;



namespace rvinowise.ai.simple {
public class Network_initialiser:
    INetwork_initialiser
{
    private IFigure_storage figure_storage;
    
    private readonly string[] symbol_figures = {",",";","=","+","-"};

    public delegate IFigure Create_base_signal(string id);

    private Create_base_signal create_base_signal;
    
    public Network_initialiser() {
        create_base_signal = create_simple_base_signal;
    }
    
    public Network_initialiser(
        IFigure_storage in_storage,
        Create_base_signal in_create_base_signal = null
    ) {
        if (in_create_base_signal == null) {
            create_base_signal = create_simple_base_signal;
        } else {
            create_base_signal = in_create_base_signal;
        }
        figure_storage = in_storage;
    }
    
    public void create_base_signals() {
        foreach(string pattern_id in symbol_figures) {
            IFigure figure = create_base_signal(
                pattern_id
            );
            Contract.Ensures(figure != null);
            figure_storage.append_figure(figure);
        }
        for (int i=0;i<=9;i++) {
            IFigure figure = create_base_signal(
                get_id_for_index(i)
            );
            Contract.Ensures(figure != null);
            figure_storage.append_figure(figure);
        }
    }

    public IFigure create_simple_base_signal(string id) {
        IFigure signal = new Figure(id);
        return signal;
    }
    
    private string get_id_for_index(int in_index) {
        return $"{in_index}";
    }
    
}
}