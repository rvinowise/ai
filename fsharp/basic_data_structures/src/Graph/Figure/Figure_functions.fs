namespace rvinowise.ai

    open Xunit
    open FsUnit

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure=
        open rvinowise.ai.figure
        open rvinowise
        open rvinowise.extensions
        
        let regular (id:string) (edges:Edge seq)=
            {id=id;edges=edges}
            
        let stencil_output (edges:Edge seq)=
            regular "out" edges

        let empty id = regular id []

        [<Fact>]
        let ``equality comparison``()=
            let f1 = regular "F" [
                Edge(Subfigure("a0","a"), Subfigure("b0","b"))
            ]
            let f2 = regular "F" [
                Edge(Subfigure("a0","a"), Subfigure("b0","b"))
            ]
            f1 |>should equal f2
        
        let need_every_subfigure subfigure =
            true

        let need_subfigure_referencing_figure 
            referenced_figure
            (subfigure: Subfigure) =
            subfigure.referenced = referenced_figure

        let subfigure_with_id id =
            

        let subfigures_reacheble_from_other_subfigures
            (is_needed: Subfigure->bool)
            (figure_in_which_search: Figure)
            (subfigures_before_goals: Vertex_id seq)
            =
            Edges.vertices_reacheble_from_other_vertices
                (is_needed (subfigure_with_id))
                figure_in_which_search.edges
                subfigures_before_goals
    
        let subfigures_reaching_other_subfigures
            (is_needed: Subfigure->bool)
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
                need_every_subfigure
                figure.Example.a_high_level_relatively_simple_figure
                (["b0";"b2"]|>Subfigure.many_simple)
            |> should equal (["f1"]|>Subfigure.many_simple)

        [<Fact>]
        let ``subfigures reaching others``()=
            subfigures_reaching_other_subfigures
                need_every_subfigure
                figure.Example.a_high_level_relatively_simple_figure
                (["b1";"f1"]|>Subfigure.many_simple)
            |> should equal (["b0"]|>Subfigure.many_simple)
        

        let next_subfigures figure subfigure=
            Edges.next_vertices figure.edges subfigure

        let previous_subfigures figure subfigure=
            Edges.previous_vertices figure.edges subfigure

        let first_subfigures (figure:Figure) =
            Edges.first_subfigures figure.edges
            

        let subfigure_occurances (subfigure:Figure) (figure:Figure) =
            figure.edges
            |>Seq.collect (fun e->
                [e.tail; e.head]
            )
            |>Set.ofSeq

        let lower_figures (figure:Figure) =
            figure.edges
            |>Edges.all_subfigures
            |>Subfigures.referenced_figures
            |>Set.ofSeq

        let subfigures (figure:Figure) =
            figure.edges
            |>Edges.all_subfigures

        let nodes_referencing_lower_figure figure lower_figure = 
            figure
            |> subfigures 
            |> Subfigures.pick_referencing_figure lower_figure

        let subfigures_with_ids ids (figure:Figure)=
            figure.edges
            |>  Edges.subfigures_with_ids ids


        

        let subgraph_with_vertices 
            (target:Figure) 
            (vertices:Set<Vertex>)
            =
            vertices
            |>Edges.edges_between_vertices target.edges
            |>stencil_output

