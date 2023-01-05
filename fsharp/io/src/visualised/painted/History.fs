namespace rvinowise.ai.ui.painted


    open rvinowise.ui
    open rvinowise.ui.infrastructure
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


        let get_start_of_appearance 
            appeared_figure
            moment
            

        let connect_finish_to_start
            start_node
            finish_node
            =
            start_node
            |>infrastructure.Graph.with_edge finish_node

        let fill_batch_with_events
            (created_starts: Map<Moment, seq<infrastructure.Node> >)
            events
            (batch_cluster: infrastructure.Node)
            =
            events
            |>Seq.iter (fun (event:Appearance_event) ->
                let new_vertex = 
                    batch_cluster
                    |>infrastructure.Graph.provide_vertex (
                        match event with
                        |Start figure -> "("+figure
                        |Finish (figure, _) -> figure+")"
                    )
                match event with
                |Start figure ->
                    created_starts = created_starts|>extensions.Map.add_by_key 
                |Finish (figure, start_moment) ->
                    let start_vertex = get_start_of_appearance new_vertex.data.id
                    connect_finish_to_start new_vertex

            )
            batch_cluster

        let add_event_batch 
            (receptacle: infrastructure.Node)
            (moment:Moment,
            events: Appearance_event seq)
            =
            receptacle
            |>infrastructure.Graph.provide_vertex (string moment)
            |>fill_batch_with_events events

        let connect_start_to_finish 
            histories
            (start_node:infrastructure.Node)
            =
            node.data.children
            |>Seq.map
            |>infrastructure.Graph.provide_vertex (string moment)

        let connect_event_batches
            (tail, head)
            =
            tail
            |>infrastructure.Graph.with_edge head
            |>ignore

        let add_figure_histories
            histories
            node
            =
            histories
            |>History.combine
            |>Seq.map (fun pair->pair.Key,pair.Value)
            |>Seq.map (add_event_batch 
                node
            )
            |>Seq.map (connect_start_to_finish histories)
            |>Seq.pairwise
            |>Seq.iter (connect_event_batches)
            node

        
        let as_graph 
            histories 
            =
            let description = 
                histories
                |>combined_description
            
            description
            |>infrastructure.Graph.empty
            |>infrastructure.Graph.with_rectangle_vertices
            |>add_figure_histories histories
        
        
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

        
        