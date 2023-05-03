namespace rvinowise.ai

open System
open rvinowise.ai

open FsUnit
open Xunit

module Finding_repetitions =
    //let repeated_pair = ``Finding_repetitions(simple)``.repeated_pair
    //let repeated_pair = ``Finding_repetitions(gpu)``.repeated_pair
    let repeated_pair a b = Finding_sequences.repeated_pair (a,b)


    let repeated_pair_with_histories
        (repeating_sequence: Figure_id array)
        (a_appearances: Interval array,
        b_appearances:  Interval array)
        =
        {
            Sequence_appearances.sequence = repeating_sequence
            appearances=
                (repeated_pair 
                    a_appearances
                    b_appearances
                ).ToArray()
        }

    [<Fact>]//(Timeout=1000)
    let ``finding repetitions, when the last A-figure is taken, but there's still B-figures left ``()=
        async { 
            let signal1 = built.Figure_id_appearances.from_moments "signal1" [0;5]
            let signal2 = built.Figure_id_appearances.from_moments "signal2" [1;6;7]
            repeated_pair
                (signal1.appearances|>Array.ofSeq)
                (signal2.appearances|>Array.ofSeq)
            |>should equal (
                [
                    0,1; 5,6
                ]
                |>Seq.map Interval.ofPair
            )
        }

    [<Fact>]
    let ``try repeated_pair_with_histories``()=
        let a_history =
            {
                Sequence_appearances.sequence= [|Figure_id "a"|]
                appearances=[0;5]|>Seq.map Interval.moment|>Array.ofSeq
            }
        let b_history =
            {
                Sequence_appearances.sequence= [|Figure_id "b"|]
                appearances=[1;7]|>Seq.map Interval.moment|>Array.ofSeq
            }
                            
        repeated_pair_with_histories
            (Array.append a_history.sequence b_history.sequence)
            (a_history.appearances, b_history.appearances)
        |>should equal
            {
                Sequence_appearances.sequence=[|"a";"b"|]|>Array.map Figure_id
                appearances=
                    [
                        0,1; 5,7
                    ]
                    |>Seq.map Interval.ofPair
                    |>Array.ofSeq
            }

