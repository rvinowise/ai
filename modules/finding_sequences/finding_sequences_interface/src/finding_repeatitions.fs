namespace rvinowise.ai


open FsUnit
open Xunit

module Finding_repetitions =
    let repeated_pair = 
        ``Finding_repetitions(fsharp_simple)``.repeated_pair
    let repeated_pair_with_histories = 
        ``Finding_repetitions(fsharp_simple)``.repeated_pair_with_histories
    //let repeated_pair a b = Finding_sequences_csharp_gpu.repeated_pair (a,b)
    //let repeated_pair_with_histories a b = Finding_sequences_csharp_gpu.repeated_pair (a,b)


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
