namespace rvinowise.ai

open System.Diagnostics
open BenchmarkDotNet.Engines
open Xunit
open FsUnit
open rvinowise
open rvinowise.extensions


(* a concept (e.g. a Digit) can be represented by a consecutive application of stencils,
so that following stencils are applied to the result of previous stencil applications. it's an algorithm *)

    
module Digit_concept =
    
    let mapping_name_to_id = Mapping_functions_registry.onto_exact_figure
    let mapping_id_to_name = Mapping_functions_registry.id_into_name
    let figure_name_to_id = Figure_registry.provide_signal
    let figure_id_to_name = Figure_registry.id_into_name
    
    let signal = built.Figure.signal figure_name_to_id figure_id_to_name
    
    let sequential_unmapped_figure_from_sequence_of_vertices =
        built.Figure.sequential_figure_from_sequence_of_vertices
            (String.remove_number_with_hash >> Mapping_functions_registry.onto_exact_figure)
    
    let figure_from_sequence =
        built.Figure.sequential_figure_from_text
            figure_name_to_id
            figure_id_to_name
    
    let mappee_from_sequence =
        built.Figure.sequential_figure_from_text
            mapping_name_to_id
            mapping_id_to_name
    
    let digit_declaration_stencil = {
        Conditional_stencil.figure =
            {
                existing = mappee_from_sequence "D;"
                impossibles =
                    [
                        ["D#1";"D#2";";#1"]
                        |>sequential_unmapped_figure_from_sequence_of_vertices
                        ["D#1";";#2";";#1"]
                        |>sequential_unmapped_figure_from_sequence_of_vertices
                    ]
                    |>List.map built.Conditional_figure.from_figure_without_impossibles
                    |>Set.ofList
            }
        output_border = {
            before =  "D#1" |>Vertex_id|>Set.singleton
            after = ";#1" |>Vertex_id|>Set.singleton
        } 
    }
    
    let between_commas_stencil = {
        Conditional_stencil.figure =
            {
                existing =
                    mappee_from_sequence
                        ",,"
                impossibles =
                    [
                        [",#1";",#3";",#2"]
                        |>sequential_unmapped_figure_from_sequence_of_vertices
                        ["D#1";";#2";";#1"]
                        |>sequential_unmapped_figure_from_sequence_of_vertices
                    ]
                    |>List.map built.Conditional_figure.from_figure_without_impossibles
                    |>Set.ofList
            }
        output_border = {
            before =  ",#1" |>Vertex_id|>Set.singleton
            after = ",#2" |>Vertex_id|>Set.singleton
        } 
    }
    
    let before_commas_stencil = {
        Conditional_stencil.figure =
            {
                existing = mappee_from_sequence ","
                impossibles =
                    [",#2";",#1"]
                    |>sequential_unmapped_figure_from_sequence_of_vertices
                    |>built.Conditional_figure.from_figure_without_impossibles
                    |>Set.singleton
            }
        output_border = {
            before =  Set.empty
            after = ",#1" |>Vertex_id|>Set.singleton
        }
    }
    let after_commas_stencil = {
        Conditional_stencil.figure =
            {
                existing = mappee_from_sequence ","
                    
                impossibles =
                    [",#1";",#2"]
                    |>sequential_unmapped_figure_from_sequence_of_vertices
                    |>built.Conditional_figure.from_figure_without_impossibles
                    |>Set.singleton
            }
        output_border = {
            before = ",#1" |>Vertex_id|>Set.singleton
            after =  Set.empty
        }
    }
    
    let results_of_stencil_application =
        Applying_stencil.results_of_conditional_stencil_application Mapping_functions_registry.id_into_function
    
    let finding_digits_between_commas =
        [
            results_of_stencil_application between_commas_stencil
            results_of_stencil_application before_commas_stencil
            results_of_stencil_application after_commas_stencil
        ]
        
    let find_incarnations_of_digit target =
        target
        |>results_of_stencil_application digit_declaration_stencil
        |>Seq.collect (Algorithm.apply_parallel_functions finding_digits_between_commas)
        
    [<Fact>]
    let ``find incarnations of digit-concept``()=
        "D0,1,2,3,4,5,6,7,8,9;"
        |>figure_from_sequence
        |>find_incarnations_of_digit
        |>Set.ofSeq
        |>should equal (
            "0123456789"
            |>Seq.map string
            |>Seq.map signal
            |>Set.ofSeq
        )

    [<Fact>]
    let ``find incarnations of concept in several places of incarnation``()=
        let history_as_figure =
            "D0,1;x,y;z,D0,2;"
    //mom:   0123456789¹123456789²
            |>figure_from_sequence

        let incarnations = 
            find_incarnations_of_digit history_as_figure
        
        incarnations
        |>Seq.filter(fun figure->
            figure
            |>Figure.is_signal (figure_name_to_id "0")
        )|>Seq.length
        |>should equal 2
    
        incarnations
        |>Seq.map (Renaming_figures.rename_vertices_to_standard_names figure_id_to_name)
        |>Set.ofSeq
        |>should equal (
            "012"
            |>Seq.map string
            |>Seq.map signal
            |>Set.ofSeq
        )
        
        
    let is_non_digit figure vertex =
        let checked_subfigure =
            Figure.reference_of_vertex figure vertex
        
        find_incarnations_of_digit figure
        |>Seq.exists (fun digit_incarnation ->
            digit_incarnation
            |>Figure.is_signal checked_subfigure
        )|>not
        
    let non_digit_mappee =
        built.Figure.signal
            (fun _ -> Mapping_functions_registry.provide_function "non_digit" is_non_digit)
            Mapping_functions_registry.id_into_name
            "non_digit"
        |>built.Conditional_figure.from_figure_without_impossibles
                
    [<Fact>]
    let ``find non-digits, i.e. subfigures which are different from specified``()=
        let history =
            "D1,2;_1+1=2;"
    //mom:   0123456789¹123456789²123456789³123456789
            |>figure_from_sequence
        
        let found_non_digits =
            Mapping_graph_with_immutable_mapping.map_conditional_figure_onto_target
                Mapping_functions_registry.id_into_function
                Map.empty
                history
                non_digit_mappee
        let expected_target_subfigures =
            [
                "D#1";",#1";";#1";"+#1";"=#1";";#2";"_#1"
            ]|>Seq.map (Vertex_id >> Set.singleton)
            |>Set.ofSeq
        
        found_non_digits
        |>Seq.map (Map.values>>Set.ofSeq)|>Set.ofSeq
        |>should equal expected_target_subfigures
        
    let number_stencil =
        let non_digit_id = Mapping_functions_registry.remember_function "non-digit" is_non_digit
        {
            Conditional_stencil.figure =
                {
                    existing =
                        ["non-digit#1","non-digit#2"]
                        |>built.Figure.simple (fun _ -> non_digit_id)
                    impossibles =
                        ["non-digit#1";"non-digit#3";"non-digit#2"]
                        |>built.Figure.sequential_figure_from_sequence_of_vertices
                            (fun _ -> non_digit_id)
                        |>built.Conditional_figure.from_figure_without_impossibles
                        |>Set.singleton
                }
            output_border = {
                before = "non-digit#1" |>Vertex_id|>Set.singleton
                after = "non-digit#2" |>Vertex_id|>Set.singleton
            } 
        }    
    
    
    [<Fact>]
    let ``find numbers``()=
        
        let history =
            "D0,1,2,3,4; 1+1=2;× 12+31=43;×"
    //mom:   0123456789¹123456789²123456789³123456789
            |>figure_from_sequence
        
        let found_non_digits =
            Mapping_graph_with_immutable_mapping.map_conditional_figure_onto_target
                Mapping_functions_registry.id_into_function
                Map.empty
                history
                non_digit_mappee
        
        let found_numbers =
            history
            |>results_of_stencil_application number_stencil 
            |>Seq.map (Renaming_figures.rename_vertices_to_standard_names figure_id_to_name)
            
        let expected_numbers =
            [
                figure_from_sequence "1";
                figure_from_sequence "2";
                figure_from_sequence "12";
                figure_from_sequence "31";
                figure_from_sequence "43";
            ]|>Set.ofSeq
        
        found_numbers
        |>Set.ofSeq
        |>Set.intersect expected_numbers
        |>should equal expected_numbers 
        
        ()
    
    let math_primer =
        let non_digit_id = Mapping_functions_registry.provide_function "non_digit" is_non_digit
        let non_number_id = Mapping_functions_registry.provide_function "non_number" is_non_digit
        let number_id = Mapping_functions_registry.provide_function "number" is_non_digit
        {
            existing =
                ["number#1","non_number#1","number#2","=#1","number#3",";#1"]
                |>built.Figure.simple (fun _ -> non_digit_id)
            impossibles =
                Set.empty
        }
    
    let math_primer_stencil =
        let non_digit_id = Mapping_functions_registry.provide_function "non_digit" is_non_digit
        {
            Conditional_stencil.figure =
                {
                    existing =
                        ["number#1","non_number#2","number#2","=#1"]
                        |>built.Figure.simple (fun _ -> non_digit_id)
                    impossibles =
                        ["non_digit#1";"non_digit#3";"non_digit#2"]
                        |>built.Figure.sequential_figure_from_sequence_of_vertices
                            (fun _ -> non_digit_id)
                        |>built.Conditional_figure.from_figure_without_impossibles
                        |>Set.singleton
                }
            output_border = {
                before = "non_digit#1" |>Vertex_id|>Set.singleton
                after = "non_digit#2" |>Vertex_id|>Set.singleton
            } 
        }   
        
    [<Fact>]
    let ``find mathematical primers``()=
        let history_as_figure =
            "D0,1,2,3,4,5,6,7,8,9; 1+1=2;× 12+23=35;×"
    //mom:   0123456789¹123456789²
            |>figure_from_sequence
        
        
        ()
        
module Number_concept = ()
    
    // let not_digit_subfigure = {
    //     Subfigure.name = Figure_id "[not_digit]"
    //     is_mappable =
    //         //Digit_concept.find_incarnations_of_digit
    //         Figure_id "[not_digit]"
    //         |>built.Subfigure.does_subfigure_reference_needed_figure 
    // }
    // let number_concept = {
    //     Conditional_stencil.figure = {
    //         existing =
    //             [
    //                 not_digit_subfigure,1
    //                 not_digit_subfigure,2
    //             ]
    //             |>built.Figure.sequential_figure_from_sequence_of_subfigures
    //         impossibles =
    //             ["[not_digit]#1";"[not_digit]#3";"[not_digit]#2"]
    //             |>built.Figure.sequential_figure_from_sequence_of_vertices String.remove_number_with_hash
    //             |>built.Conditional_figure.from_figure_without_impossibles
    //             |>Set.singleton
    //     }
    //     output_border = {
    //         before = "[not_digit]#1"|>Vertex_id|>Set.singleton
    //         after = "[not_digit]#2"|>Vertex_id|>Set.singleton
    //     } 
    // }
    //
    // let find_instances_of_number target =()
        