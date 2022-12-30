namespace rvinowise.ai.ui.painted

    open Rubjerg
    open Rubjerg.Graphviz

    open rvinowise.ui
    open rvinowise.ai


    module History =
        
        let add_fired_figures
            ensemble
            graph
            =
            ensemble.fired
            |>Seq.iter (fun fired_figure ->
                graph
                |>infrastructure.Graph.with_vertex fired_figure
                |>ignore
            )
            

        let add_ensemble_to_graph
            (graph: infrastructure.Graph)
            (moment:Moment)
            ensemble
            =
            graph
            |>infrastructure.Graph.with_cluster 
                (moment.ToString())
                (add_fired_figures ensemble)
            ()

        let add_ensembles
            history
            (graph:infrastructure.Graph)
            =
            history.ensembles
            |>Seq.iteri (fun index ensemble ->
                add_ensemble_to_graph 
                    graph
                    (uint64(index))
                    ensemble
            )

        let add_history
            history
            (graph:infrastructure.Graph)
            =
            graph
            |>add_ensembles history

        let description history=
            $"history from {history.interval.tail} to {history.interval.head}"


        