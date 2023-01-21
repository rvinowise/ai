module rvinowise.ai.built.Event_batches
    open FsUnit
    open Xunit
    open System.Collections.Generic

    open rvinowise.ai
    open rvinowise 

        
    let from_contingent_signals 
        start
        batches
        =
        batches
        |>Seq.mapi (fun index (fired_signals: string seq)->
            (
                start+index,
                Event_batch.ofSignalsWithMood fired_signals
            )
        )|>Map.ofSeq
    
    
    [<Fact>]
    let ``construct combined history with mood``()=
        let history = 
            from_contingent_signals 0 [
                ["a";"x"];
                ["+1";"b";"y"];
                ["a";"z";"x"];
                ["+2";"c"];
                ["b";"x"];
                ["b"];
                ["-3";"a"];
                ["c"]
            ]
        history

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
        |>Event_batches.interval
        |>should equal
            (Interval.regular 0 7)

    let from_text (text:string)=
        text
        |>Seq.map (fun symbol->
            match symbol with
            |'×'->seq{"+1"}
            |symbol -> seq{string symbol}
        )
        |>from_contingent_signals 0

    [<Fact>]
    let ``from text``()=
        "1+2=3×"
        |>from_text
        |>should equal (
            from_contingent_signals 0 [
                ["1"];["+"];["2"];["="];["3"];["+1"];
            ]
        )

    let from_text_blocks (text_blocks:string seq)=
        text_blocks
        |>Seq.collect id
        |>string
        |>from_text

    [<Fact>]
    let ``from text blocks``()=
        ["1+2=3";"1"]
        |>from_text_blocks
        |>should equal (
            from_contingent_signals 0 [
                ["1"];["+"];["2"];["="];["3"];["1"]
            ]
        )

    let add_signal_to_history 
        figure
        moment
        (batches:Event_batches)
        =
        batches
            |>Map.add moment (
                let start_batch = 
                    batches
                    |>extensions.Map.getOrDefault moment Event_batch.empty
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
                    |>extensions.Map.getOrDefault interval.start Event_batch.empty
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
                |>extensions.Map.getOrDefault interval.finish Event_batch.empty
            {end_batch with 
                events=
                    end_batch.events
                    |>Seq.append [Finish (figure, interval.start)] 
            }
        )

    let add_events_to_combined_history 
        (figure_history : Figure_history)
        (event_batches: Event_batches)
        = 
        figure_history.appearances
        |>Seq.fold (fun batches interval->
            if interval.start = interval.finish then
                add_signal_to_history
                    figure_history.figure
                    interval.start
                    batches
            else
                add_start_and_finish_to_history
                    figure_history.figure
                    interval
                    batches
            
        ) event_batches

    let combine_figure_histories 
        (figure_histories : Figure_history seq)
        =
        figure_histories
        |>Seq.fold (fun event_batches figure_history ->
            add_events_to_combined_history figure_history event_batches
        ) 
            Map.empty
            
    [<Fact>]
    let ``combine figure histories``()=
        let history_of_a = built.Figure_history.from_tuples "a" [
            0,1; 2,4
        ]
        let history_of_b = built.Figure_history.from_tuples "b" [
            0,2; 4,4
        ]
        [history_of_a; history_of_b]
        |>combine_figure_histories 
        |>extensions.Map.toPairs
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

    let add_mood_to_combined_history 
        (mood_history: Map<Moment, Mood_state>) 
        (event_batches: Event_batches)
        = 
        mood_history
        |>Seq.fold (fun batches moment_mood ->
            let moment = moment_mood.Key
            let mood_change = moment_mood.Value.change
            let mood_value = moment_mood.Value.value
            batches
            |>Map.add moment (
                let previous_batch = 
                    batches
                    |>extensions.Map.getOrDefault moment Event_batch.empty
                {previous_batch with 
                    mood=
                        {
                            change=mood_change
                            value=mood_value
                        }
                }
            )
        ) event_batches

    let from_figure_and_mood_histories
        mood_history
        figure_histories
        =
        figure_histories
        |>combine_figure_histories
        |>add_mood_to_combined_history mood_history
    
    let to_figure_histories
        event_batches
        =
        let figure_appearances = 
            Dictionary<Figure_id, ResizeArray<Interval>>()
        event_batches
        |>extensions.Map.toPairs
        |>Seq.iter (fun ((moment:Moment), (batch: Event_batch)) ->
            batch.events
            |>Seq.iter (function
                |Finish (figure, start_moment) ->
                    let old_appearances=
                        figure_appearances
                        |>extensions.Dictionary.getOrDefault 
                            figure (ResizeArray())
                    old_appearances.Add(Interval.regular start_moment moment)
                    figure_appearances[figure]<- old_appearances
                |Signal figure ->
                    let old_appearances=
                        figure_appearances
                        |>extensions.Dictionary.getOrDefault 
                            figure (ResizeArray())
                    old_appearances.Add(Interval.moment moment)
                    figure_appearances[figure]<- old_appearances
                |_ ->()
            )
        )
        figure_appearances
        |>Seq.map (fun pair ->
            built.Figure_history.from_intervals pair.Key (pair.Value.ToArray())
        )

    let from_combined_histories 
        (histories: Event_batches seq)
        =
        histories
        |>Seq.collect to_figure_histories
        |>combine_figure_histories
    
    let add_figure_histories
        (figure_histories: Figure_history seq)
        (event_batches: Event_batches)
        =
        event_batches
        |>to_figure_histories 
        |>Seq.append figure_histories
        |>combine_figure_histories


    [<Fact>]
    let ``turn a combined history into separate figure histories``()=
        from_contingent_signals 0
            [
                ["a";"b"]//0
                ["c";"d"]//1
                ["a"]//2
                ["b"]//3
            ]
            |>to_figure_histories
            |>should equal [
                built.Figure_history.from_moments "a" [0;2]
                built.Figure_history.from_moments "b" [0;3]
                built.Figure_history.from_moments "c" [1]
                built.Figure_history.from_moments "d" [1]
            ]


    let add_mood_history 
        (mood_changes_history: Map<Moment, Mood>) 
        (event_batches: Event_batches)
        =
        ()
    
    let remove_batches_without_actions (event_batches:Event_batches)=
        event_batches
        |>extensions.Map.toPairs
        |>Seq.filter (fun (moment, batch)->
            (batch.events|>Seq.isEmpty|>not)
            ||
            (batch.mood.change|>Mood.(<>) (Mood 0))
        )
        |>Map.ofSeq