namespace rvinowise.ai

    open Xunit
    open FsUnit

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Graph=
        open rvinowise.ai
        open rvinowise
        open rvinowise.extensions

        let need_every_vertex vertex =
            true

        // let need_vertex_referencing_graph 
        //     referenced_graph
        //     (vertex: #Vertex) =
        //     vertex.referenced = referenced_graph

        let vertices_reacheble_from_other_vertices
            (is_needed: #Vertex->bool)
            (graph_in_which_search: Graph)
            (vertices_before_goals: #Vertex seq)
            =
            Edges.vertices_reacheble_from_other_vertices
                is_needed
                graph_in_which_search.edges
                vertices_before_goals
    
        let vertices_reaching_other_vertices
            (is_needed: #Vertex->bool)
            (graph_in_which_search: Graph)
            (vertices_after_goals: #Vertex seq)
            =
            Edges.vertices_reaching_other_vertices
                is_needed
                graph_in_which_search.edges
                vertices_after_goals

        

       

        let next_vertices (graph:Graph) vertex=
            Edges.next_vertices graph.edges vertex

        let previous_vertices (graph:Graph) vertex=
            Edges.previous_vertices graph.edges vertex

        let first_vertices (graph:Graph) =
            Edges.first_vertices graph.edges
            

        let vertex_occurances (vertex:Graph) (graph:Graph) =
            graph.edges
            |>Seq.collect (fun e->
                [e.tail; e.head]
            )
            |>Set.ofSeq

        let vertices (graph:Graph) =
            graph.edges
            |>Edges.all_vertices


        let vertices_with_ids ids (graph:Graph)=
            ids
            |>Edges.vertices_with_ids graph.edges


        let private edges_between_vertices 
            (edges:seq<#ai.Edge>)
            (vertices:Set<#Vertex>)
            =
            (edges:seq<#ai.Edge>)
            |>Seq.filter (fun (edge:#ai.Edge)->
                Set.contains edge.tail vertices
                &&
                Set.contains edge.head vertices
            )

        let subgraph_with_vertices 
            (target:Graph) 
            (vertices:Set<#Vertex>)
            =
            vertices
            |>edges_between_vertices target.edges
            |>stencil_output

