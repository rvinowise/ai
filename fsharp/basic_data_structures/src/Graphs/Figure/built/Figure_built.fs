module rvinowise.ai.built.Figure
    
    open Xunit
    open FsUnit
    
    open System.Diagnostics.Contracts
    
    open rvinowise.ai
    open rvinowise.extensions
    open rvinowise

    let simple (edges:seq<string*string>) =
        {
            edges=built.Graph.simple edges
            subfigures=
                edges
                |>Seq.map (fun(tail_id,head_id)->
                    [
                        (Vertex_id tail_id,  
                        tail_id
                        |>String.remove_number
                        |>Figure_id
                        );
                        (Vertex_id head_id,
                        head_id 
                        |>String.remove_number
                        |>Figure_id
                        )
                    ]
                )
                |>Seq.concat
                |>Map.ofSeq 
        }
    
    let from_sequence (subfigures: Figure_id seq) =
        let separator = "#"
        let _, (subfigures_sequence: (Vertex_id*Figure_id) list) = 
            subfigures
            |>Seq.fold (
                fun 
                    (referenced_figures_to_last_number,subfigures_sequence)
                    referenced_figure
                    ->
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
                        (Figure_id.value referenced_figure+separator+string this_number) |> Vertex_id,
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

    let sequence_from_text (text:string) =
        text
        |>Seq.map (string>>Figure_id)
        |>Array.ofSeq
        |>from_sequence

    [<Fact>]
    let ``try sequence_from_text``()=
        "abba"
        |>sequence_from_text
        |>should equal
            {
                edges=
                    ["a#1","b#1";"b#1","b#2";"b#2","a#2"]
                    |>Seq.map Edge.ofStringPair
                    |>Set.ofSeq
                subfigures=
                    ["a#1","a";"a#2","a";"b#1","b";"b#2","b"]
                    |>Seq.map (fun (tail,head) -> Vertex_id tail, Figure_id head)
                    |>Map.ofSeq
                    |>Map.map (fun _ value ->value)
            }

    let signal (id:string) =
        {
            edges=Set.empty
            subfigures=[id|>Vertex_id, id|>Figure_id]|>Map.ofSeq
        }

    let vertex_data_from_edges_of_figure (vertex_data: Map<Vertex_id, Figure_id>) (edges:Edge seq) =
        edges
        |>Edges.all_vertices
        |>Seq.map (fun vertex->
            let referenced_element = vertex_data.TryFind(vertex)
            Contract.Assume(
                (referenced_element <> None), 
                "the taken edges of the provided figure must not have verticex, which are not in that figure"
            )
            match referenced_element with
            |Some referenced_figure -> (vertex,referenced_figure)
            |None->
                invalidArg 
                    (nameof edges + " or " + nameof referenced_element)
                    "the taken edges of the provided figure must not have verticex, which are not in that figure"
        )
        |>Map.ofSeq
        


    let vertex_data_from_tuples 
        (edges:seq<string*string*string*string>) 
        =
        edges
        |>Seq.map (fun(tail_id,tail_ref,head_id,head_ref)->
            [
                (Vertex_id tail_id, Figure_id tail_ref);
                (Vertex_id head_id, Figure_id head_ref)
            ]
        )
        |>Seq.concat
        |>Map.ofSeq 

    let from_edges_of_figure
        (figure:Figure)
        (edges:Edge seq) =
        {
            edges=edges|>Set.ofSeq
            subfigures=vertex_data_from_edges_of_figure figure.subfigures edges

        }

    let from_tuples 
        (edges:seq<string*string*string*string>) =
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