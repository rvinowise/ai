namespace rvinowise.ai

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Stencil=

    open rvinowise.ai
    open rvinowise.extensions

    let outputs (stencil: Stencil) =
        stencil.nodes
        |>Seq.filter (fun vertex->
            vertex.Value=Stencil_output
        )
        |>Seq.map KeyValuePair.key
        |>Seq.distinct

    let is_output stencil vertex=
        vertex
        |>Dictionary.some_value stencil.nodes
        |>function
        |None -> false
        |Some node -> node=Node_reference.Stencil_output

    let is_subfigure stencil vertex =
        vertex
        |>is_output stencil
        |>not

    

    let only_subfigures stencil vertices=
        vertices
        |>Seq.filter (is_subfigure stencil)

    let next_subfigures subfigures (stencil: Stencil)=
        subfigures
        |>Seq.collect (Edges.next_vertices stencil.graph.edges)
        |>Seq.distinct
        |>only_subfigures stencil

    let previous_subfigures_jumping_over_outputs
        stencil
        vertex 
        =
        vertex
        |>Edges.incoming_edges stencil.graph.edges
        |>Seq.collect (fun edge->
            if is_output stencil edge.tail then
                Edges.previous_vertices stencil.graph.edges edge.tail
            else
                Seq.ofList [edge.tail]
        )
    
    let is_vertex_referencing_figure
        owner_stencil 
        referenced_figure
        checked_vertex
        =
        checked_vertex
        |>Dictionary.some_value owner_stencil.nodes
            = Some (Node_reference.Lower_figure referenced_figure)

    let vertices_referencing_figure 
        owner_stencil
        search_in_these_vertices
        referenced_figure
        =
        search_in_these_vertices
        |>Seq.filter (
            is_vertex_referencing_figure
                owner_stencil
                referenced_figure
        )
    
    let nonexistent_vertex = Node_reference.Lower_figure "0" 

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
        stencil.graph
        |>Graph.first_vertices 
        |>Seq.filter (is_subfigure stencil)

    let first_referenced_figures stencil=
        Graph.first_vertices stencil.graph
        |>Seq.map (referenced_node stencil)
        |>Seq.choose (function
            |Node_reference.Lower_figure referenced_figure -> Some referenced_figure
            |_->None
        )
        |>Seq.distinct