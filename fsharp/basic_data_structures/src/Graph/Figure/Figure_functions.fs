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
        
        let need_every_vertex _ =
            true

        let need_vertex_referencing_figure 
            (owner_figure:Figure)
            (referenced_figure:Figure_id)
            (checked_vertex) =
            let (exist,reference) = owner_figure.subfigures.TryGetValue(checked_vertex)
            exist && reference=referenced_figure

        let reference_of_vertex (figure:Figure) vertex =
            Dictionary.some_value vertex figure.subfigures

        let vertices_reacheble_from_other_vertices
            (is_needed: Vertex_id->bool)
            (graph_in_which_search: Graph)
            (vertices_before_goals: Vertex_id seq)
            =
            Edges.vertices_reacheble_from_other_vertices
                is_needed
                graph_in_which_search.edges
                vertices_before_goals
    
        let vertices_reaching_other_vertices
            (is_needed: Vertex_id->bool)
            (graph_in_which_search: Graph)
            (vertices_after_goals: Vertex_id seq)
            =
            Edges.vertices_reaching_other_vertices
                is_needed
                graph_in_which_search.edges
                vertices_after_goals

        

        [<Fact>]
        let ``subfigures reacheble from others``()=
            vertices_reacheble_from_other_vertices
                need_every_vertex
                figure.Example.a_high_level_relatively_simple_figure.graph
                ["b0";"b2"]
            |> should equal ["f1"]

        [<Fact>]
        let ``subfigures reaching others``()=
            vertices_reaching_other_vertices
                need_every_vertex
                figure.Example.a_high_level_relatively_simple_figure.graph
                ["b1";"f1"]
            |> should equal ["b0"]
        

        let next_vertices graph vertex=
            Edges.next_vertices graph.edges vertex

        let previous_vertices graph vertex=
            Edges.previous_vertices graph.edges vertex

        let first_vertices (graph:Graph) =
            Edges.first_vertices graph.edges
            

        let vertices_referencing_lower_figure (owner_figure:Figure) lower_figure = 
            lower_figure 
            |> Dictionary.keys_with_value owner_figure.subfigures  

        let referenced_figures owner_figure (subfigures:Vertex_id seq)=
            subfigures
            |>Seq.choose (Dictionary.some_value owner_figure.subfigures)

        let subgraph_with_vertices 
            target 
            (vertices:Set<Vertex_id>)
            =
            vertices
            |>Edges.edges_between_vertices target.graph.edges
            |>built.stencil_output target

