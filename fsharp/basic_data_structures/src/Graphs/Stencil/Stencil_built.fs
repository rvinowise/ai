module rvinowise.ai.built.Stencil

    open rvinowise.ai
    open rvinowise.extensions

    let node_reference_from_string name=
        match name with
        | "out" -> Stencil_output
        | subfigure -> 
            subfigure
            |>Figure_id
            |>Stencil_node.Lower_figure 

    let vertex_data_from_tuples 
        (edges: seq<string*string*string*string>) =
        edges
        |>Seq.map (fun(tail_id,tail,head_id,head)->
            [
                (Vertex_id tail_id, node_reference_from_string tail);
                (Vertex_id head_id, node_reference_from_string head)
            ]
        )
        |>Seq.concat
        |>dict 

    

    let simple 
        (turn_vertex_id_into_figure_id: string->string)
        (edges:seq<string*string>) 
        =
        {
            edges=built.Graph.simple edges
            nodes=edges
                |>Seq.map (fun(tail_id,head_id)->
                    [
                        (
                            tail_id|>Vertex_id, 
                            tail_id
                            |>turn_vertex_id_into_figure_id
                            |>node_reference_from_string
                        );
                        (
                            head_id|>Vertex_id,
                            head_id
                            |>turn_vertex_id_into_figure_id
                            |>node_reference_from_string
                        );
                    ]
                )
                |>Seq.concat
                |>dict 
        }

    let simple_without_separator (edges:seq<string*string>) =
         simple String.remove_number edges

    let simple_with_separator (edges:seq<string*string>) =
        simple String.remove_number_with_hash edges

    let from_tuples
        (edges:seq<string*string*string*string>) =
        {
            edges=built.Graph.from_tuples edges
            nodes=vertex_data_from_tuples edges
        }