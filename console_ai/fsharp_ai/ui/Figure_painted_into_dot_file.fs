module rvinowise.ai.Figure_painted_into_dot_file

open Rubjerg.Graphviz
open System.IO
open System.Diagnostics


let create_graph =
    let root = RootGraph.CreateNew("Some Unique Identifier", GraphType.Directed);
    root.SafeSetAttribute("rankdir", "LR", "");

    Node.IntroduceAttribute(root, "shape", "circle");

    // The node names are unique identifiers within a graph in Graphviz
    let nodeA = root.GetOrAddNode("A");
    let nodeB = root.GetOrAddNode("B");
    let nodeC = root.GetOrAddNode("C");
    let nodeD = root.GetOrAddNode("D");

    // The edge name is only unique between two nodes
    let edgeAB = root.GetOrAddEdge(nodeA, nodeB, "Some edge name");
    let edgeBC = root.GetOrAddEdge(nodeB, nodeC, "Some edge name");
    let anotherEdgeBC = root.GetOrAddEdge(nodeB, nodeC, "Another edge name");

    let filename = Directory.GetCurrentDirectory() + "/out.svg"
    root.ComputeLayout();
    root.ToSvgFile(filename);
    root.FreeLayout();
    Process.Start("cmd", $"/c {filename}") |> ignore
    //root.ToDotFile(Directory.GetCurrentDirectory() + "/out.dot");
    ()

let create_history =
    let root = RootGraph.CreateNew("history", GraphType.Directed);
    root.SafeSetAttribute("rankdir", "LR", "");

    Node.IntroduceAttribute(root, "shape", "box");

    // The node names are unique identifiers within a graph in Graphviz
    let nodeA = root.GetOrAddNode("1");
    let nodeB = root.GetOrAddNode("2");
    let nodeC = root.GetOrAddNode("3");
    let nodeD = root.GetOrAddNode("4");

    // The edge name is only unique between two nodes
    let edgeAB = root.GetOrAddEdge(nodeA, nodeB, "Some edge name");
    let edgeBC = root.GetOrAddEdge(nodeB, nodeC, "Some edge name");
    let cd = root.GetOrAddEdge(nodeC, nodeD, "Another edge name");

    let filename = Directory.GetCurrentDirectory() + "/out.svg"
    root.ComputeLayout();
    root.ToSvgFile(filename);
    root.FreeLayout();
    Process.Start("cmd", $"/c {filename}") |> ignore
    //root.ToDotFile(Directory.GetCurrentDirectory() + "/out.dot");
    ()