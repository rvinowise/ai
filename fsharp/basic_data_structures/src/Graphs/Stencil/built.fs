module rvinowise.ai.built.Stencil

    open rvinowise.ai
    open rvinowise.extensions

    let node_reference_from_string name=
        match name with
        | "out" -> Node_reference.Stencil_output
        | subfigure -> Node_reference.Lower_figure subfigure

    let vertex_data_from_tuples edges =
        edges
        |>Seq.map (fun(tail_id,tail,head_id,head)->
            [
                (tail_id, node_reference_from_string tail);
                (head_id, node_reference_from_string head)
            ]
        )
        |>Seq.concat
        |>dict 

    let simple id (edges:seq<string*string>) =
        {
            edges=built.Graph.simple edges
            nodes=edges
                |>Seq.map (fun(tail_id,head_id)->
                    [
                        (
                            tail_id, 
                            tail_id
                            |>String.remove_number 
                            |>node_reference_from_string
                        );
                        (
                            head_id,
                            head_id
                            |>String.remove_number 
                            |>node_reference_from_string
                        );
                    ]
                )
                |>Seq.concat
                |>dict 
        }

    let from_tuples
        (edges:seq<Vertex_id*string*Vertex_id*string>) =
        {
            edges=built.Graph.from_tuples edges
            nodes=vertex_data_from_tuples edges
        }
