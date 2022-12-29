namespace rvinowise.ai.ui.painted

    open Rubjerg
    open rvinowise.ai
    open rvinowise.ai.stencil
    open rvinowise.ai.ui

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Stencil=

        let painted_vertex 
            stencil
            vertex
            =
            let label = 
                match 
                    Stencil.referenced_node stencil vertex
                with
                |Lower_figure id -> id
                |Stencil_output -> "out"
            painted.Vertex(vertex, label)

        let painted_edges 
            (stencil:Stencil)
            =
            let edges = stencil.graph.edges
            edges
            |>Seq.map (fun e->
                painted.Edge(
                    (painted_vertex stencil e.tail),
                    (painted_vertex stencil e.head)
                )
            )

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure=

        let painted_vertex 
            figure
            vertex
            =
            let label = 
                Figure.reference_of_vertex figure vertex
                
            painted.Vertex(vertex, label)

        let painted_edges 
            figure
            =
            figure.graph.edges
            |>Seq.map (fun e->
                painted.Edge(
                    (painted_vertex figure e.tail),
                    (painted_vertex figure e.head)
                )
            )

        
    

