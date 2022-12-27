namespace rvinowise.ai

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Stencil =
        open rvinowise.ai.stencil

        let regular (id:string) (edges:Edge seq)=
            {Stencil.id=id;edges=edges}

        // let first_subfigures (stencil:Stencil) =
        //     stencil.edges
        //     |>Edges.first_vertices
        //     |>Nodes.only_subfigures

        let outputs (stencil: Stencil) =
            stencil.edges
            |>Edges.all_vertices
            |>Seq.filter (fun node->
                node.referenced=Stencil_output
            )
            |>Seq.distinct

        let previous_subfigures_jumping_over_outputs
            (edges: Edge seq) 
            vertex 
            =
            vertex
            |>Edges.incoming_edges edges
            |>Seq.collect (fun edge->
                match Subfigure.ofNode edge.tail with
                |Some previous_subfigure -> Seq.ofList [previous_subfigure.id]
                |None -> (Edges.previous_vertices edges edge.tail)
            )