namespace rvinowise.ai.built
    


open rvinowise.ai
open rvinowise.extensions


exception BadGraph of string

module Check_figure_correctness =
    
    let is_empty (figure:Figure<_>) =
        figure.targets
        |>Map.isEmpty

    let error_because_of_cycles (figure:Figure<_>) =
        figure
        |>Figure.first_vertices
        |>List.ofSeq
        |>function
        |[]->
            if is_empty figure then
                None
            else
                Some "non-empty figure without first vertices, possibly because of a loop in them"
        |[signal]->None
        |many_vertices->
            many_vertices
            |>Seq.map (Edges.vertex_which_goes_into_cycle figure.edges)
            |>Seq.tryPick id
            |>(fun cycled_vertex ->
                match cycled_vertex with
                |Some vertex ->
                    Some $"figure has a loop with vertex \"{vertex}\""
                |None->
                    None
            )

    
    let error_in_correspondence_between_subfigures_and_edges (figure:IFigure)=
        let subfigures_in_edges = 
            figure.edges
            |>Seq.collect (fun edge->
                [edge.head;edge.tail]
            )|>Set.ofSeq
        let subfigures = 
            figure.targets
            |>Map.keys
            |>Set.ofSeq
        let difference =
            subfigures_in_edges
            |>Set.difference subfigures
        if difference.IsEmpty then
            None
        else
            Some $"superfluous subfigures: {difference}"

    

    let check_correctness figure =
        [
            error_because_of_cycles;
            error_in_correspondence_between_subfigures_and_edges;
        ]|>List.map(fun check ->
            check figure
        )|>List.choose id
        |>function
        |[]->figure
        |errors->
            errors
            |>String.concat "\n"
            |>BadGraph
            |>raise