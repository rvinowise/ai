namespace rvinowise.ai.ui.painted

open Rubjerg
open Rubjerg.Graphviz
open System.IO
open System.Diagnostics
open System

open rvinowise
open rvinowise.ai
open rvinowise.ai.figure

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Node =
    let set_attribute key value (element:Graphviz.Node) =
        element.SafeSetAttribute(key,value,"")
        element

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Graph =

    let empty_root_graph name =
        let root = RootGraph.CreateNew(name, GraphType.Directed)
        root.SafeSetAttribute("rankdir", "LR", "")
        Node.IntroduceAttribute(root, "shape", "circle")
        root
    
    let set_attribute key value (element:Graph) =
        element.SafeSetAttribute(key,value,"")
        element

    let provide_node 
        id
        (graph:Graph) 
        =
        graph.GetOrAddNode(id)


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Figure=

    let provide_cluster_inside_graph 
        name
        (graph:Graph)
        =
        graph.GetOrAddSubgraph("cluster_"+name)
        |> Graph.set_attribute "label" name


    let provide_subgraph_inside_graph
        (subgraph_id: string)
        (edges: ai.figure.Edge seq)
        (graph: Graph)
        =
        edges
        |> Seq.iter (
            fun (edge: ai.figure.Edge) -> 
                let tail = 
                    graph
                    |>Graph.provide_node (subgraph_id+edge.tail.id)
                    |>Node.set_attribute "label" edge.tail.id
                
                let head = 
                    graph
                    |>Graph.provide_node (subgraph_id+edge.head.id)
                    |>Node.set_attribute "label" edge.head.id

                graph.GetOrAddEdge(
                    tail, head, ""
                ) |> ignore
        )
        graph    


    let provide_clustered_subgraph_inside_root_graph
        name
        (edges:ai.figure.Edge seq) 
        (root:RootGraph)
        =
        root
        |>provide_cluster_inside_graph name
        |>provide_subgraph_inside_graph name edges
        |>ignore
        root


    
    let open_image_of_graph (root:RootGraph) =
        let filename = Directory.GetCurrentDirectory() + "/out"
        root.ComputeLayout()
        root.ToSvgFile(filename+".svg")
        root.ToDotFile(filename+".dot")
        root.FreeLayout()
        Process.Start("cmd", $"/c {filename}.svg") |> ignore
        ()

    let visualise_figure 
        (figure:Figure) 
        =
        figure.id
        |>Graph.empty_root_graph
        |>provide_clustered_subgraph_inside_root_graph figure.id figure.edges
        |>open_image_of_graph
        |>ignore

    

    let create_history =
        let root = RootGraph.CreateNew("history", GraphType.Directed)
        root.SafeSetAttribute("rankdir", "LR", "")

        Node.IntroduceAttribute(root, "shape", "box")

        // The node names are unique identifiers within a graph in Graphviz
        let nodeA = root.GetOrAddNode("1")
        let nodeB = root.GetOrAddNode("2")
        let nodeC = root.GetOrAddNode("3")
        let nodeD = root.GetOrAddNode("4")

        // The edge name is only unique between two nodes
        let edgeAB = root.GetOrAddEdge(nodeA, nodeB, "Some edge name")
        let edgeBC = root.GetOrAddEdge(nodeB, nodeC, "Some edge name")
        let cd = root.GetOrAddEdge(nodeC, nodeD, "Another edge name")

        let filename = Directory.GetCurrentDirectory() + "/out.svg"
        root.ComputeLayout()
        root.ToSvgFile(filename)
        root.FreeLayout()
        Process.Start("cmd", $"/c {filename}") |> ignore
        //root.ToDotFile(Directory.GetCurrentDirectory() + "/out.dot")
        ()