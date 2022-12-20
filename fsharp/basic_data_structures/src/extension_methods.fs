namespace rvinowise.extensions

[<AutoOpen>]
module Extensions=
    open System.Text

    let (++) (left : System.Text.StringBuilder) (right : 't) : System.Text.StringBuilder =
        left.Append right

    let (+=) (left : System.Text.StringBuilder) (right : 't) : unit =
        left ++ right |> ignore