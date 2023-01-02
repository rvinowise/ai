module rvinowise.ai.ui.painted.Expected_figure_prolongation

open Rubjerg.Graphviz
open System
open System.IO
open System.Diagnostics

open rvinowise.ui
open rvinowise.ai
open rvinowise.ai.ui




let mark_expected_nodes
    (prolongation:Expected_figure_prolongation) 
    subgraph_id
    (graph: infrastructure.Node)
    =
    prolongation.expected
    |> Seq.iter (
        fun (vertex) ->
            graph
            |>infrastructure.Graph.provide_vertex (subgraph_id+vertex)
            |>rvinowise.ui.infrastructure.Graph.fill_with_color "red"
            |>ignore
    )
    graph


let with_expected_prolongation
    name
    (prolongation:Expected_figure_prolongation) 
    (graph:infrastructure.Node)
    =
    graph
    |>infrastructure.Graph.provide_vertex name
    |>painted.Figure.add_figure prolongation.prolongated
    |>mark_expected_nodes
        prolongation
        prolongation.prolongated.graph.id
    |>ignore
    graph



let visualise_prolongation 
    (prolongation:Expected_figure_prolongation) 
    =
    prolongation.prolongated.graph.id
    |>infrastructure.Graph.empty
    |>with_expected_prolongation 
        prolongation.prolongated.graph.id
        prolongation
    |>image.open_image_of_graph
    |>ignore

    
    
