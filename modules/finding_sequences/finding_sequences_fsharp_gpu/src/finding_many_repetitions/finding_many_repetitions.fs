namespace rvinowise.ai



module ``Finding_many_repetitions(fsharp_gpu)`` =
    
    open System
    open Xunit
    open Xunit.Abstractions
    open FsUnit
    open ILGPU
    open ILGPU.Runtime


    type Known_sequences = Set<Figure_id array>

    let private is_this_sequence_already_found 
        (known_sequences: Known_sequences)
        ab_sequence
        =
        known_sequences
        |>Set.contains ab_sequence 

    type Program =
        static member kernel 
            (index: Index1D) 
            (appearances: ArrayView1D<Interval, Stride1D.Dense>) 
            (buffer: ArrayView1D<int, Stride1D.Dense>) 
            =
            buffer.[index] <- int(index) + constant

    let all_repetitions 
        (sequence_appearances: seq<Sequence_appearances>)
        =
        let context = Context.CreateDefault();
        let accelerator = 
            context.GetPreferredDevice(preferCPU: false).CreateAccelerator(context);
        
        use appearances_buffer = 
            accelerator.Allocate1D [0; 1; 2; 3; 4; 5; 6; 7; 8; 9]
        use deviceOutput = accelerator.Allocate1D<int>(10_000);

        let loadedKernel = 
            accelerator.LoadAutoGroupedStreamKernel<_,_,_>(Program.kernel)

        loadedKernel(Index1D(int(deviceOutput.Length)), deviceData.View, deviceOutput.View);

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
                |>repetitions_of_one_stage
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
        |>Event_batches.from_text
        |>Event_batches.to_sequence_appearances
        |>all_repetitions
        |>Set.ofSeq