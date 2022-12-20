namespace rvinowise.ai

    open rvinowise.ai.figure
    open rvinowise.ai.stencil
    open System.Collections.Generic

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

        let outgoing_edges 
            (edges: figure.Edge seq) 
            node_id
            =
            edges
            |>Seq.filter (fun e->
                e.tail.id = node_id
            )

        

        

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

        let incoming_edges (edges: stencil.Edge seq) (node:Node_id) =
            edges
            |>Seq.filter (fun e->e.head.id = node)

        let next_nodes (edges: stencil.Edge seq) (node:Node_id) =
            edges
            |>Seq.filter (fun e->e.tail.id = node)
            |>Seq.map (fun e->e.head)

        let previous_nodes (edges: stencil.Edge seq) (node:Node_id) =
            node
            |>incoming_edges edges
            |>Seq.map (fun e->e.tail)
        
        let previous_subfigures edges node =
            node
            |>previous_nodes edges
            |>Nodes.only_subfigures

        

        let previous_subfigures_jumping_over_outputs edges node =
            node
            |>incoming_edges edges
            |>Seq.collect (fun edge->
                match Subfigure.ofNode edge.tail with
                |Some previous_subfigure -> Seq.ofList [previous_subfigure]
                |None -> (previous_subfigures edges edge.tail.id)
            )
            
    