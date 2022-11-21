module rvinowise.ai.ui.painted.Expected_figure_prolongation

open Rubjerg.Graphviz
open System
open System.IO
open System.Diagnostics

open rvinowise
open rvinowise.ai

module Node =
    let set_attribute key value (element:Node) =
        element.SafeSetAttribute(key,value,"")
        element

module Graph =
    let set_attribute key value (element:Graph) =
        element.SafeSetAttribute(key,value,"")
        element

    let provide_node 
        id
        (graph:Graph) 
        =
        graph.GetOrAddNode(id)

let empty_root_graph name =
    let root = RootGraph.CreateNew(name, GraphType.Directed)
    root.SafeSetAttribute("rankdir", "LR", "")
    Node.IntroduceAttribute(root, "shape", "circle")
    root


let provide_subgraph_inside_graph
    (prolongation:Expected_figure_prolongation) 
    (graph:Graph)
    =
    let subgraph_id = Guid.NewGuid()
    prolongation.prolongated.edges
    |> Seq.iter (
        fun (edge: ai.figure.Edge) -> 
            let tail = 
                graph
                |>Graph.provide_node (subgraph_id.ToString()+edge.tail.id)
                |>Node.set_attribute "label" edge.tail.id
            
            let head = 
                graph
                |>Graph.provide_node (subgraph_id.ToString()+edge.head.id)
                |>Node.set_attribute "label" edge.head.id

            graph.GetOrAddEdge(
                tail, head, ""
            ) |> ignore
    )
    prolongation.expected
    |> Seq.iter (
        fun (s) ->
            graph.GetOrAddNode(subgraph_id.ToString()+s.id)
            |>Node.set_attribute "fillcolor" "red"
            |>Node.set_attribute "style" "filled"
            |>ignore
    )
    graph

let provide_cluster_inside_graph 
    name
    (graph:Graph)
    =
   graph.GetOrAddSubgraph("cluster_"+name)
   |> Graph.set_attribute "label" name

let provide_clastered_subgraph_inside_root_graph
    name
    (prolongation:Expected_figure_prolongation) 
    (root:RootGraph)
    =
    root
    |>provide_cluster_inside_graph name
    |>provide_subgraph_inside_graph prolongation
    |>ignore
    root


type Frame={
    prolongation: Expected_figure_prolongation;
    comment: string
}

let add_several_prolongation_steps_to_root 
    (frames: Frame seq)
    (root: RootGraph)
    =
    frames
    |>Seq.map (fun f->
        provide_subgraph_inside_graph f.prolongation root
    ) |> ignore
    root

let open_image_of_graph (root:RootGraph) =
    let filename = Directory.GetCurrentDirectory() + "/out"
    root.ComputeLayout()
    root.ToSvgFile(filename+".svg")
    root.ToDotFile(filename+".dot")
    root.FreeLayout()
    Process.Start("cmd", $"/c {filename}.svg") |> ignore
    ()

let visualise_prolongation 
    (prolongation:Expected_figure_prolongation) 
    =
    prolongation.prolongated.id
    |>empty_root_graph
    |>provide_clastered_subgraph_inside_root_graph prolongation.prolongated.id prolongation
    |>open_image_of_graph
    |>ignore

    
    
