module rvinowise.ai.printed.Interval

open rvinowise
open rvinowise.ai
open System

let to_string (interval: Interval) =
    if interval.start = interval.finish then
        $"{interval.start}"
    else
        $"{interval.start}-{interval.finish}"


let sequence_to_string (intervals: Interval seq) =
    if Seq.isEmpty intervals then
        sprintf $"[]"
    else
        String.Join (" ", 
            intervals
            |>Seq.map to_string 
        )



    




