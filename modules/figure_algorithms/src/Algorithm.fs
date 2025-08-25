namespace rvinowise.ai


module Algorithm =
    
    let apply_consecutive_functions
        (actions: (Figure<_> -> Figure<_> seq) seq )
        target
        =
        actions
        |>Seq.fold( fun targets action  ->
            targets
            |>Seq.collect action
        )
            [target]  
        
    
    let apply_parallel_functions
        (actions: (Figure<_> -> Figure<_> seq) seq )
        target
        =
        actions
        |>Seq.map( fun action  ->
            action target
        )
        |>Seq.collect id
