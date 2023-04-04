namespace rvinowise.ai

    open Xunit
    open FsUnit

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure=
        open rvinowise.ai
        open rvinowise
        open rvinowise.extensions
        
        
        let need_vertex_referencing_element 
            (owner_figure:Figure)
            (referenced_element)
            (checked_vertex) =
            let (exist,reference) = owner_figure.subfigures.TryGetValue(checked_vertex)
            exist && reference=referenced_element

        let nonexistent_vertex = Figure_id "" 

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

        let is_empty (figure:Figure) =
            figure.subfigures
            |>Map.isEmpty

        let id_of_a_sequence (figure:Figure) =
            if Seq.isEmpty figure.edges then 
                figure.subfigures
                |>Seq.head
                |>extensions.KeyValuePair.value
            else
                printed.Figure.id_of_a_sequence_from_edges figure.edges figure.subfigures

        
        
        let private try_the_only_vertex figure =
            figure.subfigures
            |>Seq.tryHead 
            |>function
            |Some pair->
                pair
                |>KeyValuePair.key
                |>Seq.singleton
            |None->Seq.empty

        let first_vertices figure =
            if Seq.isEmpty figure.edges then
                try_the_only_vertex figure
            else
                Edges.first_vertices figure.edges

        let last_vertices figure =
            if Seq.isEmpty figure.edges then
                try_the_only_vertex figure
            else
                Edges.last_vertices figure.edges

        
        

        