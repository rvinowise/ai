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

        

        
        let open_image_of_graph (graph:infrastructure.Graph) =
            let filename = Directory.GetCurrentDirectory() + "/out"
            graph|>infrastructure.Graph.save_to_file filename
            Process.Start("cmd", $"/c {filename}.svg") |> ignore
            ()


        // [<Fact(Skip="ui")>]
        // let ``visualise history``()=
        //     [ai.history.example.short_history_with_some_repetitions]
        //     |>History.as_graph
        //     |>open_image_of_graph
        
        [<Fact>]
        let ``construct a graph``()=
            let graph =
                "my graph"
                |>Graph.empty
                
            let tail=  
                graph.root_node
                |>Graph.provide_vertex graph "outer_circles"
                |>Graph.with_circle_vertices graph
                |>Graph.with_vertex graph "b"
                |>Graph.with_vertex graph "c"
            
            let head =
                graph.root_node
                |>Graph.provide_vertex graph "outer2"
                |>Graph.with_vertex graph "d"
                |>Graph.with_vertex graph "e"
            
            tail
            |>Graph.with_edge graph head
            |>ignore

            graph
            |>open_image_of_graph