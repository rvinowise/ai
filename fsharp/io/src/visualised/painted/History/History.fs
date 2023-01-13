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


        let description (history:Figure_history)=
            $"appearances of {history.figure} from {history.interval.finish} to {history.interval.start}"
        
        let combined_description 
            (histories: Figure_history seq)
            =
            let figures =
                histories
                |>Seq.map ai.figure.History.figure
                |>String.concat ","

            let border = 
                histories
                |>Seq.map ai.figure.History.interval
                |>Interval.bordering_interval_of_intervals

            $"appearances of {figures} from {border.start} to {border.finish}"


        let graphically_add_event_batches 
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


        let graphically_connect_events_start_to_finish
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

        let graphically_arrange_event_batches_sequentially
            (batches:Map<Moment, (Event_batch*infrastructure.Node) > )
            =
            batches
            |>Seq.map (extensions.KeyValuePair.value>>snd)
            |>Seq.pairwise
            |>Seq.iter arrange_two_batches
            
            batches
            
        let add_histories
            figure_histories
            mood_changes_history
            node
            =
            figure_histories
            |>ai.Combined_history.combine_figure_histories
            |>ai.Combined_history.add_mood_changes_to_combined_history mood_changes_history
            |>graphically_add_event_batches node
            |>graphically_connect_events_start_to_finish
            |>graphically_arrange_event_batches_sequentially
            |>ignore
            node

        
        let as_graph 
            figure_histories
            mood_changes_history 
            =
            let description = 
                figure_histories
                |>combined_description
            
            description
            |>infrastructure.Graph.empty
            |>infrastructure.Graph.with_rectangle_vertices
            |>add_histories figure_histories mood_changes_history
        
        

        
        