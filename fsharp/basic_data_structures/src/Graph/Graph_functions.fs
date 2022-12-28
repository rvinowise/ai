namespace rvinowise.ai

    open Xunit
    open FsUnit

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Graph=
        open rvinowise
        open rvinowise.extensions

        
        let need_every_vertex _ =
            true

        

        let vertices_reacheble_from_other_vertices
            (is_needed: Vertex_id->bool)
            (graph_in_which_search: Graph)
            (vertices_before_goals: Vertex_id seq)
            =
            Edges.vertices_reacheble_from_other_vertices
                is_needed
                graph_in_which_search.edges
                vertices_before_goals
    
        let vertices_reaching_other_vertices
            (is_needed: Vertex_id->bool)
            (graph_in_which_search: Graph)
            (vertices_after_goals: Vertex_id seq)
            =
            Edges.vertices_reaching_other_vertices
                is_needed
                graph_in_which_search.edges
                vertices_after_goals

        

        let next_vertices graph vertex=
            Edges.next_vertices graph.edges vertex

        let previous_vertices graph vertex=
            Edges.previous_vertices graph.edges vertex

        let first_vertices (graph:Graph) =
            Edges.first_vertices graph.edges