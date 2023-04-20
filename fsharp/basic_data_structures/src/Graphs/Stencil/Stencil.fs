namespace rvinowise.ai

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Stencil=

    open rvinowise.ai
    open rvinowise.extensions

    let output (stencil: Stencil) =
        stencil.nodes
        |>Seq.pick (fun pair->
            match pair.Value with
            |Stencil_output _ -> Some pair.Key
            |_->None
            //vertex.Value=Stencil_output
        )

    let is_output stencil vertex=
        vertex
        |>Dictionary.some_value stencil.nodes
        |>function
        |None -> false
        |Some node ->
            match node with
            |Stencil_output _ ->true
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
            vertex
            |>Dictionary.some_value stencil.nodes
            |>function
            |None -> None
            |Some node ->
                match node with
                |Lower_figure figure->Some (vertex,figure)
                |Stencil_output _ -> None
        )

    let next_subfigures_of_many (stencil: Stencil) vertices =
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

    
    
    let nonexistent_vertex =  "0"|>Figure_id|>Lower_figure

    let referenced_node stencil vertex_id =
        vertex_id
        |>Dictionary.some_value stencil.nodes
        |>function
        |Some node_reference -> node_reference
        |None -> nonexistent_vertex
    
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

   