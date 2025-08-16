namespace rvinowise.ai

open rvinowise.ai

type Expected_figure_prolongation = {
    prolongated: Constant_figure
    expected: Vertex_id Set
}


module Expected_figure_prolongation =

    let from_figure (figure: Constant_figure) =
        {
            prolongated=figure;
            expected=figure.edges
                |>Edges.first_vertices 
                |>Set.ofSeq
        }

