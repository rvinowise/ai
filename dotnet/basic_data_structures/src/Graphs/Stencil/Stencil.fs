namespace rvinowise.ai

open rvinowise.extensions


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Stencil=

    let output (stencil: Stencil) =
        stencil.nodes
        |>Seq.pick (fun pair->
            match pair.Value with
            |Stencil_output -> Some pair.Key
            |_->None
        )

    let is_output stencil vertex =
        stencil.nodes
        |>Map.find vertex 
        |>function
        |Stencil_output ->true
        |_->false

    let is_subfigure stencil vertex =
        vertex
        |>is_output stencil
        |>not

    let only_subfigures stencil vertices =
        vertices
        |>Seq.filter (is_subfigure stencil)

    let only_subfigures_with_figures stencil vertices =
        vertices
        |>Seq.choose (fun vertex->
            stencil.nodes
            |>Map.find vertex 
            |>function
            |Lower_figure figure->Some (vertex,figure)
            |Stencil_output -> None
        )

    let next_subfigures_of_many stencil vertices =
        vertices
        |>Edges.next_vertices_of_many stencil.edges
        |>only_subfigures stencil

    let previous_subfigures_jumping_over_outputs
        stencil
        vertex 
        =
        vertex
        |>Edges.incoming_edges stencil.edges
        |>Seq.collect (fun edge->
            if is_output stencil edge.tail then
                Edges.previous_vertices stencil.edges edge.tail
            else
                Set.ofList [edge.tail]
        )
    
    let is_vertex_referencing_figure
        owner_stencil 
        (referenced_figure: Figure_id)
        checked_vertex
        =
        checked_vertex
        |>Dictionary.some_value owner_stencil.nodes
            = Some (Lower_figure referenced_figure)

    
    let referenced_node stencil vertex_id =
        stencil.nodes
        |>Map.find vertex_id
    
    let referenced_nodes
        owner_stencil
        (vertices:Vertex_id seq)
        =
        vertices
        |>Seq.choose (Dictionary.some_value owner_stencil.nodes)

    let first_subfigures stencil=
        stencil.edges
        |>Edges.first_vertices 
        |>Seq.filter (is_subfigure stencil)

    let first_referenced_figures stencil=
        Edges.first_vertices stencil.edges
        |>Seq.map (referenced_node stencil)
        |>Seq.choose (function
            |Stencil_node.Lower_figure name -> Some name
            |_->None
        )
        |>Seq.distinct
    
    let impossible_output_parts (stentcil:Stencil) =
        stentcil.output_without

   