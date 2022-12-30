namespace rvinowise.ai.ui.painted

    module image =
        open Rubjerg.Graphviz
        open rvinowise.ai
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
            "signal history"
            |>infrastructure.Graph.empty
            |>infrastructure.Graph.with_circle_vertices
            //|>infrastructure.Graph.with_cluster "signal history" ()
            |>open_image_of_graph
            |>ignore