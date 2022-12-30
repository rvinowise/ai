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


        let visualise_figure 
            (figure:Figure) 
            =
            figure.graph.id
            |>infrastructure.Graph.empty
            |>infrastructure.Graph.with_circle_vertices
            |>infrastructure.Graph.with_cluster figure.graph.id 
                (painted.Graph.add_graph figure.graph)
            |>open_image_of_graph
            |>ignore

        let visualise_history 
            (history:rvinowise.ai.History) 
            =
            history
            |>painted.History.description
            |>infrastructure.Graph.empty
            |>infrastructure.Graph.with_rectangle_vertices
            |>painted.History.add_history history
            //|>infrastructure.Graph.with_cluster "signal history" ()
            |>open_image_of_graph
            |>ignore
        
        [<Fact>]
        let ``visualise history``()=
            ai.history.example.short_history_with_some_repetitions
            |>visualise_history