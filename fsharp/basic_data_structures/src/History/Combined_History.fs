namespace rvinowise.ai
    open FsUnit
    open Xunit


    open rvinowise
    open rvinowise.extensions
    open rvinowise.ai


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Combined_history =
        
        let interval history =
            let moments = 
                history.batches
                |>Seq.map extensions.KeyValuePair.key
            (
                moments|>Seq.min,
                moments|>Seq.max
            )|>Interval.ofPair


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
            (figure_history : Figure_history)
            (combined_history: Combined_history)
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
            (figure_histories : Figure_history seq)
            =
            figure_histories
            |>Seq.fold (fun combined_history figure_history ->
                add_events_to_combined_history figure_history combined_history
            ) 
                {
                    Combined_history.batches=Map.empty
                }

        [<Fact>]
        let ``combine figure histories``()=
            let history_of_a = ai.figure_history.built.from_tuples "a" [
                0,1; 2,4
            ]
            let history_of_b = ai.figure_history.built.from_tuples "b" [
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


namespace rvinowise.ai.combined_history
    open FsUnit
    open Xunit

    open rvinowise.ai

    module built =
        
        let from_contingent_signals 
            start
            batches
            =
            {
                //interval=Interval.regular start (start+Seq.length(batches)-1)

                batches=
                    batches
                    |>Seq.mapi (fun index (fired_figures: Figure_id seq)->
                        (
                            start+index,
                            Event_batch.ofSignals 0 fired_figures
                        )
                    )|>Map.ofSeq
            }
        
        [<Fact>]
        let ``history interval can start from any moment``()=
            let history = 
                from_contingent_signals 0 [
                    ["a";"x"];
                    ["b";"y"];
                    ["a";"z";"x"];
                    ["c"];
                    ["b";"x"];
                    ["b"];
                    ["a"];
                    ["c"]
                ]
            history
            |>Combined_history.interval
            |>should equal
                (Interval.regular 0 7)


        let from_figure_and_mood_histories
            figure_histories
            mood_changes_history
            =
            figure_histories
            |>Combined_history.combine_figure_histories
            |>Combined_history.add_mood_changes_to_combined_history mood_changes_history
        

    module example=
        let short_history_with_some_repetitions=
            built.from_contingent_signals 0 [
                    ["a";"x"];
                    ["b";"y"];
                    ["a";"z";"x"];
                    ["c"];
                    ["b";"x"];
                    ["b"];
                    ["a"];
                    ["c"]
                ]
        
        