namespace rvinowise.ai

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Stencil =
        open rvinowise.ai.stencil
        open rvinowise.extensions

        let outputs (stencil: Stencil) =
            stencil.nodes
            |>Seq.filter (fun vertex->
                vertex.Value=Stencil_output
            )
            |>Seq.map KeyValuyePair.key
            |>Seq.distinct

        let is_output stencil vertex=
            vertex
            |>Dictionary.some_value stencil.nodes
            |>function
            |None -> false
            |Some node -> node=Node_reference.Stencil_output

        let first_subfigures stencil=
            Graph.first_vertices

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
        
        