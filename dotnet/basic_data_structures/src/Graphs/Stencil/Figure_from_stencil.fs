namespace rvinowise.ai


module Figure_from_stencil=

    let edges_without_stencil_output stencil =
        
        let output_vertex =
            stencil
            |>Stencil.output

        let vertices_before_output =
            output_vertex
            |>Edges.previous_vertices stencil.edges
        
        let vertices_after_output =
            output_vertex
            |>Edges.next_vertices stencil.edges
        
        let edges_instead_of_output =
            (vertices_before_output,vertices_after_output)
            ||>Seq.allPairs
            |>Seq.map Edge.ofPair

        let edges_without_output = 
            stencil.edges
            |>Seq.filter(fun edge->
                (edge.head=output_vertex 
                || 
                edge.tail=output_vertex)
                |>not
            )

        edges_instead_of_output
        |>Seq.append edges_without_output
        |>Set.ofSeq


    let stencil_nodes_to_figure_subfigures 
        (nodes: Map<Vertex_id, Stencil_node>)
        =
        nodes
        |>Map.fold(fun map vertex node ->
                match node with
                |Stencil_output-> map
                |Lower_figure referenced_figure->
                    map
                    |>Map.add 
                        vertex
                        (built.Subfigure.referencing_constant_figure referenced_figure)

            )
            Map.empty
        

    let convert stencil =
        {
            Figure.edges=edges_without_stencil_output stencil
            subfigures=stencil_nodes_to_figure_subfigures stencil.nodes
        }