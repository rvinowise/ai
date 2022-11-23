namespace rvinowise.ai



type Node_reference =
    | Lower_figure of Figure_id
    | Stencil_output



type Subfigure = 
    struct
        val id: Node_id
        val referenced: Figure_id

        new (id, referenced) =
            {id = id; referenced = referenced;}
        new (id: Node_id) =
            {id = id; referenced = id;}
    end

type Node =
    struct
        val id: Node_id
        val referenced: Node_reference

        // override this.Equals(other) =
        //     match other with
        //     | :? Node as other -> 
        //         this.id = other.id && this.referenced = other.referenced
        //     | :? Subfigure as other -> 
        //         other.id = this.id 
        //         &&
        //         match this.referenced with
        //         | Lower_figure figure_id -> 
        //             figure_id = other.referenced
        //         | _ -> false
        //     | _ -> false
    end

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

    let only_subfigures (nodes: Node seq) =
        nodes
        |>Seq.choose(fun n->
            Subfigure.ofNode n
        )

