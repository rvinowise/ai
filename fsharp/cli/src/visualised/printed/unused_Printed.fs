module rvinowise.ai.ui.Printed_object

open rvinowise


let figure_appearance (appearance: ai.figure.Appearance) =
    printf $"({appearance.head} {appearance.tail}) "

let appearances_of (figure: ai.figure.Figure) =
    printfn "appearances:"
    figure.appearances
    |> Seq.iter figure_appearance

let figure (figure: ai.figure.Figure) =
    printfn $"id: %s{figure.id}"
    appearances_of figure