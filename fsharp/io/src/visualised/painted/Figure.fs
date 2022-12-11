namespace rvinowise.ai.ui.painted

    open Rubjerg
    open Rubjerg.Graphviz
    open System.IO
    open System.Diagnostics

    open rvinowise
    open rvinowise.ai
    open rvinowise.ai.figure

    type Node =
        struct
            val id:Node_id
            val label:string

            new(id, label) = {
                id=id; label=label;
            }
        end

    type Edge=
        struct
            val tail: Node
            val head: Node

            new (tail, head) = {
                tail=tail; head=head
            }
        end

    

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Stencil=

        let painted_node (node: stencil.Node)=
            let label = 
                match node.referenced with
                |Lower_figure id -> id
                |Stencil_output -> "out"
            Node(node.id, label)

        let painted_edges (edges: stencil.Edge seq)=
            edges
            |>Seq.map (fun e->
                Edge(
                    (painted_node e.tail),
                    (painted_node e.head)
                )
            )

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure=

        let painted_node (node: figure.Subfigure)=
            Node(node.id, node.referenced)

        let painted_edges (edges: figure.Edge seq)=
            edges
            |>Seq.map (fun e->
                Edge(
                    (painted_node e.tail),
                    (painted_node e.head)
                )
            )

        
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

        let provide_cluster_inside_graph 
            name
            (graph:Graph)
            =
            graph.GetOrAddSubgraph("cluster_"+name)
            |> set_attribute "label" name

        

        let provide_subgraph_inside_graph
            (subgraph_id: string)
            (edges: Edge seq)
            (graph: Graph)
            =
            edges
            |> Seq.iter (
                fun edge -> 
                    let tail = 
                        graph
                        |>provide_node (subgraph_id+edge.tail.id)
                        //|>Node.set_attribute "label" edge.tail.label
                        |>Node.set_attribute "label" edge.tail.id
                    
                    let head = 
                        graph
                        |>provide_node (subgraph_id+edge.head.id)
                        //|>Node.set_attribute "label" edge.head.label
                        |>Node.set_attribute "label" edge.head.id

                    graph.GetOrAddEdge(
                        tail, head, ""
                    ) |> ignore
            )
            graph


        let provide_clustered_subgraph_inside_root_graph
            name
            (edges:Edge seq) 
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
            |>empty_root_graph
            |>provide_clustered_subgraph_inside_root_graph figure.id 
                (Figure.painted_edges figure.edges)
            |>open_image_of_graph
            |>ignore