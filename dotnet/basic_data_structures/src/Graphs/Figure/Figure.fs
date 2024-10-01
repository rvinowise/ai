namespace rvinowise.ai

open rvinowise.ai
open rvinowise.extensions


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Figure=

    let need_vertex_referencing_element 
        (owner_figure:Figure)
        referenced_element
        checked_vertex =
        let exist,reference = owner_figure.subfigures.TryGetValue(checked_vertex)
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


    let is_vertex_referencing_figure 
        owner_figure
        referenced_figure
        checked_vertex
        =
        checked_vertex
        |>Dictionary.some_value owner_figure.subfigures
            = Some(referenced_figure)

    let all_vertices_referencing_figure lower_figure (owner_figure:Figure)  = 
        lower_figure 
        |> Dictionary.keys_with_value owner_figure.subfigures  

    let vertices_referencing_figure 
        search_in_these_vertices
        referenced_figure
        owner_figure
        =
        search_in_these_vertices
        |>Seq.filter (
            is_vertex_referencing_figure
                owner_figure
                referenced_figure
        )

    let referenced_figures 
        owner_figure
        (subfigures:Vertex_id seq)
        =
        subfigures
        |>Seq.choose (Dictionary.some_value owner_figure.subfigures)
        |>Seq.distinct

    let vertices_with_their_referenced_figures 
        (owner_figure:Figure)
        vertices
        =
        vertices
        |>Seq.map (fun vertex->
            vertex, owner_figure.subfigures[vertex]
        )

    

    let has_edges (figure:Figure) =
        figure.edges
        |>Seq.isEmpty|>not

    

    let id_of_a_sequence (figure:Figure) =
        if Seq.isEmpty figure.edges then 
            figure.subfigures
            |>Seq.head
            |>_.Value
        else
            Figure_printing.id_of_a_sequence_from_edges figure.edges figure.subfigures

    
    let private try_the_only_vertex figure =
        figure.subfigures
        |>Seq.tryHead 
        |>function
        |Some pair->
            Seq.singleton pair.Key
        |None->Seq.empty

    let first_vertices figure =
        if Seq.isEmpty figure.edges then
            try_the_only_vertex figure
        else
            Edges.first_vertices figure.edges

    let first_referenced_figures figure =
        figure
        |>first_vertices
        |>referenced_figures figure

    let last_vertices figure =
        if Seq.isEmpty figure.edges then
            try_the_only_vertex figure
        else
            Edges.last_vertices figure.edges

    let is_signal name figure =
        figure.subfigures.Count = 1
        &&
        figure.subfigures
        |>Map.toSeq|>Seq.head|>snd
        |>Figure_id.value = name