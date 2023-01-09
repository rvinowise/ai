namespace rvinowise.ai.ui.painted
    open FsUnit
    open Xunit

    open rvinowise
    open rvinowise.extensions
    open rvinowise.ui
    open rvinowise.ui.infrastructure
    open rvinowise.ai

    type Appearance_event=
    |Start of Figure_id
    |Finish of Figure_id * Moment
    |Mood_change of int

    type Event_batch = {
        events: Map<Appearance_event, infrastructure.Node option>
        mood:Mood
        node: infrastructure.Node option
    }

    type Combined_history = {
        batches: Map<Moment, Event_batch>
    }

    module History =
        
        let description history=
            $"appearances of {history.figure} from {history.interval.finish} to {history.interval.start}"
        
        let combined_description 
            (histories: Figure_history seq)
            =
            let figures =
                histories
                |>Seq.map figure.History.figure
                |>String.concat ","

            let border = 
                histories
                |>Seq.map figure.History.interval
                |>Interval.bordering_interval_of_intervals

            $"appearances of {figures} from {border.start} to {border.finish}"

        let add_events_to_combined_history 
            figure_history 
            (combined_history: Combined_history)
            //(map: Map<Moment, Set<Appearance_event> >)
            = 
            figure_history.appearances
            |>Seq.fold (fun combined interval->
                {combined with 
                    batches=
                        let combined_batches =
                            combined.batches
                            |>Map.add interval.start (
                                let start_batch = 
                                    combined.batches
                                    |>Map.getOrDefault interval.start {
                                        events=Map.empty
                                        mood=0
                                        node=None
                                    }
                                {start_batch with 
                                    events=
                                        start_batch.events
                                        |>Map.add (Start figure_history.figure) None 
                                }
                            )
                        combined_batches
                        |>Map.add interval.finish (
                            let end_batch = 
                                combined_batches
                                |>Map.getOrDefault interval.finish {
                                    events=Map.empty
                                    mood=0
                                    node=None
                                }
                            {end_batch with 
                                events=
                                    end_batch.events
                                    |>Map.add (Finish (figure_history.figure, interval.start)) None 
                            }
                        )
                        
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
                                |>Map.getOrDefault moment {
                                    events=Map.empty
                                    mood=0
                                    node=None
                                }
                            {previous_batch with 
                                events=
                                    previous_batch.events
                                    |>Map.add (Mood_change mood_change) None 
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
            let event_batch_without_graphics batch=
                batch.events
                |>Seq.map (fun pair->pair.Key)

            let history_of_a = figure.history.built.from_tuples "a" [
                0,1; 2,4
            ]
            let history_of_b = figure.history.built.from_tuples "b" [
                0,2; 4,4
            ]
            [history_of_a; history_of_b]
            |>combine_figure_histories 
            |>fun history->history.batches
            |>Map.toPairs
            |>Seq.map (fun (moment,batch) ->
                moment, (event_batch_without_graphics batch)
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
                    Start "b";
                    Finish ("a",2);
                    Finish ("b",4)
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

        let represent_batch_graphically
            (batch: Event_batch)
            (painted_batch: infrastructure.Node)
            =
            {batch with
                events=
                    batch.events
                    |>Seq.map (fun pair->pair.Key, pair.Value)
                    |>Seq.map (fun (event, vertex) ->
                        (
                            event,
                            Some( 
                                painted_batch
                                |>infrastructure.Graph.provide_vertex (
                                    match event with
                                    |Start figure -> "("+figure
                                    |Finish (figure, _) -> figure+")"
                                    |Mood_change value ->
                                        if value>0 then
                                            $"+{value}"
                                        else
                                            $"{value}"
                                )
                            )
                        )
                    )
                    |>Map
                
                node= Some painted_batch
            }

        let graphically_add_event_batches 
            (receptacle: infrastructure.Node)
            (combined_history: Combined_history)
            =
            {combined_history with
                batches =
                combined_history.batches
                |>Map.toPairs
                |>Seq.map (fun (moment, batch) ->
                    (
                        moment,
                        receptacle
                        |>infrastructure.Graph.provide_vertex (string moment)
                        |>represent_batch_graphically batch
                    )
                )
                |>Map
            }

        let find_appearance_start_in_history
            history
            appeared_figure
            start_moment
            =
            history.batches
            |>Map.find start_moment
            |>fun batch->batch.events
            |>Map.find (Start appeared_figure)
            |>Option.value_exc

        let connect_events_start_to_finish_in_batch
            history
            batch
            =
            batch.events
            |>Map.toPairs
            |>Seq.iter(fun (event, node)->
                match event with 
                |Finish (figure,start_moment)->
                    match node with
                    |Some finish_node ->
                        let start_node =
                            find_appearance_start_in_history
                                history
                                figure
                                start_moment
                        start_node
                        |>Graph.with_edge  finish_node
                        |>ignore
                    |None ->raise (System.ApplicationException(
                        "graphic node should be written into the event batch at this point"
                    ))
                |_ ->()
            )

        let arrange_events_inside_batch
            (batch:Event_batch)
            =
            batch.events
            |>Map.toPairs
            |>Seq.pairwise
            |>Seq.iter(fun ((_, start_node), (_, finish_node))->
                match (start_node, finish_node) with
                | Some start_node,Some finish_node->
                    start_node
                    |>infrastructure.Graph.with_edge finish_node
                    |>ignore
                |_->()
            )
            match batch.node with
            |Some node->
                node
                |>infrastructure.Graph.with_perpendicular_children
                |>ignore
            |None->()
            
            batch


        let arrange_events_inside_batches history =
            {history with
                batches=
                    history.batches
                    |>Seq.map (fun pair->
                        pair.Key, (arrange_events_inside_batch pair.Value)
                    )
                    |>Map.ofSeq
            }

        let graphically_connect_events_start_to_finish 
            (history: Combined_history)
            =
            history.batches
            |>Seq.map (fun pair->pair.Value)
            |>Seq.iter (connect_events_start_to_finish_in_batch history)

            history

        let arrange_two_batches
            (start, finish)
            =
            start
            |>infrastructure.Graph.provide_edge finish
            |>infrastructure.Edge.with_attribute "style" "invis"
            |>ignore

        let graphically_arrange_event_batches_sequentially
            (history: Combined_history)
            =
            history.batches
            |>Seq.map(fun pair -> pair.Value)
            |>Seq.choose (fun batch -> batch.node)
            |>Seq.pairwise
            |>Seq.iter arrange_two_batches
            
            history
            
        let add_histories
            figure_histories
            mood_changes_history
            node
            =
            figure_histories
            |>combine_figure_histories
            |>add_mood_changes_to_combined_history mood_changes_history
            //|>add_mood_history mood_changes_history
            |>graphically_add_event_batches node
            |>arrange_events_inside_batches
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
        
        

        
        