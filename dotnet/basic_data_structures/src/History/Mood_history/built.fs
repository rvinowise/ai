module rvinowise.ai.built.Mood_history
    open Xunit
    open FsUnit
    open rvinowise.ai
    open rvinowise


    type Detailed_history={
        initial_mood:Mood
        mood_at_moments:Mood seq
    }
    let complete_from_tuples
        tuples
        =
        let mood_changes = 
            tuples
            |>Map.ofSeq
        
        {
            initial_mood = Mood 0
            mood_at_moments=
                mood_changes
                |>Seq.pairwise
                |>Seq.fold (
                    fun 
                        history
                        (
                            start,
                            finish
                        ) ->
                        let mood_change = start.Value
                        let amount_of_moments = finish.Key-start.Key
                        let new_mood =history.initial_mood + mood_change
                        {
                            initial_mood=new_mood
                            mood_at_moments=
                                Seq.append 
                                    history.mood_at_moments
                                    (List.init amount_of_moments (fun _->new_mood))
                        }
                    
                    )
                    {
                        Detailed_history.initial_mood=Mood 0
                        mood_at_moments=[]
                    }
                |>fun detailed_history->detailed_history.mood_at_moments
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

        }
    
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

