namespace rvinowise.ai.ui.painted

    open rvinowise
    open rvinowise.ai
    open rvinowise.ui


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure=


        let add_figure
            (figure: ai.Figure)
            (to_node: infrastructure.Node)
            =
            figure.edges
            |> Seq.iter (fun edge -> 
                let tail =
                    to_node
                    |>infrastructure.Graph.provide_vertex (edge.tail)
                
                let head = 
                    to_node
                    |>infrastructure.Graph.provide_vertex (edge.head)

                tail
                |>infrastructure.Graph.with_edge head
                |> ignore
            )
            to_node
    

