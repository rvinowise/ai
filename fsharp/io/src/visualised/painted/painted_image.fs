namespace rvinowise.ai.ui.painted

    module image =
        open Rubjerg.Graphviz
        open rvinowise.ai
        open rvinowise.ai.ui
        open System.IO
        open System.Diagnostics

        

        
        let open_image_of_graph (root:RootGraph) =
            let filename = Directory.GetCurrentDirectory() + "/out"
            root.ComputeLayout()
            root.ToSvgFile(filename+".svg")
            root.ToDotFile(filename+".dot")
            root.FreeLayout()
            Process.Start("cmd", $"/c {filename}.svg") |> ignore
            ()


        let visualise_figure 
            (figure:Figure) 
            =
            figure.graph.id
            |>Graph.empty_root_graph
            |>Graph.provide_clustered_subgraph_inside_root_graph figure.graph.id 
                (Figure.painted_edges figure)
            |>open_image_of_graph
            |>ignore

        let visualise_singnal_history 
            (figure:Figure) 
            =
            "signal history"
            |>Graph.empty_root_graph
            |>Graph.provide_clustered_subgraph_inside_root_graph "signal history"
                (Signal_history.painted_edges figure)
            |>open_image_of_graph
            |>ignore