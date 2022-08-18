module rvinowise.ai.ui.printed.Figure

open rvinowise
open rvinowise.ai.ui

let appearances (figure: ai.loaded.figure.Figure) =
    let appearances = figure.appearances
    if Seq.isEmpty appearances then
        printfn $"figure %s{figure.id} doesn't have appearances"
    else
        printfn $"appearances of figure %s{figure.id}:"
        appearances
        |> Seq.iter printed.figure.Appearance.moments

let overview (figure: ai.loaded.figure.Figure) =
    printfn $"id: %s{figure.id}"
    appearances figure
