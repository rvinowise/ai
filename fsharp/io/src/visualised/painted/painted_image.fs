namespace rvinowise.ai.ui.painted
    open Xunit
    open FsUnit

    module image =
        open Rubjerg.Graphviz
        open rvinowise.ai
        open rvinowise
        open rvinowise.ai.ui
        open rvinowise.ui
        open System.IO
        open System.Diagnostics

        

        
        let open_image_of_graph (graph:infrastructure.Graph) =
            let filename = Directory.GetCurrentDirectory() + "/out"
            graph|>infrastructure.Graph.save_to_file filename
            Process.Start("cmd", $"/c {filename}.svg") |> ignore
            ()


        [<Fact>]
        let ``visualise history``()=
            [ai.history.example.short_history_with_some_repetitions]
            |>History.as_graph
            |>open_image_of_graph