namespace rvinowise.ai.sequence

module Found =

    open rvinowise.ai
    open rvinowise.ai.figure

    let repeated_pair_in_sequences a b =
        []

    let repeated_pair_in_interval a b (interval: Interval) =
        loaded.figure.Appearances.appearances_in_interval a interval
        |> ignore//Seq.iter()

    let in_interval (interval: Interval) =
        loaded.Figure.all
        |> Seq.iter(fun head ->
            loaded.Figure.all
            |> Seq.iter(fun tail ->
                if head <> tail then
                    repeated_pair_in_interval head tail interval
                )
        )
        
        
    
