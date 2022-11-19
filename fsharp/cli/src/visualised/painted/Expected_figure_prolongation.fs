module rvinowise.ai.ui.painted.Expected_figure_prolongation

open Rubjerg.Graphviz
open System.IO
open System.Diagnostics

open rvinowise
open rvinowise.ai.figure.Expecting_figures


let image comment expected =
    let root = RootGraph.CreateNew(comment, GraphType.Directed)
    root.SafeSetAttribute("rankdir", "LR", "")
    Node.IntroduceAttribute(root, "shape", "circle")

    expected.prolongated
    |> Seq.iter (
        fun (edge: ai.figure.Edge) -> 
            let head = root.GetOrAddNode(edge.head.id)
            let tail = root.GetOrAddNode(edge.tail.id)
            root.GetOrAddEdge(
                head, tail, ""
            ) |> ignore
    )

    let filename = Directory.GetCurrentDirectory() + "/out.svg"
    root.ComputeLayout()
    root.ToSvgFile(filename)
    root.FreeLayout()
    Process.Start("cmd", $"/c {filename}") |> ignore
    //root.ToDotFile(Directory.GetCurrentDirectory() + "/out.dot")
    ()
