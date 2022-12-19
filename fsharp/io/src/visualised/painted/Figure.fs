namespace rvinowise.ai.ui.painted

    open Rubjerg
    open Rubjerg.Graphviz
    open System.IO
    open System.Diagnostics

    open rvinowise
    open rvinowise.ai
    open rvinowise.ai.figure
    open rvinowise.ai.ui

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Stencil=

        let painted_node (node: stencil.Node)=
            let label = 
                match node.referenced with
                |Lower_figure id -> id
                |Stencil_output -> "out"
            painted.Node(node.id, label)

        let painted_edges (edges: stencil.Edge seq)=
            edges
            |>Seq.map (fun e->
                painted.Edge(
                    (painted_node e.tail),
                    (painted_node e.head)
                )
            )

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure=

        let painted_node (node: figure.Subfigure)=
            painted.Node(node.id, node.referenced)

        let painted_edges (edges: figure.Edge seq)=
            edges
            |>Seq.map (fun e->
                painted.Edge(
                    (painted_node e.tail),
                    (painted_node e.head)
                )
            )

        
    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Node =
        let set_attribute key value (element:Graphviz.Node) =
            element.SafeSetAttribute(key,value,"")
            element

