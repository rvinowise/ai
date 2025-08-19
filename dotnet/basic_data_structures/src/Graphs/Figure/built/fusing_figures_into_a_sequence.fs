namespace rvinowise.ai.built

open Xunit
open FsUnit

open rvinowise.ai
open rvinowise

module Fusing_figures_into_sequence=
    
    let private rename_duplicating_vertices
        (a_figure: Constant_figure)
        (b_figure: Constant_figure)
        =
        b_figure.targets
        |>Seq.choose (fun pair->
            let b_vertex = pair.Key
            if 
                a_figure.targets|>Map.containsKey b_vertex
            then
                Some (b_vertex, b_vertex+Vertex_id "'")
            else
                None

        )
        |>Map.ofSeq
    
    let private vertices_referencing_figure 
        referenced_figure 
        all_vertices
        =
        all_vertices
        |>Map.tryFind referenced_figure
        |>function
        |None->[]
        |Some vertices ->vertices

    let assign_numbers_to_vertices_of_one_figure //todo use simpler vertices_with_sequencial_names
        (referenced_figure)
        starting_number
        vertices
        =
        let renamed =
            vertices
            |>Seq.fold(
                fun 
                    (renamed_vertices, last_number) _
                    ->
                let new_vertex =
                    referenced_figure
                    +
                    (string last_number)
                    |>Vertex_id
                (
                    new_vertex::renamed_vertices
                    ,
                    last_number+1
                )
            ) ([],starting_number)
        
        renamed
        |>fst
        |>List.rev
        |>Seq.zip (vertices)
        |>Map.ofSeq
        ,
        (snd renamed)

    type Renamed_subfigures = 
        Map<Constant_figure_id, //figure, referenced by renamed vertices 
                list< //every element = owner figure which has the renamed verticex
                    Map<
                        Vertex_id, //old name of a vertex
                        Vertex_id> //new name of a vertex
                    >
            >

    let private renamed_vertices_for_fusing_figures
        figure_id_to_name
        (a_figure: Constant_figure)
        (b_figure: Constant_figure)
        :Renamed_subfigures
        =

        let a_vertices = 
            a_figure.targets
            |>extensions.Map.reverse_with_list_of_keys
        let b_vertices = 
            b_figure.targets
            |>extensions.Map.reverse_with_list_of_keys
        
        let all_referenced_figures =
            a_figure.targets
            |>Map.values
            |>Seq.append (
                b_figure.targets
                |>Map.values
            )
            |>Set.ofSeq
        
        all_referenced_figures
        |>Seq.map (fun target_id -> target_id, figure_id_to_name target_id)
        |>Seq.map (fun (referenced_figure, name) ->
            
            let renamed_a_vertices, last_number =
                a_vertices
                |>vertices_referencing_figure referenced_figure
                |>assign_numbers_to_vertices_of_one_figure
                    name 1
            let renamed_b_vertices, _ =
                b_vertices
                |>vertices_referencing_figure referenced_figure
                |>assign_numbers_to_vertices_of_one_figure
                    name last_number
            (
                referenced_figure, 
                [
                    renamed_a_vertices;
                    renamed_b_vertices
                ]
            )
        )|>Map.ofSeq
    
    let all_renamed_subfigures
        (renamed_subfigures: Renamed_subfigures)
        =
        renamed_subfigures
        |>Seq.collect (fun pair->
            let referenced_figure = pair.Key
            let vertex_names_of_owner_figures = pair.Value
            vertex_names_of_owner_figures
            |>Seq.collect(fun vertex_names->
                vertex_names
                |>Seq.map(fun pair->
                    (pair.Value, referenced_figure) 
                )
            )
        )
        |>Map.ofSeq

    let private map_from_old_to_new_vertex_names
        (renamed_subfigures: Renamed_subfigures)
        figure_index
        =
        renamed_subfigures
        |>Seq.collect (fun pair->
            let referenced_figure = pair.Key
            let owner_figures = pair.Value
            owner_figures[figure_index]
            |>extensions.Map.toPairs
        )
        |>Map.ofSeq

    

    let sequential_pair 
        (a_figure: Constant_figure)
        (b_figure: Constant_figure)
        =
        let renamed_subfigures =
            renamed_vertices_for_fusing_figures
                Figure_registry.id_into_name
                a_figure b_figure

        let old_to_new_vertices_of_a = 
            map_from_old_to_new_vertex_names
                renamed_subfigures
                0
        let old_to_new_vertices_of_b = 
            map_from_old_to_new_vertex_names
                renamed_subfigures
                1

        let renamed_a_edges =
            a_figure.edges
            |>Renaming_figures.renamed_edges_for_figure old_to_new_vertices_of_a
        
        let renamed_b_edges =
            b_figure.edges
            |>Renaming_figures.renamed_edges_for_figure old_to_new_vertices_of_b
        
        let renamed_a_subfigures =
            a_figure.targets
            |>Renaming_figures.renamed_subfigures_for_figure old_to_new_vertices_of_a
        
        let renamed_b_subfigures =
            b_figure.targets
            |>Renaming_figures.renamed_subfigures_for_figure old_to_new_vertices_of_b

        let edges_inbetween =
            let last_vertices_of_a =
                {
                    edges=renamed_a_edges|>Set.ofSeq
                    targets=renamed_a_subfigures
                }
                |>Figure.last_vertices
            let first_vertices_of_b =
                {
                    edges=renamed_b_edges|>Set.ofSeq
                    targets=renamed_b_subfigures
                }
                |>Figure.first_vertices
            Seq.allPairs last_vertices_of_a first_vertices_of_b 
            |>Seq.map Edge.ofPair

        {
            edges=
                renamed_a_edges
                |>Seq.append renamed_b_edges
                |>Seq.append edges_inbetween
                |>Set.ofSeq
            
            targets=
                renamed_subfigures
                |>all_renamed_subfigures

        }

    [<Fact>]
    let ``try sequential_pair``()=
        let a_figure = built.Figure.simple_without_separator ["a1","b1";"b1","a2";"b1","c1"]
        let b_figure = built.Figure.simple_without_separator ["a1","b1";"e1","b1";"b1","a2"]
        let expected_ab_figure = {
            edges=[
                "a1","b1"; "b1","a2"; "b1","c1";
                "a3","b2"; "e1","b2"; "b2","a4";
                //edges between glued graphs:
                "a2","a3";
                "a2","e1";
                "c1","a3";
                "c1","e1"

            ]
            |>Seq.map Edge.ofStringPair
            |>Set.ofSeq
            
            targets=[
                "a1","a";
                "a3","a";
                "a2","a";
                "a4","a";
                "b1","b";
                "b2","b";
                "c1","c";
                "e1","e";
            ]
            |>Seq.map (fun pair->
                pair|>fst|>Vertex_id,
                pair|>snd|>Figure_registry.provide_signal
            )
            |>Map.ofSeq
        }
        let real_ab_figure = 
            sequential_pair
                a_figure
                b_figure
        real_ab_figure.edges
        |>Seq.sort
        |>should equal expected_ab_figure.edges

        real_ab_figure.targets
        |>should equal expected_ab_figure.targets

    [<Fact>]
    let ``sequential_pair with the same figure generates different subfigure ids``()=
        let figure =
            built.Figure.signal
                Figure_registry.provide_signal
                Figure_registry.id_into_name
                "a"
                
        let pair = sequential_pair figure figure
        pair.targets
        |>Map.keys
        |>Seq.distinct
        |>List.ofSeq
        |>should haveLength 2
        