module rvinowise.ai.built.Appearances
    open Xunit
    open FsUnit
    open rvinowise.ai


    
    let from_tuples 
        tuples
        =
        tuples
        |>Seq.map Interval.ofPair
        |>Array.ofSeq
        
    let from_moments 
        moments
        =
        moments
        |>Seq.map Interval.moment
        |>Array.ofSeq
    
    

