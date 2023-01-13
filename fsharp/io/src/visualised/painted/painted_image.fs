namespace rvinowise.ai.ui.painted
    open System
    open Xunit
    open FsUnit

    module image =
        open rvinowise.ai
        open rvinowise
        open rvinowise.ui.infrastructure
        open rvinowise.ui
        open System.IO
        open System.Diagnostics

        
        exception BadGraphvizFile of string
        
        let open_image_of_graph (graph_node:infrastructure.Node) =
            let filename = Directory.GetCurrentDirectory() + $"/{graph_node.data.id}"
            let dot_file =
                graph_node.graph
                |>infrastructure.Graph.save_to_file filename
            let root =
                try 
                    dot_file
                    |>Rubjerg.Graphviz.RootGraph.FromDotFile
                with
                    | _ -> raise (BadGraphvizFile "bad .dot file was generated")
            root.ComputeLayout()       
            root.ToSvgFile($"{filename}.svg")
            root.FreeLayout()
            Process.Start("cmd", $"/c \"{filename}.svg\"") |> ignore
            ()


        
        
        [<Fact>]//(Skip="ui")
        let ``visualise history``()=
            (
                [
                    ai.figure_history.example.short_history_with_some_repetitions;
                    ai.figure_history.example.another_history_for_combining_togetner
                ],
                mood_history.example.short_history_of_mood_changes
            )
            ||>ai.combined_history.built.from_figure_and_mood_histories
            |>History.as_graph
            |>open_image_of_graph
        

        [<Fact>]//(Skip="ui")
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
        
  