namespace rvinowise.ai

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Stencil =
        open rvinowise.ai.stencil

        let first_subfigures (stencil:Stencil) =
            stencil.edges
            |>rvinowise.ai.stencil.Edges.first_nodes 
            |>Nodes.only_subfigures

        let outputs (stencil: Stencil) =
            stencil.edges
            |>Edges.all_nodes
            |>Seq.filter (fun node->
                node.referenced=Stencil_output
            )
            |>Seq.distinct