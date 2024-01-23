namespace rvinowise.ai.built

open rvinowise.ai

module Appearances=
    
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
    
    

