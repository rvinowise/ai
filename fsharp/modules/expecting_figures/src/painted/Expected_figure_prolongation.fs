module rvinowise.ai.ui.painted.Expected_figure_prolongation

open Rubjerg.Graphviz
open System
open System.IO
open System.Diagnostics

open rvinowise
open rvinowise.ai
open rvinowise.ai.ui.painted



type External_graph = Rubjerg.Graphviz.Graph


let mark_expected_nodes
    (prolongation:Expected_figure_prolongation) 
    subgraph_id
    (graph:External_graph)
    =
    prolongation.expected
    |> Seq.iter (
        fun (vertex) ->
            graph.GetOrAddNode(subgraph_id+vertex)
            |>Node.set_attribute "fillcolor" "red"
            |>Node.set_attribute "style" "filled"
            |>ignore
    )
    graph


let provide_expected_prolongation_inside_graph
    name
    (prolongation:Expected_figure_prolongation) 
    (graph:RootGraph)
    =
    let subgraph_id = prolongation.prolongated.graph.id
    graph
    |>Graph.provide_cluster_inside_graph name
    |>Graph.provide_subgraph_inside_graph 
        name 
        (Figure.painted_edges prolongation.prolongated)
    |>mark_expected_nodes
        prolongation
        name
    |>ignore
    graph




let visualise_prolongation 
    (prolongation:Expected_figure_prolongation) 
    =
    prolongation.prolongated.graph.id
    |>Graph.empty_root_graph
    |>provide_expected_prolongation_inside_graph 
        prolongation.prolongated.graph.id
        prolongation
    |>Graph.open_image_of_graph
    |>ignore

    
    
