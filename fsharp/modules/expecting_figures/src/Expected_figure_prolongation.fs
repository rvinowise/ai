namespace rvinowise.ai

open rvinowise.ai.figure

type Expected_figure_prolongation = {
    prolongated: Figure
    expected: Subfigure Set
}


[<CompilationRepresentationAttribute(CompilationRepresentationFlags.ModuleSuffix)>]
module Expected_figure_prolongation =

    

    

    let from_figure (figure: Figure) :Expected_figure_prolongation =
        {
            prolongated=figure;
            expected=figure
                        |>Figure.first_subfigures 
                        |>Set.ofSeq
        }

