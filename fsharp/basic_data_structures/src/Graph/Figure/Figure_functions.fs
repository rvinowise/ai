namespace rvinowise.ai

    open Xunit
    open FsUnit

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure=
        open rvinowise.ai.figure
        open rvinowise
        open rvinowise.extensions
        
        [<Fact>]
        let ``equality comparison``()=
            let f1 = built.from_tuples "F" [
                "a0","a","b0","b"
            ]
            let f2 = built.from_tuples "F" [
                "a0","a","b0","b"
            ]
            f1 |>should equal f2


        [<Fact>]
        let ``vertices reacheble from others``()=
            Graph.vertices_reacheble_from_other_vertices
                Graph.need_every_vertex
                figure.Example.a_high_level_relatively_simple_figure.graph
                ["b0";"b2"]
            |> should equal ["f1"]

        [<Fact>]
        let ``vertices reaching others``()=
            Graph.vertices_reaching_other_vertices
                Graph.need_every_vertex
                figure.Example.a_high_level_relatively_simple_figure.graph
                ["b1";"f1"]
            |> should equal ["b0"]

        
        let need_vertex_referencing_figure 
            (owner_figure:Figure)
            (referenced_figure:Figure_id)
            (checked_vertex) =
            let (exist,reference) = owner_figure.subfigures.TryGetValue(checked_vertex)
            exist && reference=referenced_figure

        let reference_of_vertex (figure:Figure) vertex =
            Dictionary.some_value vertex figure.subfigures

        let vertices_referencing_lower_figure (owner_figure:Figure) lower_figure = 
            lower_figure 
            |> Dictionary.keys_with_value owner_figure.subfigures  

        let referenced_figures owner_figure (subfigures:Vertex_id seq)=
            subfigures
            |>Seq.choose (Dictionary.some_value owner_figure.subfigures)

        let subgraph_with_vertices 
            original_figure 
            (vertices:Set<Vertex_id>)
            =
            vertices
            |>Edges.edges_between_vertices original_figure.graph.edges
            |>built.stencil_output original_figure

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
            Graph.vertices_reacheble_from_other_vertices
                (
                    is_vertex_referencing_figure 
                        owner_figure 
                        figure_referenced_by_needed_subfigures
                    )
                owner_figure.graph
                subfigures_before_goals

        let has_edges (figure:Figure) =
            figure.graph.edges
            |>Seq.isEmpty|>not