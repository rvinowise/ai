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


        [<Fact(Skip="ui")>]
        let ``visualise history``()=
            [ai.history.example.short_history_with_some_repetitions]
            |>History.as_graph
            |>open_image_of_graph
        
        [<Fact>]
        let ``construct a graph``()=
            let graph, root_vertex =
                "my graph"
                |>Graph.empty
                
                root_vertex
                |>Graph.provide_vertex graph "vertex"
                |>Graph.with_circle_vertices graph
            
            let tail=  
                root_vertex              
                |>Graph.provide_vertex graph "outer"
                |>Graph.with_vertex graph "b"
                |>Graph.with_vertex graph "c"
            
            let head =
                root_vertex
                |>Graph.provide_vertex graph "outer2"
                |>Graph.with_vertex graph "d"
                |>Graph.with_vertex graph "e"
            
            tail
            |>Graph.with_edge graph head

            tail
            |>open_image_of_graph