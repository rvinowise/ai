module rvinowise.ai.built.Event_batches
    open System
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
        |>extensions.String.split_into_same_symbols
        |>Seq.collect (fun group->
            match Seq.head group with
            |'×'-> seq{seq{$"+{Seq.length group}"}}
            |'¬'-> seq{seq{$"-{Seq.length group}"}}
            |signal ->
                group
                |> Seq.map (string>>Seq.singleton)
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
        |>String.Concat
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
        (figure_history : Figure_id_appearances)
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

    let from_figure_id_appearances 
        (figure_histories : Figure_id_appearances seq)
        =
        figure_histories
        |>Seq.fold (fun event_batches figure_history ->
            add_events_to_combined_history figure_history event_batches
        ) 
            Map.empty
    
    let from_figure_appearances 
        (figure_histories : Figure_appearances seq)
        =
        figure_histories
        |>Seq.map built.Figure_appearances.to_figure_id_appearances
        |>from_figure_id_appearances
            
    [<Fact>]
    let ``combine figure histories``()=
        let history_of_a = built.Figure_id_appearances.from_tuples "a" [
            0,1; 2,4
        ]
        let history_of_b = built.Figure_id_appearances.from_tuples "b" [
            0,2; 4,4
        ]
        [history_of_a; history_of_b]
        |>from_figure_id_appearances 
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
        |>from_figure_id_appearances
        |>add_mood_to_combined_history mood_history
    
    let to_separate_histories
        (event_batches: Event_batches)
        =
        let figure_appearances = 
            Dictionary<Figure_id, ResizeArray<Interval>>()
        let mutable mood_changes: Mood_changes_history = Map.empty
        
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
            if batch.mood.change <> Mood 0 then 
                mood_changes <- mood_changes.Add(moment, batch.mood.change)
            else
                ()
        )
        
        {
            Separate_histories.figure_apperances=
                figure_appearances
                |>Seq.map (fun pair ->
                    built.Figure_id_appearances.from_intervals pair.Key (pair.Value.ToArray())
                )
            mood_change_history=mood_changes
        }

    [<Fact>]
    let ``to mood changes history``()=
        "1×23×45¬¬67"
       //01234567 89 <-moments
        |>from_text
        |>to_separate_histories
        |>Separate_histories.mood_change_history
        |>should equal (dict [
            1,Mood +1;
            4,Mood +1;
            7,Mood -2;
        ])
    [<Fact>]
    let ``to mood changes history2``()=
        "00¬¬¬223××4¬¬"
       //012  3456 78 9 <-moments
        |>from_text
        |>to_separate_histories
        |>Separate_histories.mood_change_history
        |>should equal (dict [
            2,Mood -3;
            6,Mood +2;
            8,Mood -2;
        ])


    let from_combined_histories 
        (histories: Event_batches seq)
        =
        histories
        |>Seq.map to_separate_histories
        |>Seq.collect Separate_histories.figure_id_appearances
        |>from_figure_id_appearances
    
    let add_figure_id_appearances
        (figure_id_appearances: Figure_id_appearances seq)
        (event_batches: Event_batches)
        =
        event_batches
        |>to_separate_histories
        |>Separate_histories.figure_id_appearances
        |>Seq.append figure_id_appearances
        |>from_figure_id_appearances

    let add_figure_appearances
        (figure_appearances: Figure_appearances seq)
        (event_batches: Event_batches)
        =
        event_batches
        |>add_figure_id_appearances (
            figure_appearances
            |>Seq.map built.Figure_appearances.to_figure_id_appearances
        )


    [<Fact>]
    let ``turn a combined history into separate figure histories``()=
        from_contingent_signals 0
            [
                ["a";"b"]//0
                ["c";"d"]//1
                ["a"]//2
                ["b"]//3
            ]
            |>to_separate_histories
            |>Separate_histories.figure_id_appearances
            |>should equal [
                built.Figure_id_appearances.from_moments "a" [0;2]
                built.Figure_id_appearances.from_moments "b" [0;3]
                built.Figure_id_appearances.from_moments "c" [1]
                built.Figure_id_appearances.from_moments "d" [1]
            ]


    let add_mood_history 
        (mood_changes_history: Mood_changes_history) 
        (event_batches: Event_batches)
        =
        ()
    
    let remove_batches_without_actions (event_batches:Event_batches)=
        event_batches
        |>extensions.Map.toPairs
        |>Seq.filter (fun (moment, batch)->
            (batch.events|>Seq.isEmpty|>not)
            ||
            (batch.mood.change <> (Mood 0))
        )
        |>Map.ofSeq