namespace rvinowise.ai.ui.painted

open Giraffe.ViewEngine

open rvinowise
open rvinowise.ui
open rvinowise.ui.infrastructure
open rvinowise.ai

    
module History =


    let combined_history_description 
        (history: Event_batch seq)
        =
        $"history from 0 to {Seq.length history}"


    let add_event_batches 
        (receptacle: infrastructure.Node)
        (event_batches: Appearance_event list seq)
        =
        event_batches
        |>Seq.mapi (fun moment batch ->
            (
                moment,
                (   
                    batch,
                    receptacle
                    |>infrastructure.Graph.provide_html_vertex (
                        batch
                        |>Batch_html.layout_for_event_batch moment (Mood 0) (Mood 0)
                        |>RenderView.AsString.htmlNode
                        |>sprintf "<\n%s\n>"
                    )
                )
            )
        )
        |>Map


    let connect_events_start_to_finish
        (batches:Map<Moment, (Appearance_event list*infrastructure.Node) > )
        =
        batches
        |>Seq.map extensions.KeyValuePair.value
        |>Seq.iter(fun (events,node) ->
            events
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
        (batches:Map<Moment, infrastructure.Node > )
        =
        batches
        |>Seq.map (extensions.KeyValuePair.value)
        |>Seq.pairwise
        |>Seq.iter arrange_two_batches
        
        batches
        
    let add_combined_history 
        history 
        node
        =
        node
        |>infrastructure.Graph.with_rectangle_vertices
        |>ignore
        
        history
        |>add_event_batches node
        |>connect_events_start_to_finish
        |>Map.map (fun _ (_, node)->node)
        |>arrange_event_batches_sequentially
        |>ignore
        


    
    