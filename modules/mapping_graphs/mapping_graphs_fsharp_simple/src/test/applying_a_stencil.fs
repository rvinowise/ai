namespace rvinowise.ai.test

open Xunit
open FsUnit

open rvinowise
open rvinowise.ai
open rvinowise.ai.Applying_stencil
open rvinowise.ai.Mapping_graph_with_immutable_mapping
open rvinowise.ai.ui
open rvinowise.ai.stencil
open rvinowise.ui
        
        
module ``application of stencils``=
    
    let figure_name_to_id = Figure_registry.provide_signal
    let figure_id_to_name = Figure_registry.id_into_name
        
    let initial_mapping_without_prolongation = Immutable_mapping.ofStringPairs ["b#1","b#3";"h#1","h#1"]
    let initial_fruitful_mapping = Immutable_mapping.ofStringPairs ["b#1","b#1";"h#1","h#1"]
    let initial_useless_mapping = Immutable_mapping.ofStringPairs ["b#1","b#2";"h#1","h#1"]
        
    let result_of_fruitful_stencil_application =
        built.Figure.simple_without_separator
            [
                "d","e"
            ]
              

    [<Fact>]
    let ``impossible to prolongate, because of no matching following subfigures``()=
        prolongate_one_mapping_with_next_subfigures
            example.Figure.fitting_stencil_as_figure
            example.Figure.a_high_level_relatively_simple_figure
            Map.empty
            [Vertex_id "f#1"]
            initial_mapping_without_prolongation
        |> should equal
            []
    
    [<Fact>]
    let``prolongation with a single following node``()=
        
        prolongate_one_mapping_with_next_subfigures
            example.Figure.fitting_stencil_as_figure
            example.Figure.a_high_level_relatively_simple_figure
            Map.empty
            [Vertex_id "f#1"]
            initial_useless_mapping
        |> should equal
            [
                Immutable_mapping.ofStringPairs [
                    "b#1","b#2";
                    "h#1","h#1";
                    "f#1","f#2"
                ]
            ]
    

    
    [<Fact>]
    let ``several mappings can be produced by one stencil application``()=
        
        (
             map_figure_onto_target
                Mapping_functions_registry.id_into_function
                example.Figure.a_high_level_relatively_simple_figure
                example.Figure.fitting_stencil_as_figure
                |> Set.ofSeq
        )
        |>should equal (
            Set.ofSeq [
                Immutable_mapping.ofStringPairs [
                    "b#1","b#1"
                    "h#1","h#1"
                    "f#1","f#2"
                ];
                Immutable_mapping.ofStringPairs [
                    "b#1","b#2"
                    "h#1","h#1"
                    "f#1","f#2"
                ]
            ])

    [<Fact>]
    let ``a full mapping can be produced if the stencil has only "out" in the middle``()=
        let mappee ={
            Conditional_stencil.figure=
                [
                    "N",",#2";
                    ",#1",",#2";
                    "N",";";
                    ",#1",";";
                ]
                |>built.Figure.simple
                    (extensions.String.remove_number >> Mapping_functions_registry.onto_exact_figure)
                    Mapping_functions_registry.id_into_name
                |>built.Conditional_figure.from_figure_without_impossibles
            output_border = {
                before = ["N";",#1"] |> List.map Vertex_id |>Set.ofList 
                after = [";";",#2"] |> List.map Vertex_id |>Set.ofList 
            } 
        }
        let target =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequential_figure_from_text
                Figure_registry.provide_signal
                Figure_registry.id_into_name
   
        let mappings =
            map_figure_onto_target
                Mapping_functions_registry.id_into_function
                target
                mappee.figure.existing
            |>Set.ofSeq
        
        mappings
        |>Seq.iter (fun mapping->
            mapping
            |>Seq.map (_.Key)
            |>Set.ofSeq
            |>should equal (
                ["N";",#1";",#2";";"]
                |>Seq.map Vertex_id
                |>Set.ofSeq
            )
        )

    [<Fact>]
    let ``a fitting stencil, applied to a figure, outputs a subgraph (with several vertices)``()=
        
        results_of_conditional_stencil_application
            Mapping_functions_registry.id_into_function
            example.Stencil.a_fitting_stencil
            example.Figure.a_high_level_relatively_simple_figure
        |>Set.ofSeq
        |>should equal (
            [
                ["d","e"]|>built.Figure.simple_without_separator;
                "h"
                |>built.Figure.signal
                    figure_name_to_id
                    figure_id_to_name
            ]|>Set.ofList
        )

    [<Fact>]
    let ``a fitting stencil, applied to a figure, outputs a subgraph (with only one vertex)``()=
        
        built.Figure.sequential_figure_from_text
            figure_name_to_id
            figure_id_to_name
            "N0;"
        |>results_of_conditional_stencil_application
            Mapping_functions_registry.id_into_function
            {
                Conditional_stencil.figure=
                    [
                        "N",";";
                    ]|>built.Conditional_figure.conditional_figure_mapped_onto_constants
                output_border = {
                    before = "N" |>Vertex_id|>Set.singleton
                    after = ";" |>Vertex_id|>Set.singleton
                } 
            }
        |>should equal [
            built.Figure.signal
                figure_name_to_id
                figure_id_to_name
                "0"
        ]
    
    [<Fact>]
    let ``a long fitting stencil, applied to a long figure, outputs its result (two disjoined subgraphs)``()=
        results_of_conditional_stencil_application
            Mapping_functions_registry.id_into_function
            example.Stencil.a_long_stencil
            example.Figure.a_long_figure
        |>Set.ofSeq
        |>should equal (
            [
                ["k","l";"m","n"]|>built.Figure.simple_without_separator
            ]|>Set.ofList
        )
    
    
    [<Fact>]
    let ``mapping of first stencil subfigures onto target produces initial mapping``()=
        Map_first_nodes.map_first_nodes_with_immutable_mapping
            Mapping_functions_registry.id_into_function
            example.Figure.fitting_stencil_as_figure
            example.Figure.a_high_level_relatively_simple_figure
        |>Set.ofSeq
        |> should equal ([   
            Immutable_mapping.ofStringPairs ["b#1","b#1";"h#1","h#1"];
            Immutable_mapping.ofStringPairs ["b#1","b#3";"h#1","h#1"];
            Immutable_mapping.ofStringPairs ["b#1","b#2";"h#1","h#1"]
        ]|>Set.ofSeq)
    
    [<Fact>]
    let ``initial mapping when the target lacks some figures``()=
        Map_first_nodes.map_first_nodes_with_immutable_mapping
            Mapping_functions_registry.id_into_function
            (
                built.Figure.simple
                    (extensions.String.remove_number >> Mapping_functions_registry.onto_exact_figure)
                    Mapping_functions_registry.id_into_name
                    [
                        "b","f";
                        "h","f";
                        "x","f";
                    ]
            )
            example.Figure.a_high_level_relatively_simple_figure
        |> should be Empty

    
    [<Fact>]
    let ``complete mapping of stencil onto target can be produced``()=
        
        map_figure_onto_target
            Mapping_functions_registry.id_into_function
            example.Figure.a_high_level_relatively_simple_figure
            example.Figure.fitting_stencil_as_figure
        |> should equal
            [
                Immutable_mapping.ofStringPairs [
                    "b#1","b#1";
                    "h#1","h#1";
                    "f#1","f#2";
                ];
                Immutable_mapping.ofStringPairs [
                    "b#1","b#2";
                    "h#1","h#1";
                    "f#1","f#2";
                ];
            ]

            
    let ``paint the target figure and the stencil``()=
        let figure = example.Figure.a_high_level_relatively_simple_figure
        let stencil = example.Stencil.a_fitting_stencil

        "F"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_circle_vertices
        |>infrastructure.Graph.with_filled_vertex "target figure" 
            (painted.Graph.add_graph figure.edges)
        |>infrastructure.Graph.with_filled_vertex "stencil"
            (painted.Graph.add_graph stencil.figure.existing.edges)
        |>painted.image.open_image_of_graph

    [<Fact>]
    let ``apply stencil to a long mathematical sequence``()=
        let middle_digit_stencil =
            {
                Conditional_stencil.figure =
                    [
                        "N",",#2";
                        "N",";";
                        ",#1",",#2";
                        ",#1",";";
                    ]|>built.Conditional_figure.conditional_figure_mapped_onto_constants
                output_border = {
                    before = ["N";",#1"] |>List.map Vertex_id|>Set.ofList 
                    after = [",#2";";"] |>List.map Vertex_id|>Set.ofList 
                } 
            }

        let history_as_figure =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequential_figure_from_text
                figure_name_to_id
                figure_id_to_name
        
        history_as_figure
        |>results_of_conditional_stencil_application
              Mapping_functions_registry.id_into_function
              middle_digit_stencil 
        |>Set.ofSeq
        |>should equal (
            "12345678"
            |>Seq.map string
            |>Seq.map (
                built.Figure.signal
                    figure_name_to_id
                    figure_id_to_name
            )
            |>Set.ofSeq
        )
        
    // [<Fact>]
    // let ``apply stencil, when some first mapped vertices whould be skipped``()=
    //     let last_digit_stencil =
    //         {
    //             built.Stencil.simple_with_separator [
    //                 "N","out";
    //                 ",#1","out";
    //                 "out",";";
    //             ] with
    //                 output_without=
    //                     ","|>built.Figure.signal|>Set.singleton
    //         }
    //     
    //     let history_as_figure =
    //         "N0,1,2,3,4,5,6,7,8,9;"
    // //mom:   0123456789¹123456789²
    //         |>built.Figure.sequential_figure_from_text
    //
    //     history_as_figure
    //     |>results_of_stencil_application last_digit_stencil
    //
    //     |>should equal (
    //         "9"
    //         |>built.Figure.signal
    //         |>Seq.singleton
    //     )

    [<Fact>]
    let ``applying a stencil within another mapping limits targets of application(simple example)``()=
        let mappee =
            built.Figure.sequential_figure_from_sequence_of_vertices
                (extensions.String.remove_text_with_hash >> Mapping_functions_registry.onto_exact_figure)
                ["D#1";";#1"]
        
        let target =
            ["D#bad_first";"D#good_last";";";]
            |>built.Figure.sequential_figure_from_sequence_of_vertices
                (extensions.String.remove_text_with_hash >> figure_name_to_id)
        
        let within_mapping =
            [
                "D#1","D#good_last";
                ";#1",";";
            ]
            |>List.map(fun (tail, head) -> Vertex_id tail,Vertex_id head)
            |>Map.ofList
        
        let resulting_mapping =
            [
                "D#1","D#good_last";
                ";#1",";";
            ]
            |>List.map(fun (tail, head) -> Vertex_id tail,Vertex_id head)
            |>Map.ofList
        
        map_figure_onto_target_within_mapping
            Mapping_functions_registry.id_into_function
            within_mapping
            target
            mappee
        |>should equal (
            [resulting_mapping]
        )
    
    [<Fact>]
    let ``applying a stencil within another mapping. doubled ending shouldn't be mapped twice``()=
        let mappee =
            built.Figure.sequential_figure_from_sequence_of_vertices
                (extensions.String.remove_text_with_hash >> Mapping_functions_registry.onto_exact_figure)
                ["D#1";";#2";";#1"]
        
        let target =
            ["x#first";"D#bad_first";"y";"D#good_last";"a";"b";";#good_first";"z";";#bad_last";"x#last"]
            |>built.Figure.sequential_figure_from_sequence_of_vertices
                (extensions.String.remove_text_with_hash >> Figure_registry.provide_signal)
        
        let within_mapping =
            [
                ";#1",";#good_first"
                "D#1","D#good_last"
            ]
            |>List.map(fun (tail, head) -> Vertex_id tail,Vertex_id head)
            |>Map.ofList
        
        map_figure_onto_target_within_mapping
            Mapping_functions_registry.id_into_function
            within_mapping
            target
            mappee
        |>should equal (
            []
        )  
        
    [<Fact>]
    let ``apply stencil with a blocking figure, for a "tight" application of stencil``()=
        let vertex_to_function_id =
            (extensions.String.remove_text_with_hash >> Mapping_functions_registry.onto_exact_figure)
        
        let vertex_to_figure_id =
            (extensions.String.remove_text_with_hash >> Figure_registry.provide_signal)
        
        let existing_figure =
            built.Figure.sequential_figure_from_sequence_of_vertices
                vertex_to_function_id
                ["D#1";";#1"]
                
        let impossible_figure_before =
            built.Figure.sequential_figure_from_sequence_of_vertices
                vertex_to_function_id
                ["D#1";"D#2";";#1"]
            
        let impossible_figure_after =
            built.Figure.sequential_figure_from_sequence_of_vertices
                vertex_to_function_id
                ["D#1";";#2";";#1"]
            
        let conditional_figure = {
            Conditional_figure.existing = existing_figure
            impossibles=[
                impossible_figure_before|>Conditional_figure.from_figure
                impossible_figure_after|>Conditional_figure.from_figure
            ]|>Set.ofList
        }
        
        let conditional_stencil = {
            Conditional_stencil.figure=conditional_figure
            output_border =  {
                before = "D#1"|>Vertex_id|>Set.singleton
                after = ";#1"|>Vertex_id|>Set.singleton
            }
        }
        
        let history =
            ["x#first";"D#bad_first";"y";"D#good_last";"a";"b";";#good_first";"z";";#bad_last";"x#last"]
            |>built.Figure.sequential_figure_from_sequence_of_vertices
                vertex_to_figure_id
            
        results_of_conditional_stencil_application
            Mapping_functions_registry.id_into_function
            conditional_stencil
            history
        |>Seq.map (Renaming_figures.rename_vertices_to_standard_names Figure_registry.id_into_name)
        |>should equal (
            "ab"
            |>built.Figure.sequential_figure_from_text
                figure_name_to_id
                figure_id_to_name
            |>Seq.singleton
        )
        
    [<Fact>]
    let ``applying a conditional stencil with output at the very border``() =
        let vertex_to_function_id =
            (extensions.String.remove_text_with_hash >> Mapping_functions_registry.onto_exact_figure)
        
        let target =
            "0,1"
            |>built.Figure.sequential_figure_from_text
                figure_name_to_id
                figure_id_to_name
        
        let stencil = {
            Conditional_stencil.figure=
                [",#1"]
                |>built.Figure.sequential_figure_from_sequence_of_vertices vertex_to_function_id
                |>built.Conditional_figure.from_figure_without_impossibles
            output_border =  {
                before = Set.empty
                after = ",#1"|>Vertex_id|>Set.singleton
            }
        }
        
        results_of_conditional_stencil_application
            Mapping_functions_registry.id_into_function
            stencil
            target
        |>should equal (
            "0"
            |>built.Figure.sequential_figure_from_text
                figure_name_to_id
                figure_id_to_name
            |>Seq.singleton
        )