module rvinowise.ai.sequence.Found

open System

open rvinowise.ai
open rvinowise.ai.figure


let repeated_pair a b (interval: Interval) =
    loaded.figure.Appearances.appearances_in_interval a interval
    |> ignore//Seq.iter()

let in_interval (interval: Interval) =
    loaded.Figure.all
    |> Seq.iter(fun head ->
        loaded.Figure.all
        |> Seq.iter(fun tail ->
            if head <> tail then
                repeated_pair head tail interval
            )
    )
    
    
    
