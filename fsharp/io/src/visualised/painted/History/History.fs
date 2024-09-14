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
            
            moment,
            (   
                batch,
                receptacle
                |>Graph.create_html_vertex (
                    batch
                    |>Batch_html.layout_for_event_batch moment (Mood 0) (Mood 0)
                    |>RenderView.AsString.htmlNode
                    |>sprintf "<\n%s\n>"
                )
            )
        )
        |>Map


    let connect_events_start_to_finish
        (batches: Map<Moment, Appearance_event list * Node > )
        =
        batches
        |>Map.values
        |>Seq.iter(fun (events,node_of_batch) ->
            events
            |>Seq.iter(fun event ->
                match event with 
                |Finish (figure, start_moment)->
                    let _, node_of_starting_batch = batches[start_moment]
                    (node_of_starting_batch, $"({figure}")
                    ||>Graph.provide_edge_between_ports  node_of_batch $"{figure})"
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
        |>Map.values
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
        


    
    