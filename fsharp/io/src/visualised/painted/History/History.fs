namespace rvinowise.ai.ui.painted
    open FsUnit
    open Xunit

    open Giraffe.ViewEngine

    open rvinowise
    open rvinowise.extensions
    open rvinowise.ui
    open rvinowise.ui.infrastructure
    open rvinowise.ai

    module History=
        let figure_history_description (history:Figure_history)=
            $"appearances of {history.figure} from {history.interval.finish} to {history.interval.start}"
        
        let combined_history_description 
            (history: Combined_history)
            =
            let border = 
                history
                |>Combined_history.interval

            $"history from {border.start} to {border.finish}"


        let add_event_batches 
            (receptacle: infrastructure.Node)
            (combined_history: Combined_history)
            =
            combined_history.batches
            |>Map.toPairs
            |>Seq.map (fun (moment, batch) ->
                (
                    moment,
                    (   
                        batch,
                        receptacle
                        |>infrastructure.Graph.provide_html_vertex (
                            batch
                            |>Batch_html.layout_for_event_batch moment
                            |>RenderView.AsString.htmlNode
                            |>sprintf "<\n%s\n>"
                        )
                    )
                )
            )
            |>Map


        let connect_events_start_to_finish
            (batches:Map<Moment, (Event_batch*infrastructure.Node) > )
            =
            batches
            |>Seq.map extensions.KeyValuePair.value
            |>Seq.iter(fun (batch,node) ->
                batch.events
                |>Seq.iter(fun event ->
                    match event with 
                    |Finish (figure, start_moment)->
                        let _, start_node = batches[start_moment]
                        (start_node, $"({figure}")
                        ||>Graph.provide_edge_between_ports  node $"{figure})"
                        |>ignore
                        
                    |_ ->()
                )
            )
            batches

        let arrange_two_batches
            (start, finish)
            =
            start
            |>infrastructure.Graph.provide_edge finish
            |>infrastructure.Edge.with_attribute "style" "invis"
            |>ignore

        let arrange_event_batches_sequentially
            (batches:Map<Moment, (Event_batch*infrastructure.Node) > )
            =
            batches
            |>Seq.map (extensions.KeyValuePair.value>>snd)
            |>Seq.pairwise
            |>Seq.iter arrange_two_batches
            
            batches
            
        let add_combined_history 
            history 
            node
            =
            node
            |>infrastructure.Graph.with_rectangle_vertices
            
            history
            |>add_event_batches node
            |>connect_events_start_to_finish
            |>arrange_event_batches_sequentially
            |>ignore
            
        
        let as_graph 
            combined_history 
            =
            let graph=
                combined_history
                |>combined_history_description
                |>infrastructure.Graph.empty
                
            graph|>add_combined_history combined_history
            graph

        
        