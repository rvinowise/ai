module rvinowise.ai.built.Figure_id_appearances
    open Xunit
    open FsUnit
    open rvinowise.ai

    let from_intervals 
        (figure:string)
        intervals
        =
        {
            Figure_id_appearances.figure=figure|>Figure_id
            appearances=intervals
        }
    
    let from_tuples 
        (figure:string)
        tuples
        =
        from_intervals 
            figure
            (
                tuples
                |>Seq.map Interval.ofPair
                |>Array.ofSeq
            )
        
    let from_moments 
        (figure:string)
        moments
        =
        let intervals = 
            moments
            |>Seq.map Interval.moment
            |>Array.ofSeq
        {
            figure=Figure_id figure
            appearances=intervals
        }
    
    

