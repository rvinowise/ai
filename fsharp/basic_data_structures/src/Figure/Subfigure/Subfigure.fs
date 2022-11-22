namespace rvinowise.ai.figure

open rvinowise.ai


type Subfigure_reference =
    | Lower_figure of Figure_id
    | Stencil_output

type Subfigure = 
    struct
        val id: Subfigure_id
        val referenced: Subfigure_reference

        new (id, referenced) =
            {id = id; referenced = referenced;}
        new (id: Subfigure_id) =
            {id = id; referenced = Lower_figure id;}
    end

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Subfigure = 
    
    let participating_figures (subfigures: Subfigure seq) =
        subfigures
        |>Seq.choose(fun s->
            match s.referenced with
            |Lower_figure id -> Some id
            | _ -> None
        )

    let referencing_figure figure (subfigures: Subfigure seq) =
        subfigures
        |> Seq.filter (fun s->
            match s.referenced with
            |Subfigure_reference.Lower_figure id ->
                id = figure
            | _ -> false
        )