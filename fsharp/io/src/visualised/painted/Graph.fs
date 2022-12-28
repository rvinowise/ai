namespace rvinowise.ai.ui.painted

    open Rubjerg
    open Rubjerg.Graphviz

    open rvinowise
    open rvinowise.ai
    open rvinowise.ai.figure
    open rvinowise.ai.ui
    open System.IO
    open System.Diagnostics

    type External_graph = Rubjerg.Graphviz.Graph
        
    module Graph =

        let empty_root_graph name =
            let root = RootGraph.CreateNew(name, GraphType.Directed)
            root.SafeSetAttribute("rankdir", "LR", "")
            Node.IntroduceAttribute(root, "shape", "circle")
            root
        
        let set_attribute key value (element:External_graph) =
            element.SafeSetAttribute(key,value,"")
            element

        let provide_node 
            id
            (graph:External_graph) 
            =
            graph.GetOrAddNode(id)

        let provide_cluster_inside_graph 
            name
            (graph:External_graph)
            =
            graph.GetOrAddSubgraph("cluster_"+name)
            |> set_attribute "label" name

        

        let provide_subgraph_inside_graph
            (subgraph_id: string)
            (edges: painted.Edge seq)
            (graph: External_graph)
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
            (edges:painted.Edge seq) 
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
            figure.graph.id
            |>empty_root_graph
            |>provide_clustered_subgraph_inside_root_graph figure.graph.id 
                (Figure.painted_edges figure)
            |>open_image_of_graph
            |>ignore