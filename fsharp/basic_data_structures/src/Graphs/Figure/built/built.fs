module rvinowise.ai.built.Figure
    
    open Xunit
    open FsUnit
    
    open System.Diagnostics.Contracts
    
    open rvinowise.ai
    open rvinowise.ai.figure_parts
    open rvinowise.extensions
    open rvinowise

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
                |>Map.ofSeq 
        }
    
    let sequence_from_text (text:string) =
        let _, (subfigures_sequence: (Vertex_id*Figure_id) list) = 
            text
            |>Seq.fold (
                fun 
                    (referenced_figures_to_last_number,subfigures_sequence)
                    symbol
                    ->
                let referenced_figure = string symbol
                let last_number = 
                    referenced_figures_to_last_number
                    |>Map.tryFind referenced_figure
                    |>function
                    |None -> 0
                    |Some number -> number
                let this_number = last_number+1

                (
                    referenced_figures_to_last_number
                    |>Map.add referenced_figure this_number
                    ,
                    subfigures_sequence @
                    [
                        referenced_figure+string this_number ,
                        referenced_figure
                    ]
                    
                )
            )
                (Map.empty,[])
        {
            edges=
                subfigures_sequence
                |>Seq.map fst
                |>Seq.pairwise
                |>Seq.map Edge.ofPair
                |>Set.ofSeq
            subfigures=
                subfigures_sequence
                |>Map.ofSeq
        }

    [<Fact>]
    let ``try sequence_from_text``()=
        "abba"
        |>sequence_from_text
        |>should equal
            {
                edges=
                    ["a1","b1";"b1","b2";"b2","a2"]
                    |>Seq.map Edge.ofPair
                    |>Set.ofSeq
                subfigures=
                    ["a1","a";"a2","a";"b1","b";"b2","b"]
                    |>Map.ofSeq
            }

    let signal (id:Figure_id) =
        {
            edges=[]
            subfigures=[id,id]|>Map.ofSeq
        }

    let vertex_data_from_edges_of_figure (vertex_data:Vertex_data) (edges:Edge seq) =
        edges
        |>Edges.all_vertices
        |>Seq.map (fun vertex->
            let vertex_data = vertex_data.TryFind(vertex)
            Contract.Assume(
                (vertex_data <> None), 
                "the taken edges of the provided figure must not have verticex, which are not in that figure"
            )
            match vertex_data with
            |Some referenced_figure -> (vertex,referenced_figure)
            |None->
                invalidArg 
                    (nameof edges + " or " + nameof vertex_data)
                    "the taken edges of the provided figure must not have verticex, which are not in that figure"
        )
        |>Map.ofSeq
        
    [<Fact>]
    let ``contract violation accessing verticex of a figure``()=
        Assert.Throws<System.ArgumentException>(fun()->
            vertex_data_from_edges_of_figure
                (Map.ofSeq ["a0","a";"b1","b"])
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
        |>Map.ofSeq 

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

    
    
        


    let subgraph_with_vertices 
        original_figure 
        (vertices:Set<Vertex_id>)
        =
        vertices
        |>Edges.edges_between_vertices original_figure.edges
        |>stencil_output original_figure