module rvinowise.ai.ui.Printed_object

open rvinowise


let figure_appearance (appearance: ai.Figure_appearance) =
    printf $"({appearance.head} {appearance.tail}) "

let appearances_of (figure: ai.Figure) =
    printfn "appearances:"
    figure.appearances
    |> Seq.iter figure_appearance

let figure (figure: ai.Figure) =
    printfn $"id: %s{figure.id}"
    appearances_of figure