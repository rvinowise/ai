module rvinowise.ai.ui.painted.Expected_figure_prolongation

open Rubjerg.Graphviz
open System
open System.IO
open System.Diagnostics

open rvinowise
open rvinowise.ai
open rvinowise.ai.ui.painted






let mark_expected_nodes
    (prolongation:Expected_figure_prolongation) 
    subgraph_id
    (graph:Graph)
    =
    prolongation.expected
    |> Seq.iter (
        fun (s) ->
            graph.GetOrAddNode(subgraph_id+s.id)
            |>Node.set_attribute "fillcolor" "red"
            |>Node.set_attribute "style" "filled"
            |>ignore
    )
    graph


let provide_expected_prolongation_inside_graph
    (subgraph_id:string)
    (prolongation:Expected_figure_prolongation) 
    (graph:RootGraph)
    =
    graph
    |>Figure.provide_cluster_inside_graph subgraph_id
    |>Figure.provide_subgraph_inside_graph 
        subgraph_id
        prolongation.prolongated.edges
    |>mark_expected_nodes
        prolongation
        subgraph_id
    |>ignore
    graph




let visualise_prolongation 
    (prolongation:Expected_figure_prolongation) 
    =
    prolongation.prolongated.id
    |>empty_root_graph
    |>provide_expected_prolongation_inside_graph prolongation.prolongated.id prolongation
    |>open_image_of_graph
    |>ignore

    
    