namespace rvinowise.ai.ui.painted


open rvinowise.ui
open rvinowise.ai
open rvinowise.ai.ui


module Expected_figure_prolongation =

    let mark_expected_nodes
        (prolongation:Expected_figure_prolongation) 
        (node: infrastructure.Node)
        =
        prolongation.expected
        |> Seq.iter (
            fun (vertex) ->
                node
                |>infrastructure.Graph.provide_vertex (Vertex_id.value vertex)
                |>rvinowise.ui.infrastructure.Graph.fill_with_color "red"
                |>ignore
        )
        node


    let with_expected_prolongation
        name
        (prolongation:Expected_figure_prolongation) 
        node
        =
        node
        |>infrastructure.Graph.provide_vertex name
        |>painted.Figure.add_figure prolongation.prolongated
        |>mark_expected_nodes
            prolongation
        |>ignore
        node



    let visualise_prolongation
        id
        (prolongation:Expected_figure_prolongation) 
        =
        id
        |>infrastructure.Graph.empty
        |>with_expected_prolongation 
            id
            prolongation
        |>image.open_image_of_graph

        
    
