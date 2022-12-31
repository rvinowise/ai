namespace rvinowise.ai.ui.painted

    open Rubjerg
    open Rubjerg.Graphviz

    open rvinowise.ui
    open rvinowise.ai


    module History =
        
        let description history=
            $"appearences of {history.figure} from {history.interval.tail} to {history.interval.head}"

        

        let add_figure_histories
            histories
            (graph:infrastructure.Graph)
            =
            histories
            |>History.combine
            |>Seq.map

        


        