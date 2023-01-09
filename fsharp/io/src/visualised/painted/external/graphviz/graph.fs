(* incapsulate infrastructure - a library for painting graphs *)
namespace rvinowise.ui.infrastructure

    open FsUnit
    open Xunit


    type private External_subgraph = Rubjerg.Graphviz.SubGraph
    type private External_root = Rubjerg.Graphviz.RootGraph
    type private External_graph = Rubjerg.Graphviz.Graph
    type private External_node = Rubjerg.Graphviz.Node
    type private External_element = Rubjerg.Graphviz.CGraphThing
    type private External_edge = Rubjerg.Graphviz.Edge
    
    type Node_id = string

    type Cluster = {
        impl: External_graph
        children: Node_id seq
    }

    type Element=
    |Vertex of External_node
    |Cluster of External_graph

    type Node_data={
        id:Node_id
        parent:Node_data option
        mutable children: Node_data seq

        id_impl:string
        mutable impl:Element
        mutable child_node_attr:Map<string, string>
    }

    type Graph={
        mutable nodes: Node_data seq
        root_node: Node_data
        root_impl: External_root
    }

    type Node={
        graph: Graph
        data: Node_data
    }
    type Edge={
        graph: Graph
        impl: External_edge
    }

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Node=
        let id (node:Node)=
            node.data.id

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edge=
        let with_attribute key value (edge:Edge) =
            edge.impl.SafeSetAttribute(key,value,"")
            edge

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Graph=
        open Rubjerg
        open System
        open System.Diagnostics.Contracts

        //type private Node = rvinowise.ui.infrastructure.Node

        let with_attribute key value (node:Node) =
            match node.data.impl with
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
            external_root.SafeSetAttribute("compound", "true", "") //for edges between clusters
            external_root.SafeSetAttribute("newrank", "true", "") //for different rankdir directions
            let root_node: Node_data={
                id = name
                id_impl = name
                children=[]
                parent=None
                impl=Cluster external_root
                child_node_attr=Map.empty
            }
            {
                Node.graph={
                    Graph.nodes=[]
                    root_impl=external_root
                    root_node = root_node
                }
                data=root_node
            }
        
        let root_node graph =
            graph.root_node

        let private transform_vertex_into_graph 
            (graph_node:Node) 
            =
            match graph_node.data.impl with
            |Vertex vertex -> 
                match graph_node.data.parent with
                |Some parent -> 
                    let new_cluster_impl = 
                        match parent.impl with
                        |Cluster parent_cluster ->
                            graph_node.graph.root_impl.Delete(vertex)
                            let cluster = 
                                parent_cluster.GetOrAddSubgraph(graph_node.data.id_impl)
                            cluster.SafeSetAttribute("label", graph_node.data.id, "")
                            cluster.SafeSetAttribute("cluster", "true", "")
                            cluster:>External_graph
                        |Vertex _ -> raise (ArgumentException("parent must be a graph"))
                    
                    graph_node.data.impl <- Cluster new_cluster_impl
                    new_cluster_impl
                |None -> raise (ArgumentException("root node shouldn't be turned into a graph"))
            |Cluster cluster_impl -> cluster_impl
            
        let with_circle_vertices 
            graph_node
            =
            let graph_impl = (transform_vertex_into_graph graph_node)
            graph_node.data.child_node_attr <- Map.add "shape" "circle" graph_node.data.child_node_attr

            graph_node

        let with_rectangle_vertices graph_node =
            let  graph_impl = (transform_vertex_into_graph graph_node)
            graph_node.data.child_node_attr <- Map.add "shape" "rectangle" graph_node.data.child_node_attr
            graph_node

        let with_horisontal_children graph_node=
            graph_node
            |>with_attribute "rankdir" "LR"
        
        let with_vertical_children graph_node=
            graph_node
            |>with_attribute "rankdir" "TB"
        
        let with_perpendicular_children graph_node =
            graph_node
            |>with_attribute "rank" "same"

        let write_attributes_to_node 
            (attr:Map<string, string>) 
            (node_impl:External_node)
            =
            attr
            |>Seq.iter (fun pair->
                node_impl.SafeSetAttribute(pair.Key, pair.Value,"")
            )


        let provide_vertex
            (id:Node_id)
            (owner:Node)
            =
            let owner_cluster = 
                match owner.data.impl with
                |Cluster parent_cluster -> parent_cluster
                |Vertex _->(transform_vertex_into_graph owner)
            
            let vertex_impl = 
                owner_cluster.GetOrAddNode(owner.data.id_impl+id)
            vertex_impl.SafeSetAttribute("label",id,"")
            vertex_impl|>write_attributes_to_node owner.data.child_node_attr

            let new_vertex = {
                id = id
                id_impl = owner.data.id_impl+id
                parent = Some owner.data
                children=[]
                impl = Vertex vertex_impl
                child_node_attr = owner.data.child_node_attr
            }
            owner.data.children <- owner.data.children|>Seq.append [new_vertex]
            {owner with data=new_vertex}


        let with_vertex 
            id
            target
            =
            let vertex = (provide_vertex id target)
            target

        let with_filled_vertex 
            id
            (fill_vertex:Node->unit)
            target
            =
            let vertex = provide_vertex id target
            fill_vertex vertex
            target

        // let is_vertex_impl node =
        //     match node.impl with
        //     |Cluster _ -> false
        //     |Vertex _ -> true

       
        let rec private lowest_child_vertex (node:Node_data)=
            match node.impl with
            |Element.Cluster cluster_impl -> 
                match node.children|>Seq.tryHead with
                |Some child -> (lowest_child_vertex child)
                |None->raise (ArgumentException("it's complicated to analyse unfinished clusters without child vertices"))
            |Element.Vertex vertex_impl -> node, vertex_impl

        [<Fact>]
        let ``lowest child vertex``()=
            
            let outer_cluster = 
                "my graph"
                |>empty
                |>provide_vertex "outer1"
            
            let inner_vertex =
                outer_cluster
                |>provide_vertex "outer2"
                |>provide_vertex "vertex"

            outer_cluster.data
            |>lowest_child_vertex
            |>fst
            |>fun n->n.id
            |>should equal inner_vertex.data.id

        

        let provide_edge
            (head:Node)
            (tail:Node)
            =
            Contract.Requires(head.graph = tail.graph)
            let vertex_tail, tail_impl = lowest_child_vertex tail.data
            let vertex_head, head_impl = lowest_child_vertex head.data
            let edge_impl = head.graph.root_impl.GetOrAddEdge(
                tail_impl, head_impl, $"{tail.data.id_impl}->{head.data.id_impl}"
            )
            if (tail.data = vertex_tail) then () else
                edge_impl.SafeSetAttribute("ltail",tail.data.id_impl,"")
            if (head.data = vertex_head) then () else
                edge_impl.SafeSetAttribute("lhead",head.data.id_impl,"")
            {
                Edge.graph = tail.graph;
                impl=edge_impl
            }


        let with_edge
            (head:Node)
            (tail:Node)
            =
            tail|>provide_edge head
            tail


        let save_to_file 
            filename 
            graph
            =
            let root = graph.root_impl
            root.ComputeLayout()
            root.ToSvgFile(filename+".svg")
            root.ToDotFile(filename+".dot")
            root.FreeLayout()
        

        