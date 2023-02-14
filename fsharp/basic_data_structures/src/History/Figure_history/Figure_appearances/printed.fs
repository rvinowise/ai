module rvinowise.ai.printed.Figure_appearances

open rvinowise
open rvinowise.ai
open System

let interval_to_string (interval: Interval) =
    if interval.start = interval.finish then
        $"{interval.start}"
    else
        $"{interval.start}-{interval.finish}"


let all_appearances_to_string (appearances: Interval seq) =
    if Seq.isEmpty appearances then
        sprintf $"[]"
    else
        String.Join (" ", 
            appearances
            |>Seq.map interval_to_string 
        )

let to_string 
    (figure: Figure)
    (appearances: Interval seq) =
    let str_figure = printed.Figure.to_string figure.edges figure.subfigures
    let str_appearances = all_appearances_to_string appearances
    $"""Figure_appearances({str_figure} appearances={str_appearances})"""

    




