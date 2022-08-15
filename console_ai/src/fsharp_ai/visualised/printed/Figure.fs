module rvinowise.ai.ui.printed.Figure

open rvinowise
open rvinowise.ai.ui

let appearances (figure: ai.Figure) =
    printfn "appearances:"
    figure.appearances
    |> Seq.iter printed.Figure_appearance.moments

let overview (figure: ai.Figure) =
    printfn $"id: %s{figure.id}"
    appearances figure
