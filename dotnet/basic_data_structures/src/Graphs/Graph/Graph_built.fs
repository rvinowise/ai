module rvinowise.ai.built.Graph
    open rvinowise.ai

    let simple (raw_edges:seq<string*string>) =
        raw_edges
        |>Seq.map (fun (tail_id, head_id)->
            Edge(
                tail_id, head_id
            );
        )|>Set.ofSeq

    let from_tuples 
        (raw_edges:seq<string*string*string*string>)
        =
        raw_edges
        |>Seq.map (fun (tail_id, _, head_id,_)->
            Edge(
                tail_id, head_id
            );
        )|>Set.ofSeq