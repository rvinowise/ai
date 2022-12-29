(* incapsulate infrastructure - a library for painting graphs *)
namespace rvinowise.ui.infrastructure

    open Rubjerg
    open Rubjerg.Graphviz


    type External_graph = Rubjerg.Graphviz.Graph
    type External_root = Rubjerg.Graphviz.RootGraph
    type External_node = Rubjerg.Graphviz.Node
    
    type Graph = {
        root:External_root
        impl:External_graph
    }
    type Vertex = {
        impl:External_node
    }


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Vertex =
        let set_attribute key value (vertex:Vertex) =
            vertex.impl.SafeSetAttribute(key,value,"")
            vertex

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Graph=

        open rvinowise.ui

        let empty name=
            let root:External_root = External_root.CreateNew(name, GraphType.Directed)
            root.SafeSetAttribute("rankdir", "LR", "")
            {root=root;impl=root}
        
        let with_circle_vertices graph=
            External_node.IntroduceAttribute(graph.root, "shape", "circle")
            graph

        let with_attribute key value (graph:Graph) =
            graph.impl.SafeSetAttribute(key,value,"")
            graph
        
        let with_vertex 
            id
            (graph:Graph) 
            =
            graph.impl.GetOrAddNode(id)|>ignore
            graph
        
        let provide_vertex
            id
            (graph:Graph) 
            =
            {
                Vertex.impl=graph.impl.GetOrAddNode(id)
            }


        let with_edge
            tail
            head
            (graph:Graph) 
            =
            graph.impl.GetOrAddEdge(
                tail, head, ""
            )|>ignore
            graph


        let provide_cluster 
            name
            (owner_graph:Graph)
            =
            {
                root = owner_graph.root
                impl = owner_graph.impl.GetOrAddSubgraph("cluster_"+name);
            }
            |> with_attribute "label" name

        let provide_subgraph_inside_graph
            (subgraph_id: string)
            (edges: painted.Edge seq)
            (graph: Graph)
            =
            edges
            |> Seq.iter (
                fun edge -> 
                    let tail = 
                        graph
                        |>provide_vertex (subgraph_id+edge.tail.id)
                        //|>Node.set_attribute "label" edge.tail.label
                        |>Vertex.set_attribute "label" edge.tail.id
                    
                    let head = 
                        graph
                        |>provide_vertex (subgraph_id+edge.head.id)
                        //|>Node.set_attribute "label" edge.head.label
                        |>Vertex.set_attribute "label" edge.head.id

                    graph
                    |>provide_edge
                        tail head
                    |> ignore
            )
            graph

        let provide_clustered_subgraph
            name
            (edges:painted.Edge seq) 
            (graph:Graph)
            =
            graph
            |>provide_cluster_inside_graph name
            |>provide_subgraph_inside_graph name edges
            |>ignore
            graph