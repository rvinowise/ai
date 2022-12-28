namespace rvinowise.ai.ui.painted

    open Rubjerg
    open Rubjerg.Graphviz
    open System.IO
    open System.Diagnostics

    open rvinowise
    open rvinowise.ai
    open rvinowise.ai.figure
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
                    Stencil.node_with_id stencil vertex
                with
                |Some (Lower_figure id) -> id
                |Some (Stencil_output) -> "out"
                |None -> "?"
            painted.Node(vertex, label)

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
                
            painted.Node(vertex, label)

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

        
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Node =
        let set_attribute key value (element:Graphviz.Node) =
            element.SafeSetAttribute(key,value,"")
            element

