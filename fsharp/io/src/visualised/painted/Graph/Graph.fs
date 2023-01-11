namespace rvinowise.ai.ui.painted

    open rvinowise
    open rvinowise.ai
    open rvinowise.ui

    module Graph=
        open rvinowise

        let add_graph
            (added_graph: ai.Graph)
            (to_node: infrastructure.Node)
            =
            added_graph.edges
            |> Seq.iter (
                fun edge -> 
                    let tail = 
                        to_node
                        |>infrastructure.Graph.provide_vertex edge.tail
                    
                    let head = 
                        to_node
                        |>infrastructure.Graph.provide_vertex edge.head

                    tail
                    |>infrastructure.Graph.with_edge
                         head
                    |> ignore
            )

