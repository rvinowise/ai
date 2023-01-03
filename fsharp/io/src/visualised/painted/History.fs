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
            (owner_graph: infrastructure.Graph)
            events
            (batch_cluster: infrastructure.Node)
            =
            events
            |>Seq.iter (fun (event:Appearance_event) ->
                batch_cluster
                |>infrastructure.Graph.with_vertex owner_graph
                    (
                        match event with
                        |Start figure -> "("+figure
                        |Finish figure -> figure+")"
                    )
                |>ignore
            )
            batch_cluster

        let add_next_event_batch 
            (owner_graph: infrastructure.Graph)
            (moment:Moment)
            (events: Appearance_event seq)
            (receptacle: infrastructure.Node)
            =
            receptacle
            |>infrastructure.Graph.with_vertex owner_graph 
                (string moment)
            |>fill_batch_with_events owner_graph events


        let add_figure_histories
            histories
            graph
            =()
            // histories
            // |>History.combine
            // |>Seq.iter (fun pair->
            //     graph.root
            //     |>add_next_event_batch graph
            //         pair.Key
            //         pair.Value
            //     |>ignore
            // )
            // graph

        
        let as_graph 
            histories 
            =()
            // let description = 
            //     histories
            //     |>combined_description
            
            // "unused text"
            // |>infrastructure.Graph.empty
            // |>infrastructure.Graph.with_rectangle_vertices
            // |>infrastructure.Graph.provide_vertex description
            // |>add_figure_histories histories
        
        
        let as_graph' 
            histories 
            =()
            // let description = 
            //     histories
            //     |>combined_description
            
            // let graph = 
            //     "unused text"
            //     |>infrastructure.Graph.empty
            //     |>infrastructure.Graph.root_node
            //     |>infrastructure.Graph.with_rectangle_vertices
            //     |>infrastructure.Graph.provide_vertex description
                
            // graph
            // |>add_next_event_batch 
            //     1
            //     [Start "a";Start "b";Start "c"]
            // |>add_next_event_batch 
            //     2
            //     [Start "d";Start "e";Start "f"]
            // |>add_next_event_batch 
            //     3
            //     [Start "j";Start "o";Start "i"]
            // |>add_next_event_batch 
            //     4
            //     [Start "j";Start "o";Start "p"]

        
        