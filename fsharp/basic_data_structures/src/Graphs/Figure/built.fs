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

    
    let private rename_duplicating_vertices
        (a_figure: Figure)
        (b_figure: Figure)
        =
        let renamed_subfigures = Map.empty
        b_figure.subfigures
        |>Seq.choose (fun pair->
            let b_vertex = pair.Key
            if 
                a_figure.subfigures|>Map.containsKey b_vertex
            then
                Some (b_vertex, b_vertex+"'")
            else
                None

        )
        |>Map.ofSeq
    
    let sequential_pair 
        (a_figure: Figure)
        (b_figure: Figure)
        =
        let renamed_b_vertices = 
            rename_duplicating_vertices
                a_figure
                b_figure
        let renamed_b_subfigures = 
            b_figure.subfigures
            |>Seq.map (fun pair->
                let old_name = pair.Key
                let referenced_figure = pair.Value
                let new_name = renamed_b_vertices|>Map.tryFind old_name
                match new_name with
                |Some name -> (name,referenced_figure)
                |None -> (old_name,referenced_figure)
            )
            |>Map.ofSeq

        let b_edges_with_renamed_subfigures =
            b_figure.edges
            |>Seq.map(fun edge->
                let new_head = 
                    renamed_b_vertices
                    |>Map.tryFind edge.head
                    |>function
                    |Some renamed -> renamed
                    |None -> edge.head
                let new_tail = 
                    renamed_b_vertices
                    |>Map.tryFind edge.tail
                    |>function
                    |Some renamed -> renamed
                    |None -> edge.tail
                (new_tail, new_head)
            )
            |>Seq.map Edge.ofPair

        let edges_inbetween =
            let last_vertices =
                a_figure
                |>Figure.last_vertices
            let first_vertices =
                {
                    edges=b_edges_with_renamed_subfigures
                    subfigures=renamed_b_subfigures
                }
                |>Figure.first_vertices
            Seq.allPairs last_vertices first_vertices 
            |>Seq.map Edge.ofPair

        {
            edges=
                a_figure.edges
                |>Seq.append b_edges_with_renamed_subfigures
                |>Seq.append edges_inbetween
            
            subfigures=
                a_figure.subfigures
                |>extensions.Map.add_map renamed_b_subfigures
        }

    [<Fact>]
    let ``try sequential_pair``()=
        let a_figure = simple ["a1","b1";"b1","a2";"b1","c1"]
        let b_figure = simple ["a1","b1";"e1","b1";"b1","a2"]
        let expected_ab_figure = {
            edges=[
                "a1","b1"; "b1","a2"; "b1","c1";
                "a1'","b1'"; "e1","b1'"; "b1'","a2'";
                //edges between glued graphs:
                "a2","a1'";
                "a2","e1";
                "c1","a1'";
                "c1","e1"

            ]
            |>Seq.map Edge.ofPair
            |>Seq.sort
            
            subfigures=[
                    "a1","a";
                    "a1'","a";
                    "a2","a";
                    "a2'","a";
                    "b1","b";
                    "b1'","b";
                    "c1","c";
                    "e1","e";
                ]
                |>Map.ofSeq
        }
        let real_ab_figure = 
            sequential_pair
                a_figure
                b_figure
        real_ab_figure.edges
        |>Seq.sort
        |>should equal expected_ab_figure.edges

        real_ab_figure.subfigures
        |>should equal expected_ab_figure.subfigures
        


    let subgraph_with_vertices 
        original_figure 
        (vertices:Set<Vertex_id>)
        =
        vertices
        |>Edges.edges_between_vertices original_figure.edges
        |>stencil_output original_figure