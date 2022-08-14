namespace rvinowise.ai


type Figure = {
    id: string
    appearances: Figure_appearance list
    edges: Edge list
} and Edge = {
    start: Figure
    ending: Figure
}

module Figure =

    let print_appearances_of (figure: Figure) =
        printfn "appearances:"
        figure.appearances
        |> Seq.iter Figure_appearance.print

    let print (figure: Figure) =
        printfn $"id: %s{figure.id}"
        print_appearances_of figure