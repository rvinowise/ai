namespace rvinowise.ai.ui.painted

open rvinowise
open rvinowise.ai
open rvinowise.ui

module Graph=

    let add_graph
        (edges: ai.Edge seq)
        (to_node: infrastructure.Node)
        =
        edges
        |> Seq.iter (
            fun edge -> 
                let tail = 
                    to_node
                    |>infrastructure.Graph.provide_vertex (edge.tail|>Vertex_id.value)
                
                let head = 
                    to_node
                    |>infrastructure.Graph.provide_vertex (edge.head|>Vertex_id.value)

                tail
                |>infrastructure.Graph.with_edge
                     head
                |> ignore
        )

