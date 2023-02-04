module rvinowise.ai.built.Figure_id_appearances
    open Xunit
    open FsUnit
    open rvinowise.ai

    let from_intervals 
        figure
        intervals
        =
        {
            Figure_id_appearances.figure=figure
            appearances=intervals
        }
    
    let from_tuples 
        figure
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
        figure
        moments
        =
        let intervals = 
            moments
            |>Seq.map Interval.moment
            |>Array.ofSeq
        {
            figure=figure
            appearances=intervals
        }
    
    

