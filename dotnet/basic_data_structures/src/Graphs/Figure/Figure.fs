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
        |>Seq.choose (fun vertex->
            owner_figure.subfigures
            |>Map.tryFind vertex
            |>function
            |None -> None
            |Some referenced_figure ->Some (vertex,referenced_figure)
        )

    

    let has_edges (figure:Figure) =
        figure.edges
        |>Seq.isEmpty|>not

    

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

    let first_referenced_figures figure =
        figure
        |>first_vertices
        |>referenced_figures figure

    let last_vertices figure =
        if Seq.isEmpty figure.edges then
            try_the_only_vertex figure
        else
            Edges.last_vertices figure.edges

    let impossible_parts (owner_figure:Figure) =
        owner_figure.without
    

    