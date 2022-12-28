namespace rvinowise.ai

open rvinowise.ai.figure

type Expected_figure_prolongation = {
    prolongated: Figure
    expected: Vertex_id Set
}


[<CompilationRepresentationAttribute(CompilationRepresentationFlags.ModuleSuffix)>]
module Expected_figure_prolongation =

    let from_figure (figure: Figure) =
        {
            prolongated=figure;
            expected=figure.graph
                |>Graph.first_vertices 
                |>Set.ofSeq
        }

