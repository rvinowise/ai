module rvinowise.ai.ui.painted.Expected_figure_prolongation

open Rubjerg.Graphviz
open System.IO
open System.Diagnostics

open rvinowise
open rvinowise.ai

let empty_root_graph name =
    let root = RootGraph.CreateNew(name, GraphType.Directed)
    root.SafeSetAttribute("rankdir", "LR", "")
    Node.IntroduceAttribute(root, "shape", "circle")
    root

let provide_subgraph_inside_graph
    (prolongation:Expected_figure_prolongation) 
    (graph:Graph)
    =
    prolongation.prolongated.edges
    |> Seq.iter (
        fun (edge: ai.figure.Edge) -> 
            let tail = graph.GetOrAddNode(edge.tail.id)
            let head = graph.GetOrAddNode(edge.head.id)
            graph.GetOrAddEdge(
                tail, head, ""
            ) |> ignore
    )
    prolongation.expected
    |> Seq.iter (
        fun (s) ->
            let expected = graph.GetOrAddNode(s.id)
            expected.SafeSetAttribute("fillcolor","red","")
            expected.SafeSetAttribute("style","filled","")
            //expected.SafeSetAttribute("textcolor","red","")
            //expected.SafeSetAttribute("color","red", "")
            ()
    )
    graph

let provide_cluster_inside_graph 
    name
    (graph:Graph)
    =
    graph.GetOrAddSubgraph(name)

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

    
    
