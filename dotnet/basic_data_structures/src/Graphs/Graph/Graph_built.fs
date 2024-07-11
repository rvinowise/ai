namespace rvinowise.ai.built
open rvinowise.ai

module Graph =

    let simple (raw_edges:seq<string*string>) =
        raw_edges
        |>Seq.map Edge
        |>Set.ofSeq

    let from_tuples 
        (raw_edges:seq<string*string*string*string>)
        =
        raw_edges
        |>Seq.map (fun (tail_id, _, head_id,_)->
            Edge(
                tail_id, head_id
            );
        )|>Set.ofSeq

    let unique_numbers_for_names_in_sequence 
        (names: string seq)
        =
        let separator = "#"
       
        names
        |>Seq.fold (
            fun 
                (referenced_figures_to_last_number,subfigures_sequence)
                name
                ->
            let last_number = 
                referenced_figures_to_last_number
                |>Map.tryFind name
                |>function
                |None -> 0
                |Some number -> number
            let this_number = last_number+1

            (
                referenced_figures_to_last_number
                |>Map.add name this_number
                ,
                subfigures_sequence @
                [
                    (name+separator+string this_number) |> Vertex_id,
                    name
                ]
                
            )
        )
            (Map.empty,[])
        |>snd

    let sequential_edges
        vertices_sequence
        =
        vertices_sequence
        |>Seq.pairwise
        |>Seq.map Edge.ofPair
        |>Set.ofSeq