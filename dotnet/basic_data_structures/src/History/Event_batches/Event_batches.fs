module rvinowise.ai.Event_batches
    open System
    open FsUnit
    open Xunit
    open System.Collections.Generic
    //open FSharpPlus

    open rvinowise.ai
    open rvinowise 


    let only_signals 
        (event_batches: (Appearance_event list*Mood) seq) 
        =
        event_batches
        |>Seq.map fst

    let only_mood_changes
        (event_batches: (Appearance_event list*Mood) seq) 
        =
        event_batches
        |>Seq.map snd
        |>Seq.indexed
        |>Seq.filter (fun (_,mood)->
            mood <> Mood 0
        )

    let event_history_from_lists =
        List.map (List.map (Figure_id>>Appearance_event.Signal))
        >>Array.ofList
    
    let add_signal_to_history 
        figure
        moment
        (batches: Appearance_event list array)
        =
        batches[moment] <- (
            (Signal figure)::batches[moment]
        )
        batches

    let add_start_and_finish_to_history
        figure
        (interval:Interval)
        (batches: Appearance_event list array)
        =
        batches[interval.start] <- (
            (Start figure)::batches[interval.start]
        )
        batches[interval.finish] <- (
            (Finish (figure,interval.start))::batches[interval.finish]
        )
        batches

    let add_events_to_combined_history 
        figure
        (appearances : Interval array)
        (event_batches: Appearance_event list array)
        = 
        appearances
        |>Seq.fold (fun batches interval->
            if interval.start = interval.finish then
                add_signal_to_history
                    figure
                    interval.start
                    batches
            else
                add_start_and_finish_to_history
                    figure
                    interval
                    batches
            
        ) event_batches

    let from_appearances 
        (figure_histories : (Figure_id * Interval array) seq)
        =
        let history = 
            if Seq.isEmpty figure_histories then
                [||]
            else 
                figure_histories
                |>Seq.map (fun history->
                    history
                    |>snd
                    |>Seq.maxBy (fun interval -> interval.finish)
                    |>Interval.finish
                )
                |>Seq.max
                |>fun history_length -> (history_length+1),[]
                ||>Array.create
        figure_histories
        |>Seq.fold (
            fun event_batches 
                (figure, appearances) ->
            add_events_to_combined_history figure appearances event_batches
        ) history
            


    [<Fact>]
    let ``combine figure histories``()=
        let history_of_a = built.Appearances.from_tuples [
            0,1; 2,4
        ]
        let history_of_b = built.Appearances.from_tuples [
            0,2; 4,4
        ]
        [
            (Figure_id "a", history_of_a); 
            (Figure_id "b", history_of_b)
        ]
        |>from_appearances 
        |>Seq.mapi (fun moment batch ->
            moment, (batch|>Seq.sort)
        )
        |>should equal [
            0,[
                "a"|>Figure_id|>Start;
                "b"|>Figure_id|>Start
            ];
            1,[
                Finish ("a"|>Figure_id,0)
            ];
            2,[
                "a"|>Figure_id|>Start;
                Finish ("b"|>Figure_id,0)
            ];
            4,[
                Finish ("a"|>Figure_id,2);
                "b"|>Figure_id|>Signal
            ]
        ]


 
    let event_batches_to_figure_appearances
        starting_moment
        (event_batches: Appearance_event list seq)
        =
        event_batches
        |>Seq.mapi (fun index batch ->
            (index+starting_moment, batch)
        )
        |>Seq.fold (
            fun 
                (figure_appearences: Map<Figure_id, Interval list>)
                (moment, events)
                ->
            events
            |>Seq.fold (fun figure_appearances event->
                match event with
                |Finish (figure, start_moment)->
                    figure_appearances
                    |>Map.add figure (
                        (Interval.from_int start_moment moment)
                        :: (
                            figure_appearences
                            |>Map.tryFind figure
                            |>Option.defaultValue []
                        )
                    )
                |Signal (figure)->
                    figure_appearances
                    |>Map.add figure (
                        (Interval.moment moment)
                        :: (
                            figure_appearences
                            |>Map.tryFind figure
                            |>Option.defaultValue []
                        )
                    )
                |_->figure_appearances
            ) figure_appearences
        ) Map.empty
        |> Map.map (fun figure appearances->
            appearances|>List.rev|>Array.ofList
        )


    // let to_separate_histories
    //     (event_batches: (Appearance_event list * Mood) seq )
    //     =
    //     let figure_appearances = 
    //         Dictionary<Figure_id, ResizeArray<Interval>>()
    //     let mutable mood_changes: Mood_changes_history = Map.empty
        
    //     event_batches
    //     |>Seq.iteri (fun moment (events,mood) ->
    //         events
    //         |>Seq.iter (function
    //             |Finish (figure, start_moment) ->
    //                 let old_appearances =
    //                     figure_appearances
    //                     |>extensions.Dictionary.getOrDefault 
    //                         figure (ResizeArray())
    //                 old_appearances.Add(Interval.regular start_moment moment)
    //                 figure_appearances[figure]<- old_appearances
    //             |Signal figure ->
    //                 let old_appearances =
    //                     figure_appearances
    //                     |>extensions.Dictionary.getOrDefault 
    //                         figure (ResizeArray())
    //                 old_appearances.Add(Interval.moment moment)
    //                 figure_appearances[figure]<- old_appearances
    //             |_ ->()
    //         )
    //         if mood <> Mood 0 then 
    //             mood_changes <- mood_changes.Add(moment, mood)
    //         else
    //             ()
    //     )
    //     {
    //         Separate_histories.figure_apperances=
    //             figure_appearances
    //             |>Seq.map (fun pair ->
    //                 built.Figure_id_appearances.from_intervals 
    //                     (pair.Key |>Figure_id.value)
    //                     (pair.Value.ToArray())
    //             )
    //         mood_change_history=mood_changes
    //     }


    // [<Fact>]
    // let ``to mood changes history``()=
    //     "1ok;23ok;45no;no;67"
    //    //01  234  567  8  9¹ <-moments
    //     |>built_from_text.Event_batches.signals_with_mood_from_text
    //     |>event_batches_to_mood_changes
    //     |>should equal (dict [
    //         1,Mood +1;
    //         4,Mood +1;
    //         7,Mood -2;
    //     ])
    // [<Fact>]
    // let ``to mood changes history2``()=
    //     "00 no3; 223 ok2;4 no2;"
    //    //01 2    345 6   7 89 <-moments
    //     |>built_from_text.Event_batches.signals_with_mood_from_text
    //     |>event_batches_to_mood_changes
    //     |>should equal (dict [
    //         2,Mood -3;
    //         6,Mood +2;
    //         8,Mood -2;
    //     ])


    
    let add_appearances
        (appearances: (Figure_id*Interval array) seq )
        (event_batches: Appearance_event list seq)
        =
        event_batches
        |>event_batches_to_figure_appearances 0
        |>FSharpPlus.Map.union (appearances|>Map)
        |>extensions.Map.toPairs
        |>from_appearances

    
    let add_sequence_appearances
        (sequence_appearances: (Sequence*Interval array) seq)
        (event_batches: Appearance_event list array)
        =
        event_batches
        |>add_appearances (
            sequence_appearances
            |>Seq.map (fun (sequence, apperances)->
                (sequence|>Sequence.to_figure_id)
                ,
                apperances
            )
        )

    [<Fact>]
    let ``turn a combined history into separate figure histories``()=
            [
                ["a";"b"]//0
                ["c";"d"]//1
                ["a"]//2
                ["b"]//3
            ]|>List.map (List.map (Figure_id>>Appearance_event.Signal))
            |>event_batches_to_figure_appearances 0
            |>should equal (
                [
                    "a", [0;2]
                    "b", [0;3]
                    "c", [1]
                    "d", [1]
                ]|>List.map(fun (signal, appearances)->
                    signal|>Figure_id
                    , 
                    appearances|>Seq.map Interval.moment
                )
            )


    
    let to_sequence_appearances event_batches =
        event_batches
        |>event_batches_to_figure_appearances 0
        |>Seq.map (fun pair ->
            [|pair.Key|], pair.Value
        )