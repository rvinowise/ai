namespace rvinowise.ai

    open rvinowise.ai.figure
    open rvinowise.ai.stencil
    open System.Collections.Generic

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Vertex =
        let ids<'Vertex when 'Vertex:>Vertex> (vertices:'Vertex seq)=
            vertices
            |>Seq.map (fun vertex->vertex.id)

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Subfigure =
        
        open System.Text.RegularExpressions
        let private remove_number label =
            Regex.Replace(label, @"[^a-zA-Z]", "")
                    
        let simple id =
            Subfigure(
                id,
                remove_number id
            )

        let many_simple ids =
            ids|>Seq.map simple

        let ofNode (node:Node) =
            match node.referenced with
                |Node_reference.Lower_figure figure_id -> 
                    Some (Subfigure(node.id, figure_id))
                | _ -> None

        

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Subfigures = 
        
        let referenced_figures (subfigures: Subfigure seq) =
            subfigures
            |>Seq.map(fun s->s.referenced)
            |>Set.ofSeq

        let pick_referencing_figure figure (subfigures: Subfigure seq) =
            subfigures
            |> Seq.filter (fun s->s.referenced = figure)

        
        // let ids (subfigures:Subfigure seq) =
        //     subfigures
        //     |>Seq.map (fun s -> s.id) 


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Nodes =

        let only_subfigures (nodes: Node seq) =
            nodes
            |>Seq.choose(fun n->
                Subfigure.ofNode n
            )

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Node =

        let stencil_out id =
            Node(
                id,
                Stencil_output
            )

        