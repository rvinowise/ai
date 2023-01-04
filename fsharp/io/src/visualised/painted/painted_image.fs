namespace rvinowise.ai.ui.painted
    open Xunit
    open FsUnit

    module image =
        open rvinowise.ai
        open rvinowise
        open rvinowise.ui.infrastructure
        open rvinowise.ui
        open System.IO
        open System.Diagnostics

        

        
        let open_image_of_graph (graph_node:infrastructure.Node) =
            let filename = Directory.GetCurrentDirectory() + "/out"
            graph_node.graph|>infrastructure.Graph.save_to_file filename
            Process.Start("cmd", $"/c {filename}.svg") |> ignore
            ()


        [<Fact>]//(Skip="ui")
        let ``visualise history``()=
            [ai.history.example.short_history_with_some_repetitions]
            |>History.as_graph
            |>open_image_of_graph
        
        [<Fact>]
        let ``construct a graph``()=
            let root_node =
                "my graph"
                |>Graph.empty
                
            let tail=  
                root_node
                |>Graph.provide_vertex "outer_circles"
                |>Graph.with_circle_vertices
                |>Graph.with_vertex "b"
                |>Graph.with_vertex "c"
            
            let head =
                root_node
                |>Graph.provide_vertex "outer2"
                |>Graph.with_vertex "d"
                |>Graph.with_vertex "e"
            
            tail
            |>Graph.with_edge head
            |>ignore

            root_node
            |>open_image_of_graph