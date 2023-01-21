module rvinowise.ai.built.Graph
    open rvinowise.ai

    let simple (id:Figure_id) (edges:seq<Vertex_id*Vertex_id>) =
        {
            id=id;
            edges=
                edges
                |>Seq.map (fun (tail_id, head_id)->
                    Edge(
                        tail_id, head_id
                    );
                )
        }

    let from_tuples id edges =
        {
            id=id;
            edges=
                edges
                |>Seq.map (fun (tail_id, _, head_id,_)->
                    Edge(
                        tail_id, head_id
                    );
                )
        }