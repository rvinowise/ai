namespace rvinowise.ai

    open Xunit
    open FsUnit

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure=
        open rvinowise.ai
        open rvinowise
        open rvinowise.extensions
        
        
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

        let private id_of_a_sequence_from_edges (edges: Edge seq) =
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

        let id_of_a_sequence (figure:Figure) =
            if Seq.isEmpty figure.edges then 
                figure.subfigures
                |>Seq.head
                |>extensions.KeyValuePair.key
            else
                id_of_a_sequence_from_edges figure.edges

        
        
        let private the_only_vertex figure =
            figure.subfigures
            |>Seq.head 
            |>KeyValuePair.key
            |>Seq.singleton

        let first_vertices figure =
            if Seq.isEmpty figure.edges then
                the_only_vertex figure
            else
                Edges.first_vertices figure.edges

        let last_vertices figure =
            if Seq.isEmpty figure.edges then
                the_only_vertex figure
            else
                Edges.last_vertices figure.edges