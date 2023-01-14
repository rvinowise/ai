namespace rvinowise.ai.fsharp_impl

open System
open rvinowise.ai

module Finding_many_repetitions =



    let many_repetitions
        (figure_histories: seq<Figure_history>)
        =
        figure_histories
        |>Seq.map