module rvinowise.ai.Renaming_figures
    
    open Xunit
    open FsUnit
    
    open rvinowise.ai
    open rvinowise.extensions
    open rvinowise
    open System.Collections.Generic


    
    let vertices_with_sequencial_names
        (referenced_figure: Figure_id)
        (amount:int)
        =
        List.init amount (fun number->
            (
                (Figure_id.value referenced_figure)
                +
                (string (number+1))
            )
            |>Vertex_id
        )

    let renamed_edges_for_figure
        (old_to_new_names: Map<Vertex_id, Vertex_id>)
        (edges: Edge seq)
        =
        edges
        |>Seq.map (fun edge->
            Edge(
                old_to_new_names[edge.tail], 
                old_to_new_names[edge.head]
            )
        )

    let renamed_subfigures_for_figure 
        (old_to_new_names: Map<Vertex_id, Vertex_id>)
        (subfigures: IDictionary<Vertex_id, Figure_id>)
        =
        subfigures
        |>Seq.map(fun pair->
            old_to_new_names[pair.Key],
            pair.Value
        )|>Map.ofSeq

    let rename_vertices_to_standard_names 
        (owner_figure:Figure)
        =
        let figure_to_vertices = 
            owner_figure.subfigures
            |>extensions.Map.reverse_with_list_of_keys

        let vertices_to_new_names = 
            figure_to_vertices
            |>Seq.collect (fun pair ->
                let referenced_figure = pair.Key
                let vertices_to_this_figure = pair.Value
                let renamed_vertices =
                    vertices_with_sequencial_names
                        referenced_figure
                        (Seq.length vertices_to_this_figure)
                
                renamed_vertices
                |>Seq.zip vertices_to_this_figure
            )|>Map.ofSeq

        {
            edges=
                owner_figure.edges
                |>renamed_edges_for_figure vertices_to_new_names
                |>Set.ofSeq
            
            subfigures=
                owner_figure.subfigures
                |>renamed_subfigures_for_figure vertices_to_new_names
        }

    [<Fact>]
    let ``try rename_vertices_to_standard_names``()=
        built.Figure.from_tuples [
            "my_a0","a","my_b1","b";
            "my_a0","a","uppercase_b","B";
            "uppercase_b","B","c0_at_the_end","figure_c";
            "uppercase_b","B","another_a","a";
        ]|>rename_vertices_to_standard_names
        |>should equal (
            built.Figure.from_tuples [
                "a1","a","b1","b";
                "a1","a","B1","B";
                "B1","B","figure_c1","figure_c";
                "B1","B","a2","a";
            ]
        )