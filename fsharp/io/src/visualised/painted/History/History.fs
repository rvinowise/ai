namespace rvinowise.ai.ui.painted
    open FsUnit
    open Xunit

    open Giraffe.ViewEngine

    open rvinowise
    open rvinowise.extensions
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
                |>Seq.map ai.figure.History.figure
                |>String.concat ","

            let border = 
                histories
                |>Seq.map ai.figure.History.interval
                |>Interval.bordering_interval_of_intervals

            $"appearances of {figures} from {border.start} to {border.finish}"


        let add_signal_to_history 
            figure
            moment
            (batches:Map<Moment, Event_batch>)
            =
            batches
                |>Map.add moment (
                    let start_batch = 
                        batches
                        |>Map.getOrDefault moment Event_batch.empty
                    {start_batch with 
                        events=
                            start_batch.events
                            |>Seq.append [Signal figure]
                    }
                )

        let add_start_and_finish_to_history
            figure
            (interval:Interval)
            (batches:Map<Moment, Event_batch>)
            =
            let combined_batches =
                batches
                |>Map.add interval.start (
                    let start_batch = 
                        batches
                        |>Map.getOrDefault interval.start Event_batch.empty
                    {start_batch with 
                        events=
                            start_batch.events
                            |>Seq.append [Start figure]
                    }
                )
            combined_batches
            |>Map.add interval.finish (
                let end_batch = 
                    combined_batches
                    |>Map.getOrDefault interval.finish Event_batch.empty
                {end_batch with 
                    events=
                        end_batch.events
                        |>Seq.append [Finish (figure, interval.start)] 
                }
            )

        let add_events_to_combined_history 
            figure_history 
            (combined_history: Combined_history)
            //(map: Map<Moment, Set<Appearance_event> >)
            = 
            figure_history.appearances
            |>Seq.fold (fun combined interval->
                {combined with 
                    batches=
                        if interval.start = interval.finish then
                            add_signal_to_history
                                figure_history.figure
                                interval.start
                                combined.batches
                        else
                            add_start_and_finish_to_history
                                figure_history.figure
                                interval
                                combined.batches
                }
            ) combined_history

        let add_mood_changes_to_combined_history 
            (mood_changes_history: Mood_changes_history) 
            (combined_history: Combined_history)
            //(map: Map<Moment, Set<Appearance_event> >)
            = 
            mood_changes_history.changes
            |>Seq.fold (fun combined moment_mood ->
                {combined with 
                    batches=
                        let moment = moment_mood.Key
                        let mood_change = moment_mood.Value
                        combined.batches
                        |>Map.add moment (
                            let previous_batch = 
                                combined.batches
                                |>Map.getOrDefault moment Event_batch.empty
                            {previous_batch with 
                                events=
                                    previous_batch.events
                                    |>Seq.append [Mood_change mood_change] 
                            }
                        )
                        
                }
            ) combined_history

        let combine_figure_histories 
            figure_histories 
            =
            figure_histories
            |>Seq.fold (fun combined_history figure_history->
                add_events_to_combined_history figure_history combined_history
            ) {batches=Map.empty}

        [<Fact>]
        let ``combine figure histories``()=
            let history_of_a = ai.figure.history.built.from_tuples "a" [
                0,1; 2,4
            ]
            let history_of_b = ai.figure.history.built.from_tuples "b" [
                0,2; 4,4
            ]
            [history_of_a; history_of_b]
            |>combine_figure_histories 
            |>fun history->history.batches
            |>Map.toPairs
            |>Seq.map (fun (moment,batch) ->
                moment, (batch.events|>Seq.sort)
            )
            |>should equal [
                0,[
                    Start "a";
                    Start "b"
                ];
                1,[
                    Finish ("a",0)
                ];
                2,[
                    Start "a";
                    Finish ("b",0)
                ];
                4,[
                    Finish ("a",2);
                    Signal "b"
                ]
            ]

        let add_mood_history 
            (mood_changes_history: Mood_changes_history) 
            (combined_history: Combined_history)
            =
            ()


        let connect_finish_to_start
            start_node
            finish_node
            =
            start_node
            |>infrastructure.Graph.with_edge finish_node

        


        
                

    

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
                        //|>infrastructure.Graph.provide_vertex ("batch"+ string(moment))
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
            |>Seq.map extensions.KeyValuyePair.value
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
            |>Seq.map (extensions.KeyValuyePair.value>>snd)
            |>Seq.pairwise
            |>Seq.iter arrange_two_batches
            
            batches
            
        let add_histories
            figure_histories
            mood_changes_history
            node
            =
            figure_histories
            |>combine_figure_histories
            |>add_mood_changes_to_combined_history mood_changes_history
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
        
        

        
        