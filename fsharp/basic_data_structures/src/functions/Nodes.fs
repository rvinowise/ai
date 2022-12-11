namespace rvinowise.ai

    open rvinowise.ai.figure
    open rvinowise.ai.stencil


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Subfigure =
        
        let ofNode (node:Node) =
            match node.referenced with
                |Node_reference.Lower_figure figure_id -> 
                    Some (Subfigure(node.id, figure_id))
                | _ -> None

        let next_subfigures 
            (edges: figure.Edge seq)
            (subfigure: Node_id) 
            =
            edges
            |>Seq.filter (fun e->e.tail.id = subfigure)
            |>Seq.map (fun e->e.head)

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Subfigures = 
        
        let referenced_figures (subfigures: Subfigure seq) =
            subfigures
            |>Seq.map(fun s->s.referenced)
            |>Set.ofSeq

        let pick_referencing_figure figure (subfigures: Subfigure seq) =
            subfigures
            |> Seq.filter (fun s->s.referenced = figure)

        
        let ids (subfigures:Subfigure seq) =
            subfigures
            |>Seq.map (fun s -> s.id) 


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Node =

        let stencil_out id =
            Node(
                id,
                Stencil_output
            )

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Nodes =

        open System.Text.RegularExpressions

        let only_subfigures (nodes: Node seq) =
            nodes
            |>Seq.choose(fun n->
                Subfigure.ofNode n
            )


    