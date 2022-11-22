namespace rvinowise.ai

open rvinowise.ai.figure

type Expected_figure_prolongation = {
    prolongated: Figure
    expected: Subfigure Set
}


[<CompilationRepresentationAttribute(CompilationRepresentationFlags.ModuleSuffix)>]
module Expected_figure_prolongation =

    

    

    let from_figure (figure: Figure) :Expected_figure_prolongation =
        let first_subfigures = Figure.first_subfigures figure
        {
            Expected_figure_prolongation.prolongated=figure;
            Expected_figure_prolongation.expected=first_subfigures
        }

