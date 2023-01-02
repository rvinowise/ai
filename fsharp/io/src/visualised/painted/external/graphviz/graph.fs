(* incapsulate infrastructure - a library for painting graphs *)
namespace rvinowise.ui.infrastructure

    open FsUnit
    open Xunit


    type private External_subgraph = Rubjerg.Graphviz.SubGraph
    type private External_root = Rubjerg.Graphviz.RootGraph
    type private External_node = Rubjerg.Graphviz.Node
    type private External_element = Rubjerg.Graphviz.CGraphThing
    
    type Node_id = string

    type Cluster = {
        impl: External_subgraph
        children: Node_id seq
    }

    type Element=
    |Vertex of External_node
    |Cluster of External_subgraph

    type Node={
        id:Node_id
        parent:Node option
        children: Node seq

        id_impl:string
        impl:Element
    }

    type Graph={
        nodes: Node seq
        root_node: Node
        root_impl: External_root
    }

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Graph=
        open Rubjerg
        open System

        //type private Node = rvinowise.ui.infrastructure.Node

        let with_attribute key value (node:Node) =
            match node.impl with
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
                Graph.nodes=[]
                root_impl=external_root
                root={
                    Node.id = name
                    id_impl = name
                    children=[]
                    parent=None
                    impl=Vertex (external_root.GetOrAddNode(name))
                }
            },
        
        

        let private transform_vertex_into_graph 
            (graph:Graph) 
            node
            =
            match node.impl with
            |Vertex vertex -> 
                match node.parent with
                |Some parent -> 
                    let new_cluster_impl = 
                        match parent.impl with
                        |Cluster parent_cluster ->
                            graph.root_impl.Delete(vertex)
                            let cluster = 
                                parent_cluster.GetOrAddSubgraph(node.id_impl)
                            cluster.SafeSetAttribute("label", node.id, "")
                            cluster.SafeSetAttribute("cluster", "true", "")
                            cluster
                        |Vertex _ -> raise (ArgumentException("parent must be a graph"))
                    {
                        node with 
                            impl=Cluster new_cluster_impl
                    },new_cluster_impl
                |None -> raise (ArgumentException("root node shouldn't be turned into a graph"))
            |Cluster cluster_impl -> node,cluster_impl
            
        let with_circle_vertices 
            graph
            (node: Node)
            =
            let node, graph_impl = (transform_vertex_into_graph graph node)
            graph_impl.SafeSetAttribute("shape", "circle","")
            node

        let with_rectangle_vertices graph node=
            //External_node.IntroduceAttribute(node.root_impl, "shape", "rectangle")
            let node, graph_impl = (transform_vertex_into_graph graph node)
            graph_impl.SafeSetAttribute("shape", "circle","")
            node

        let provide_vertex
            (owner_graph:Graph)
            (id:Node_id)
            (owner_node:Node) 
            =
            let owner_node, owner_cluster = 
                match owner_node.impl with
                |Cluster parent_cluster -> owner_node, parent_cluster
                |Vertex _->(transform_vertex_into_graph owner_graph owner_node)
            
            let vertex_impl = 
                owner_cluster.GetOrAddNode(owner_node.id_impl+id)
            vertex_impl.SafeSetAttribute("label",id,"")
            
            {
                Node.id = id
                Node.id_impl = owner_node.id_impl+id
                parent = Some owner_node
                children=[]
                impl = Vertex vertex_impl
            }


        let with_vertex 
            graph
            id
            (node:Node) 
            =
            let vertex = (provide_vertex graph id node)
            node

        let is_vertex_impl node =
            match node.impl with
            |Cluster _ -> false
            |Vertex _ -> true

        // let rec private first_vertex_up_hierarchy node = 
        //     match node.parent with
        //     |Element.Cluster cluster_impl -> 
        //         match node.children|>Seq.tryHead with
        //         |Some child -> (first_vertex_up_hierarchy child)
        //         |None->node
        //     |Element.Vertex vertex_impl -> node, vertex_impl
            
        let rec private lowest_child_vertex node=
            match node.impl with
            |Element.Cluster cluster_impl -> 
                match node.children|>Seq.tryHead with
                |Some child -> (lowest_child_vertex child)
                |None->raise (ArgumentException("it's complicated to analyse unfinished clusters without child vertices"))
            |Element.Vertex vertex_impl -> node, vertex_impl

        let with_edge
            (owner_graph:Graph)
            (head:Node)
            (tail:Node)
            =
            let vertex_tail, tail_impl = lowest_child_vertex tail
            let vertex_head, head_impl = lowest_child_vertex head
            owner_graph.root_impl.GetOrAddEdge(
                tail_impl, head_impl, ""
            )|>ignore
            tail


        
        let save_to_file filename graph=
            let root = graph.root_impl
            root.ComputeLayout()
            root.ToSvgFile(filename+".svg")
            root.ToDotFile(filename+".dot")
            root.FreeLayout()
        

        