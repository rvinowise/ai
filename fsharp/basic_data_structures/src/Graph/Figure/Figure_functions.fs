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

        let subfigures_reacheble_from_other_subfigures
            (is_needed: Vertex_id->bool)
            (figure_in_which_search: Figure)
            (subfigures_before_goals: Vertex_id seq)
            =
            Edges.vertices_reacheble_from_other_vertices
                is_needed
                figure_in_which_search.edges
                subfigures_before_goals
    
        let subfigures_reaching_other_subfigures
            (is_needed: Vertex_id->bool)
            (figure_in_which_search: Figure)
            (subfigures_after_goals: Vertex_id seq)
            =
            Edges.vertices_reaching_other_vertices
                is_needed
                figure_in_which_search.edges
                subfigures_after_goals

        

        [<Fact>]
        let ``subfigures reacheble from others``()=
            subfigures_reacheble_from_other_subfigures
                need_every_vertex
                figure.Example.a_high_level_relatively_simple_figure
                ["b0";"b2"]
            |> should equal ["f1"]

        [<Fact>]
        let ``subfigures reaching others``()=
            subfigures_reaching_other_subfigures
                need_every_vertex
                figure.Example.a_high_level_relatively_simple_figure
                ["b1";"f1"]
            |> should equal ["b0"]
        

        let next_vertices figure subfigure=
            Edges.next_vertices figure.edges subfigure

        let previous_vertices figure subfigure=
            Edges.previous_vertices figure.edges subfigure

        let first_vertices (figure:Figure) =
            Edges.first_vertices figure.edges
            

        let vertices_referencing_lower_figure (figure:Figure) lower_figure = 
            figure.subfigures
            |> Dictionary.keys_with_value lower_figure 


        let subgraph_with_vertices 
            (target:Figure) 
            (vertices:Set<Vertex_id>)
            =
            vertices
            |>Edges.edges_between_vertices target.edges
            |>built.stencil_output target

