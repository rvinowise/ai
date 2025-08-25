namespace rvinowise.ai

open rvinowise.ai

type Expected_figure_prolongation = {
    prolongated: Figure<Constant_figure_id>
    expected: Vertex_id Set
}


module Expected_figure_prolongation =

    let from_figure (figure: Figure<_>) =
        {
            prolongated=figure;
            expected=figure.edges
                |>Edges.first_vertices 
                |>Set.ofSeq
        }

