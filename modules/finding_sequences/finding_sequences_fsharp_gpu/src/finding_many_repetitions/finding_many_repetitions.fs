namespace rvinowise.ai



module ``Finding_many_repetitions(fsharp_gpu)`` =
    
    open System
    open Xunit
    open Xunit.Abstractions
    open FsUnit
    open ILGPU
    open ILGPU.Runtime


    type Program =
        static member MyKernel 
            (index: Index1D) 
            (buffer: ArrayView1D<int, Stride1D.Dense>) 
            (constant: int) 
            =
            buffer.[index] <- int(index) + constant

    type Known_sequences = Set<Figure_id array>

    let private is_this_sequence_already_found 
        (known_sequences: Known_sequences)
        ab_sequence
        =
        known_sequences
        |>Set.contains ab_sequence 

    let many_repetitions
        (sequence_appearances: seq<Sequence_appearances>)
        =
        let known_sequences = 
            sequence_appearances
            |>Seq.map (fun history->history.sequence)
            |>Set.ofSeq
        
        (sequence_appearances,sequence_appearances)
        ||>Seq.allPairs
        |>Seq.fold (
            fun 
                (known_sequences, found_pairs)
                (a_history,b_history) 
                ->
            let ab_sequence = 
                Array.append 
                    a_history.sequence
                    b_history.sequence
            if is_this_sequence_already_found
                known_sequences
                ab_sequence
            then
                known_sequences,found_pairs
            else
                let found_pair = 
                    (a_history.appearances, b_history.appearances)
                    |>``Finding_repetitions(fsharp_gpu)``.repeated_pair_with_histories ab_sequence
                if Appearances.has_repetitions found_pair.appearances then
                    (
                        known_sequences
                        |>Set.add found_pair.sequence
                    ),
                    found_pair::found_pairs
                else
                    known_sequences,found_pairs
            )
            (known_sequences,[])
        |>snd
    

    let all_repetitions 
        (sequence_appearances: seq<Sequence_appearances>)
        =
        let rec steps_of_finding_repetitions
            (all_sequences: seq<Sequence_appearances>)
            (sequences_of_previous_step: seq<Sequence_appearances>)
            =
            if Seq.isEmpty sequences_of_previous_step then
                all_sequences
            else
                let all_sequences =
                    all_sequences
                    |>Seq.append sequences_of_previous_step
                all_sequences
                |>many_repetitions
                |>steps_of_finding_repetitions all_sequences
                
        steps_of_finding_repetitions
            []
            sequence_appearances

    [<Fact>]
    let ``try all_repetitions``()=
        "a1b2c3a45bc"
//       a b c
//             a  bc
//mom:   0123456789¹1
        |>built.Event_batches.from_text
        |>built.Event_batches.to_sequence_appearances
        |>all_repetitions
        |>Set.ofSeq