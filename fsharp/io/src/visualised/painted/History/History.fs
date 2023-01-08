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

    type Event_batch = {
        events: Map<Appearance_event, infrastructure.Node option>
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

        let add_figure_events_to_combined_history 
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
                                    |>Map.getOrDefault interval.start {events=Map.empty;node=None}
                                {start_batch with 
                                    events=start_batch.events
                                                    |>Map.add (Start figure_history.figure) None 
                                }
                            )
                        combined_batches
                        |>Map.add interval.finish (
                            let end_batch = 
                                combined_batches
                                |>Map.getOrDefault interval.finish {events=Map.empty;node=None}
                            {end_batch with 
                                events=end_batch.events
                                                |>Map.add (Finish (figure_history.figure, interval.start)) None 
                            }
                        )
                        
                }
            ) combined_history

        let combine histories =
            histories
            |>Seq.fold (fun combined_history figure_history->
                add_figure_events_to_combined_history figure_history combined_history
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
            |>combine 
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
                                )
                            )
                        )
                    )
                    |>Map
                
                node= Some painted_batch
            }

        let add_graphic_event_batches 
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

        let connect_events_of_batch_to_their_pairs
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
                |Start figure->()
            )

        let connect_events_start_to_finish 
            (history: Combined_history)
            =
            history.batches
            |>Seq.map (fun pair->pair.Value)
            |>Seq.iter (connect_events_of_batch_to_their_pairs history)

            history

        let connect_two_batches
            (start, finish)
            =
            start
            |>infrastructure.Graph.with_edge finish
            |>ignore

        let connect_event_batches
            (history: Combined_history)
            =
            history.batches
            |>Seq.map(fun pair -> pair.Value)
            |>Seq.choose (fun batch -> batch.node)
            |>Seq.pairwise
            |>Seq.iter connect_two_batches
            
            history
            
        let add_figure_histories
            histories
            node
            =
            histories
            |>combine
            |>add_graphic_event_batches node
            |>connect_events_start_to_finish
            |>connect_event_batches
            |>ignore
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
        
        

        
        