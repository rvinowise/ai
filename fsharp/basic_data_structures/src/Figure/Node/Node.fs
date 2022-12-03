namespace rvinowise.ai

    type Node_reference =
    | Lower_figure of Figure_id
    | Stencil_output


namespace rvinowise.ai.figure
    open rvinowise.ai

    type Subfigure = 
        struct
            val id: Node_id
            val referenced: Figure_id

            new (id, referenced) =
                {id = id; referenced = referenced;}
            new (id: Node_id) =
                {id = id; referenced = id;}
        end

namespace rvinowise.ai.stencil
    open rvinowise.ai

    type Node =
        struct
            val id: Node_id
            val referenced: Node_reference

            new (id) =
                {id = id; referenced = Lower_figure id;}
            new (id, referenced: string) =
                {id = id; referenced = Lower_figure referenced;}
            new (id, referenced) =
                {id = id; referenced = referenced;}
        end

namespace rvinowise.ai

    open rvinowise.ai.figure
    open rvinowise.ai.stencil

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Subfigure = 
        
        let referenced_figures (subfigures: Subfigure seq) =
            subfigures
            |>Seq.map(fun s->s.referenced)
            |>Set.ofSeq

        let referencing_figure figure (subfigures: Subfigure seq) =
            subfigures
            |> Seq.filter (fun s->s.referenced = figure)

        let ofNode (node:Node) =
            match node.referenced with
                |Node_reference.Lower_figure figure_id -> 
                    Some (Subfigure(node.id, figure_id))
                | _ -> None
            

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Node =

        open System.Text.RegularExpressions

        let only_subfigures (nodes: Node seq) =
            nodes
            |>Seq.choose(fun n->
                Subfigure.ofNode n
            )

        let stencil_out id =
            Node(
                id,
                Stencil_output
            )