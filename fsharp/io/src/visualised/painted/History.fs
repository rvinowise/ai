namespace rvinowise.ai.ui.painted

    open Rubjerg
    open Rubjerg.Graphviz

    open rvinowise.ui
    open rvinowise.ai


    module History =
        
        let description history=
            $"appearances of {history.figure} from {history.interval.finish} to {history.interval.start}"
        
        let combined_description 
            (histories: Figure_history seq)
            =
            let figures =
                histories
                |>Seq.map History.figure
                |>String.concat ","

            let border = 
                histories
                |>Seq.map History.interval
                |>Interval.bordering_interval

            $"appearances of {figures} from {border.start} to {border.finish}"

        let fill_batch_with_events
            events
            (batch_graph: infrastructure.Graph)
            =
            events
            |>Seq.iter (fun (event:Appearance_event) ->
                batch_graph
                |>infrastructure.Graph.with_vertex
                    (
                        match event with
                        |Start figure -> "("+figure
                        |Finish figure -> figure+")"
                    )
                |>ignore
            )

        let add_next_event_batch 
            (moment:Moment)
            (events: Appearance_event seq)
            (graph: infrastructure.Graph)
            =
            graph
            |>infrastructure.Graph.with_cluster 
                (string moment)
                (fill_batch_with_events events)


        let add_figure_histories
            histories
            graph
            =
            histories
            |>History.combine
            |>Seq.iter (fun pair->
                graph
                |>add_next_event_batch 
                    pair.Key
                    pair.Value
                |>ignore
            )
            graph

        
        let as_graph 
            histories 
            =
            let description = 
                histories
                |>combined_description
            
            "unused text"
            |>infrastructure.Graph.empty
            |>infrastructure.Graph.with_rectangle_vertices
            |>infrastructure.Graph.provide_cluster description
            |>add_figure_histories histories
        
        
        let as_graph' 
            histories 
            =
            let description = 
                histories
                |>combined_description
            
            let graph = 
                "unused text"
                |>infrastructure.Graph.empty
                |>infrastructure.Graph.with_rectangle_vertices
                |>infrastructure.Graph.provide_cluster description
                
            graph
            //|>add_figure_histories histories
            |>add_next_event_batch 
                1
                [Start "a";Start "b";Start "c"]
            |>add_next_event_batch 
                2
                [Start "d";Start "e";Start "f"]
            |>add_next_event_batch 
                3
                [Start "j";Start "o";Start "i"]
            |>add_next_event_batch 
                4
                [Start "j";Start "o";Start "p"]

        
        