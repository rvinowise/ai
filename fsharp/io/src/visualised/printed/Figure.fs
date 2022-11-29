module rvinowise.ai.ui.printed.Figure

open rvinowise
open rvinowise.ai.ui

let appearances figure appearances =
    if Seq.isEmpty appearances then
        printfn $"figure %s{figure} doesn't have appearances"
    else
        printfn $"appearances of figure %s{figure}:"
        appearances
        |> Seq.iter printed.figure.Appearance.moments

let overview figure =
    printfn $"id: %s{figure}"
    appearances figure
