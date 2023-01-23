namespace rvinowise.ai
    open FsUnit
    open Xunit
    open rvinowise

    type Mood_change_interval = {
        interval: Interval
        change: Mood
    }

    module Mood_change_interval=
        let from_tuples (tuples: (Moment*Moment*Mood) seq) =
            tuples
            |>Seq.map(fun (start,finish,mood)->
                {
                    interval=Interval.regular start finish
                    change=mood
                }
            )

    module Desiring_future=
        open rvinowise.ai


        let intervals_changing_mood (mood_changes_history:Mood_changes_history)=
            let changes =
                mood_changes_history
                |>extensions.Map.toPairs
                |>Array.ofSeq
            changes
            |>Seq.collecti (fun (previous_moment, mood)->
                let start_moment = previous_moment+1
                changes
                |>Seq.
            )
            

        [<Fact>]
        ``intervals_changing_mood``()=
            ["1×23×45¬¬67"]
            //0123456789 <-moments
            |>built.Event_batches.from_text
            |>built.Event_batches.to_figure_histories
            |>intervals_changing_mood
            |>should equal [
                0,1,+1; 
                0,4,+2;
                0,7,0;
                2,4,+1; 
                2,7,-1; 
                5,7,-2;
            ]

        let desired history=
            ()
