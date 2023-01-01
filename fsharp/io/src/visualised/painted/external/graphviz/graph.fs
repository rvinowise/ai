(* incapsulate infrastructure - a library for painting graphs *)
namespace rvinowise.ui.infrastructure

    open Rubjerg
    open Rubjerg.Graphviz


    type private External_graph = Rubjerg.Graphviz.Graph
    type private External_root = Rubjerg.Graphviz.RootGraph
    type private External_node = Rubjerg.Graphviz.Node
    
    type Graph = {
        id:string
        impl:External_graph
        root:External_root
    }
    type Vertex = {
        id:string
        parent:Graph
        impl:External_node
    }


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Vertex =
        let with_attribute key value (vertex:Vertex) =
            vertex.impl.SafeSetAttribute(key,value,"")
            vertex

        let fill_with_color color vertex =
            vertex
            |>with_attribute "fillcolor" color
            |>with_attribute "style" "filled"

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Graph=

        open rvinowise.ui

        let empty name=
            let root:External_root = External_root.CreateNew(name, GraphType.Directed)
            root.SafeSetAttribute("rankdir", "LR", "")
            {
                root=root;
                impl=root;
                id=name;
            }
        
        let with_circle_vertices graph=
            External_node.IntroduceAttribute(graph.root, "shape", "circle")
            graph
        
        let with_rectangle_vertices graph=
            External_node.IntroduceAttribute(graph.root, "shape", "rectangle")
            graph

        let with_attribute key value (graph:Graph) =
            graph.impl.SafeSetAttribute(key,value,"")
            graph
        
        let provide_vertex
            id
            (graph:Graph) 
            =
            {
                Vertex.id = id
                parent = graph
                impl=graph.impl.GetOrAddNode(graph.id+id)
            }
            |>Vertex.with_attribute "label" id

        let with_vertex 
            id
            (graph:Graph) 
            =
            provide_vertex id graph |>ignore
            graph
        

        let with_edge
            tail
            head
            (graph:Graph) 
            =
            graph.impl.GetOrAddEdge(
                tail.impl, head.impl, ""
            )|>ignore
            graph


        let provide_cluster 
            name
            (owner_graph:Graph)
            =
            let cluster_id = "cluster_"+owner_graph.id+name
            {
                root = owner_graph.root
                impl = owner_graph.impl.GetOrAddSubgraph(cluster_id); //graphviz needs clusters to have word "cluster" in their name
                id = cluster_id
            }
            |> with_attribute "label" name

        let with_cluster
            name
            how_to_fill
            owner_graph
            =
            owner_graph
            |>provide_cluster name
            |>how_to_fill
            owner_graph

        
        
        let save_to_file filename graph=
            let root = graph.root
            root.ComputeLayout()
            root.ToSvgFile(filename+".svg")
            root.ToDotFile(filename+".dot")
            root.FreeLayout()