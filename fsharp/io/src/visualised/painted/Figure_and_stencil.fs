namespace rvinowise.ai.ui.painted

    open Rubjerg
    open rvinowise
    open rvinowise.ai
    open rvinowise.ui

    module Graph=
        open rvinowise

        let add_graph
            (added_graph: ai.Graph)
            (to_graph: infrastructure.Node)
            =
            added_graph.edges
            |> Seq.iter (
                fun edge -> 
                    let tail = 
                        to_graph
                        |>infrastructure.Graph.provide_vertex (added_graph.id+edge.tail)
                        |>infrastructure.Graph.with_attribute "label" edge.tail
                    
                    let head = 
                        to_graph
                        |>infrastructure.Graph.provide_vertex (added_graph.id+edge.head)
                        |>infrastructure.Graph.with_attribute "label" edge.head

                    to_graph
                    |>infrastructure.Graph.with_edge
                        tail head
                    |> ignore
            )


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure=


        let add_figure
            (figure: ai.Figure)
            (to_graph: infrastructure.Node)
            =
            figure.graph.edges
            |> Seq.iter (fun edge -> 
                let tail = 
                    to_graph
                    |>infrastructure.Graph.provide_vertex (figure.graph.id+edge.tail)
                    |>infrastructure.Graph.with_attribute "label" edge.tail
                
                let head = 
                    to_graph
                    |>infrastructure.Graph.provide_vertex (figure.graph.id+edge.head)
                    |>infrastructure.Graph.with_attribute "label" edge.head

                to_graph
                |>infrastructure.Graph.with_edge
                    tail head
                |> ignore
            )
            to_graph
    

