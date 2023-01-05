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
                |>Seq.map History.figure
                |>String.concat ","

            let border = 
                histories
                |>Seq.map History.interval
                |>Interval.bordering_interval

            $"appearances of {figures} from {border.start} to {border.finish}"

        let add_events_to_combined_history 
            history 
            (combined_history: Combined_history)
            //(map: Map<Moment, Set<Appearance_event> >)
            = 
            history.appearances
            |>Seq.fold (fun (combined:Combined_history) interval->
                {combined_history with 
                    batches=
                        let start_batch = 
                            combined_history.batches
                            |>Map.getOrDefault interval.start {events=Map.empty;node=None}
                        let start_batch = {
                            start_batch with events=start_batch.events
                                            |>Map.add (Start history.figure) None 
                        }    
                        
                        let end_batch = 
                            combined_history.batches|>
                            Map.getOrDefault interval.finish {events=Map.empty;node=None}
                        let end_batch = {
                            end_batch with events=end_batch.events
                                            |>Map.add (Finish (history.figure, interval.start)) None 
                        } 
                        combined_history.batches
                        |>Map.add interval.start start_batch
                        |>Map.add interval.finish end_batch
                }
            ) combined_history

        let combine histories =
            histories
            |>Seq.fold (fun combined history->
                add_events_to_combined_history history combined
            ) {batches=Map.empty}

        [<Fact>]
        let ``combine figure histories``()=
            let history_of_a = history.built.from_tuples "a" [
                0,1; 2,4
            ]
            let history_of_b = history.built.from_tuples "b" [
                0,2; 4,4
            ]
            [history_of_a; history_of_b]
            |>combine 
            |>should equal Map[
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
            }

        let add_event_batches 
            (receptacle: infrastructure.Node)
            (combined_history: Combined_history)
            =
            {combined_history with
                batches =
                combined_history.batches
                |>Seq.map(fun pair->pair.Key, pair.Value)
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

        let connect_start_to_finish 
            (history: Combined_history)
            =
            history.batches
            |>
            node.data.children
            |>Seq.map
            |>infrastructure.Graph.provide_vertex (string moment)

        let connect_two_batches
            start finish
            =
            start.batch
            |>infrastructure.Graph.with_edge finish.batch

        let connect_event_batches
            (history: Combined_history)
            =
            history.batches
            |>extensions.Map.toPairs
            |>Seq.sort
            |>Seq.pairwise
            ||>connect_two_batches

        let add_figure_histories
            histories
            node
            =
            histories
            |>combine
            |>add_event_batches node
            |>connect_start_to_finish
            |>connect_event_batches
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
        
        

        
        