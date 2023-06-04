module rvinowise.ai.built.Mood_history
    open Xunit
    open FsUnit
    open rvinowise.ai
    open rvinowise


    
    let complete_from_tuples
        tuples
        =
        let mood_changes = 
            tuples
            |>Map.ofSeq
        
        mood_changes
        |>Seq.pairwise
        |>Seq.fold (
            fun 
                (initial_mood, mood_at_moments)
                (
                    start,
                    finish
                ) ->
                let mood_change = start.Value
                let amount_of_moments = finish.Key-start.Key
                let new_mood = initial_mood + mood_change
                
                new_mood
                ,
                Seq.append 
                    mood_at_moments
                    (List.init amount_of_moments (fun _->new_mood))
            
            )
            (Mood 0,[])
        |>snd
        |>fun mood_at_moments ->
            Seq.append
                mood_at_moments
                [
                    mood_changes
                    |>Seq.last
                    |>extensions.KeyValuePair.value
                    |>fun last_mood_change->
                        mood_at_moments
                        |>Seq.last
                        |>(+) last_mood_change
                ]

    
    // [<Fact>]
    // let ``mood history build complete_from_tuples``()=
    //     [
    //         10,1; 15,1; 16,-2; 20,3
    //     ]
    //     |>complete_from_tuples
    //     |>should equal {
    //         interval=Interval.ofPair (10,20)
    //         mood_at_moments=[
    //             1;1;1;1;1;
    //             2;
    //             0;0;0;0;
    //             3
    //         ]
    //     }

