(* incapsulate infrastructure - a library for painting graphs *)
namespace rvinowise.ui.infrastructure

    open Rubjerg
    open Rubjerg.Graphviz


    type private External_graph = Rubjerg.Graphviz.Graph
    type private External_root = Rubjerg.Graphviz.RootGraph
    type private External_node = Rubjerg.Graphviz.Node
    type private External_element = Rubjerg.Graphviz.CGraphThing
    
    type Graph = {
        id:string
        parent:Graph option
        root:External_root
        impl:External_graph
    }
    type Vertex = {
        id:string
        parent:Graph
        root:External_root
        impl:External_node
    }
    type Node=
    |Graph of Graph
    |Vertex of Vertex

 

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Node =
        let impl (node:Node)=
            match node with
            |Graph graph -> graph.impl :> External_element
            |Vertex vertex -> vertex.impl :> External_element

        

        let with_attribute key value (node:Node) =
            (impl node).SafeSetAttribute(key,value,"")
            node

        let fill_with_color color node =
            node
            |>with_attribute "fillcolor" color
            |>with_attribute "style" "filled"

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
        open System.Diagnostics.Contracts
        open System

        let empty name=
            let root:External_root = External_root.CreateNew(name, GraphType.Directed)
            root.SafeSetAttribute("rankdir", "LR", "")
            Graph {
                Graph.id=name
                parent=None
                root=root
                impl = root
            }
        
        let with_circle_vertices (node: Node)=
            match node with
            |Graph graph->
                External_node.IntroduceAttribute(graph.root, "shape", "circle")
            |Node ->
            node
        
        let with_rectangle_vertices graph=
            External_node.IntroduceAttribute(graph.root, "shape", "rectangle")
            graph

        let with_attribute key value (graph:Graph) =
            graph.impl.SafeSetAttribute(key,value,"")
            graph


        let private provide_cluster_inside_graph
            name
            (owner_graph:Graph)
            =
            let cluster_id = "cluster_"+owner_graph.id+name
            {
                Graph.id = cluster_id
                parent = Some owner_graph
                root = owner_graph.root
                impl = owner_graph.impl.GetOrAddSubgraph(cluster_id); //graphviz needs clusters to have word "cluster" in their name
            }
            |> with_attribute "label" name

        
        let private transform_vertex_into_cluster (vertex:Vertex) =
            let owner_graph = vertex.parent
            owner_graph.impl.Delete(vertex.impl)
            owner_graph|>provide_cluster_inside_graph vertex.id 


        let private provide_vertex
            id
            (owner_node:Node) 
            =
            let owner_graph = 
                match owner_node with
                |Graph graph -> 
                    graph
                |Vertex vertex ->
                    transform_vertex_into_cluster vertex
            {
                Vertex.id = id
                parent = owner_graph
                root = owner_graph.root
                impl = owner_graph.impl.GetOrAddNode(owner_graph.id+id)
            }


        let get_vertex
            id
            (owner_node:Node) 
            =
            owner_node
            |>provide_vertex id
            |>Node.Vertex
            |>Node.with_attribute "label" id

        let with_vertex 
            id
            (node:Node) 
            =
            let vertex = (provide_vertex id node)
            Node.Graph vertex.parent


        let with_edge
            tail
            head
            (graph:Graph) 
            =
            graph.impl.GetOrAddEdge(
                tail.impl, head.impl, ""
            )|>ignore
            graph


        

        
        let provide_node
            name
            (owner_graph:Graph)
            =
            let node_id = "cluster_"+owner_graph.id+name
            {
                id = node_id
                parent = owner_graph
                root = owner_graph.root
                impl = owner_graph.impl.GetOrAddSubgraph(node_id); //graphviz needs clusters to have word "cluster" in their name
            }
            |> with_attribute "label" name

        
        let save_to_file filename graph=
            let root = graph.root
            root.ComputeLayout()
            root.ToSvgFile(filename+".svg")
            root.ToDotFile(filename+".dot")
            root.FreeLayout()