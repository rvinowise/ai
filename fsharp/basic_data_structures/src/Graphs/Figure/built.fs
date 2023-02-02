module rvinowise.ai.built.Figure
    
    open Xunit
    open FsUnit
    open System.Diagnostics.Contracts
    
    open rvinowise.ai
    open rvinowise.ai.figure_parts
    open rvinowise.extensions

    let simple (edges:seq<Vertex_id*Vertex_id>) =
        {
            edges=built.Graph.simple edges
            subfigures=
                edges
                |>Seq.map (fun(tail_id,head_id)->
                    [
                        (tail_id, String.remove_number tail_id);
                        (head_id, String.remove_number head_id)
                    ]
                )
                |>Seq.concat
                |>dict 
        }
    
    let vertex_data_from_edges_of_figure (vertex_data:Vertex_data) (edges:Edge seq) =
        edges
        |>Edges.all_vertices
        |>Seq.map (fun vertex->
            let (data_exists, src_vertex_data) = vertex_data.TryGetValue(vertex)
            Contract.Assume(
                (data_exists = true), 
                "the taken edges of the provided figure must not have verticex, which are not in that figure"
            )
            if (not data_exists) then
                invalidArg 
                    (nameof edges + " or " + nameof vertex_data)
                    "the taken edges of the provided figure must not have verticex, which are not in that figure"
            else
                (vertex,src_vertex_data)
        )
        |>dict
        
    [<Fact>]
    let ``contract violation accessing verticex of a figure``()=
        Assert.Throws<System.ArgumentException>(fun()->
            vertex_data_from_edges_of_figure
                (dict ["a0","a";"b1","b"])
                [
                    Edge("a0","b1");
                    Edge("b1","a1")
                ]
            |>ignore
        )


    let vertex_data_from_tuples edges=
        edges
        |>Seq.map (fun(tail_id,tail_fig,head_id,head_fig)->
            [
                (tail_id, tail_fig);
                (head_id, head_fig)
            ]
        )
        |>Seq.concat
        |>dict 

    let from_edges_of_figure
        (figure:Figure)
        (edges:Edge seq) =
        {
            edges=edges
            subfigures=vertex_data_from_edges_of_figure figure.subfigures edges

        }

    let from_tuples 
        (edges:seq<Vertex_id*Figure_id*Vertex_id*Figure_id>) =
        {
            edges=built.Graph.from_tuples edges
            subfigures=vertex_data_from_tuples edges
        }

    let stencil_output figure (edges:Edge seq)=
        from_edges_of_figure figure edges

    let empty = from_tuples []

    let sequential_pair 
        (a_figure: Figure)
        (b_figure: Figure)
        =
        ()