namespace rvinowise.ai

    open Xunit
    open FsUnit

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure=
        open rvinowise.ai
        open rvinowise
        open rvinowise.extensions
        
        [<Fact>]
        let ``equality comparison``()=
            let f1 = ai.built.Figure.from_tuples [
                "a0","a","b0","b"
            ]
            let f2 = ai.built.Figure.from_tuples [
                "a0","a","b0","b"
            ]
            f1 |>should equal f2


        [<Fact>]
        let ``vertices reacheble from others``()=
            Edges.vertices_reacheble_from_other_vertices
                (fun _->true)
                example.Figure.a_high_level_relatively_simple_figure.edges
                ["b0";"b2"]
            |> should equal ["f1"]

        [<Fact>]
        let ``vertices reaching others``()=
            Edges.vertices_reaching_other_vertices
                (fun _->true)
                example.Figure.a_high_level_relatively_simple_figure.edges
                ["b1";"f1"]
            |> should equal ["b0"]

        
        let need_vertex_referencing_figure 
            (owner_figure:Figure)
            (referenced_figure:Figure_id)
            (checked_vertex) =
            let (exist,reference) = owner_figure.subfigures.TryGetValue(checked_vertex)
            exist && reference=referenced_figure

        let nonexistent_vertex = Figure_id "0" 

        let reference_of_vertex 
            owner_figure 
            vertex
            =
            match
                Dictionary.some_value owner_figure.subfigures vertex 
            with
            | Some referenced_figure -> referenced_figure
            | None -> nonexistent_vertex

    
        let vertices_referencing_lower_figure (owner_figure:Figure) lower_figure = 
            lower_figure 
            |> Dictionary.keys_with_value owner_figure.subfigures  

        let referenced_figures 
            owner_figure
            (subfigures:Vertex_id seq)
            =
            subfigures
            |>Seq.choose (Dictionary.some_value owner_figure.subfigures)

        let subgraph_with_vertices 
            original_figure 
            (vertices:Set<Vertex_id>)
            =
            vertices
            |>Edges.edges_between_vertices original_figure.edges
            |>built.Figure.stencil_output original_figure

        let is_vertex_referencing_figure 
            owner_figure
            referenced_figure
            checked_vertex
            =
            checked_vertex
            |>Dictionary.some_value owner_figure.subfigures
                = Some(referenced_figure)

        let subfigures_after_other_subfigures
            owner_figure
            figure_referenced_by_needed_subfigures
            subfigures_before_goals
            =
            Edges.vertices_reacheble_from_other_vertices
                (
                    is_vertex_referencing_figure 
                        owner_figure 
                        figure_referenced_by_needed_subfigures
                    )
                owner_figure.edges
                subfigures_before_goals

        let has_edges (figure:Figure) =
            figure.edges
            |>Seq.isEmpty|>not

        let private id_from_a_sequence_of_edges (edges: Edge seq) =
            let first_vertex =
                edges
                |>Edges.first_vertices
                |>Seq.head
            
            let rec build_id 
                (edges : Edge seq)
                id
                (vertex:Figure_id)
                =
                let updated_id = id+String.remove_number vertex
                vertex
                |>Edges.next_vertices edges
                |>Seq.tryHead
                |>function
                |None->updated_id
                |Some next_vertex ->
                    build_id
                        edges
                        updated_id
                        next_vertex
            build_id edges "" first_vertex

        let id_from_sequence (figure:Figure) =
            if Seq.isEmpty figure.edges then 
                figure.subfigures
                |>Seq.head
                |>extensions.KeyValuePair.key
            else
                id_from_a_sequence_of_edges figure.edges

        [<Fact>]
        let ``try id_from_sequence``()=
            ["a1","b";"b","a2";"a2","c"]
            |>built.Figure.simple
            |>id_from_sequence
            |>should equal "abac"
        
        
        [<Fact>]
        let ``try id_from_sequence for a signal``()=
            "a"
            |>built.Figure.signal
            |>id_from_sequence
            |>should equal "a"