module rvinowise.ai.Appearances
    open System.Collections.Generic
    open rvinowise
    open FsUnit
    open Xunit
    open rvinowise.ai
    open FsUnit
    open Xunit


    let sequence_appearances_to_string 
        (sequence_appearances: Sequence*Interval array)
        =
        let str_sequence=
            sequence_appearances|>fst
            |>Seq.map Figure_id.value
            |>String.concat ""
            
        let str_appearances = 
            sequence_appearances|>snd
            |>Interval.intervals_to_string 
        
        $"appearances={str_appearances}"
        |>(+) $"{str_sequence}"

    let sequence_appearances_to_id_appearances 
        (history: (Sequence*Interval array) seq)
        =
        history
        |>Seq.map (fun (figure, appearances)->
            (Sequence.to_figure_id figure)
            ,
            appearances
        )

    let has_repetitions appearances =
        Seq.length appearances > 1

    let start appearances =
        appearances
        |>Array.head 
        |>Interval.start

    let finish appearances =
        appearances
        |>Array.last 
        |>Interval.finish


        
      



     
  


