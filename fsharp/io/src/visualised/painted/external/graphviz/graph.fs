(* incapsulate infrastructure - a library for painting graphs *)
namespace rvinowise.ui.infrastructure

    open FsUnit
    open Xunit


    type private External_graph = Rubjerg.Graphviz.SubGraph
    type private External_root = Rubjerg.Graphviz.RootGraph
    type private External_node = Rubjerg.Graphviz.Node
    type private External_element = Rubjerg.Graphviz.CGraphThing
    

    type Element=
    |Vertex of External_node
    |Cluster of External_graph

    type Node={
        id:string
        parent:Node option

        id_impl:string
        element_impl:Element
        root_impl:External_root
    }

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Graph=
        open Rubjerg
        open System

        //type private Node = rvinowise.ui.infrastructure.Node

        let with_attribute key value (node:Node) =
            match node.element_impl with
            |Cluster g -> g.SafeSetAttribute(key,value,"")
            |Vertex v->v.SafeSetAttribute(key,value,"")
            node

        let fill_with_color color node =
            node
            |>with_attribute "fillcolor" color
            |>with_attribute "style" "filled"

        let empty name=
            let external_root:External_root = External_root.CreateNew(name, Graphviz.GraphType.Directed)
            external_root.SafeSetAttribute("rankdir", "LR", "")
            //external_root.SafeSetAttribute("cluster", "true", "")
            {
                Node.id=name
                Node.id_impl=name
                parent=None
                root_impl=external_root
                element_impl = Cluster (external_root.GetOrAddSubgraph(name))
            }
        
        let with_circle_vertices (node: Node)=
            External_node.IntroduceAttribute(node.root_impl, "shape", "circle")
            node

        let with_rectangle_vertices node=
            External_node.IntroduceAttribute(node.root_impl, "shape", "rectangle")
            node

        let private transform_vertex_into_graph node=
            match node.element_impl with
            |Vertex vertex -> 
                match node.parent with
                |Some parent -> 
                    let new_graph = 
                        match parent.element_impl with
                        |Cluster parent_graph ->
                            parent.root_impl.Delete(vertex)
                            let graph = 
                                parent_graph.GetOrAddSubgraph(node.id_impl)
                            graph.SafeSetAttribute("label", node.id, "")
                            graph.SafeSetAttribute("cluster", "true", "")
                            graph
                        |Vertex _ -> raise (ArgumentException("parent must be a graph"))
                    {
                        node with 
                            element_impl=Cluster new_graph
                    },new_graph
                |None -> raise (ArgumentException("root node shouldn't be turned into a graph"))
            |Cluster cluster -> node,cluster
            

        let provide_vertex
            id
            (owner_node:Node) 
            =
            let owner_node, owner_graph = 
                match owner_node.element_impl with
                |Cluster parent_graph -> owner_node, parent_graph
                |Vertex _->(transform_vertex_into_graph owner_node)
            
            let vertex_impl = 
                owner_graph.GetOrAddNode(owner_node.id_impl+id)
            vertex_impl.SafeSetAttribute("label",id,"")
            
            {
                Node.id = id
                Node.id_impl = owner_node.id_impl+id
                parent = Some owner_node
                root_impl = owner_node.root_impl
                element_impl = Vertex vertex_impl
            }


        let with_vertex 
            id
            (node:Node) 
            =
            let vertex = (provide_vertex id node)
            node


        let with_edge
            (tail:Node)
            (head:Node)
            (owner_node:Node) 
            =
            // owner_node.graph_impl.GetOrAddEdge(
            //     tail.vertex_impl, head.vertex_impl, ""
            // )|>ignore
            owner_node


        
        let save_to_file filename node=
            let root = node.root_impl
            root.ComputeLayout()
            root.ToSvgFile(filename+".svg")
            root.ToDotFile(filename+".dot")
            root.FreeLayout()
        

        