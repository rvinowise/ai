namespace rvinowise.ai

open rvinowise.ai
open rvinowise.extensions


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Figure=

    let nonexistent_vertex = Figure_id "nonexistent"
    let nonexistent_constant_figure = Constant_figure_id 0

    let reference_of_vertex 
        owner_figure 
        vertex
        =
        match
            Dictionary.some_value owner_figure.targets vertex 
        with
        | Some referenced_figure -> referenced_figure
        | None -> nonexistent_constant_figure


    let is_vertex_referencing_target 
        targets
        referenced_target
        checked_vertex
        =
        checked_vertex
        |>Dictionary.some_value targets
            = Some(referenced_target)

    let all_vertices_referencing_figure lower_figure owner_figure  = 
        lower_figure 
        |> Dictionary.keys_with_value owner_figure.targets  

    let vertices_referencing_target 
        search_in_these_vertices
        (referenced_target: 'Target)
        targets
        =
        search_in_these_vertices
        |>Seq.filter (
            is_vertex_referencing_target
                targets
                referenced_target
        )

    let referenced_figures 
        (owner_figure: Figure<_>)
        (subfigures:Vertex_id seq)
        =
        subfigures
        |>Seq.choose (Dictionary.some_value owner_figure.targets)
        |>Seq.distinct

    let referenced_targets
        (targets: 'Targets)
        (subfigures:Vertex_id seq)
        =
        subfigures
        |>Seq.choose (Dictionary.some_value targets)
        |>Seq.distinct
    
    let vertices_with_their_referenced_targets 
        targets
        vertices
        =
        vertices
        |>Seq.choose (fun vertex->
            targets
            |>Map.tryFind vertex
            |>function
            |None -> None
            |Some referenced_figure ->Some (vertex,referenced_figure)
        )


    let has_edges (figure:Figure<_>) =
        figure.edges
        |>Seq.isEmpty|>not

    

    let name_of_a_sequence (figure:Figure<_>) =
        if Seq.isEmpty figure.edges then 
            figure.targets
            |>Seq.head
            |>_.Key
            |>Vertex_id.value
        else
            figure.targets
            |>Map.map (fun vertex _ -> Vertex_id.value vertex)
            |>Figure_printing.name_of_a_sequence_from_edges figure.edges

    
    let private try_the_only_vertex (figure:Figure<_>) =
        figure.targets
        |>Seq.tryHead 
        |>function
        |Some pair->
            Seq.singleton pair.Key
        |None->Seq.empty

    let first_vertices (figure: Figure<_>) =
        if Seq.isEmpty figure.edges then
            try_the_only_vertex figure
        else
            Edges.first_vertices figure.edges

    let first_referenced_figures figure =
        figure
        |>first_vertices
        |>referenced_figures figure

    let last_vertices (figure:Figure<_>) =
        if Seq.isEmpty figure.edges then
            try_the_only_vertex figure
        else
            Edges.last_vertices figure.edges

    let is_signal name (figure:Figure<_>) =
        figure.targets.Count = 1
        // &&
        // figure.targets
        // |>Map.toSeq|>Seq.head|>snd|>_.name
        // |>Figure_id.value = name