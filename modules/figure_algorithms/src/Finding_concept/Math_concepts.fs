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
    
    let digit_declaration_stencil = {
        Conditional_stencil.figure =
            {
                existing =
                    built.Figure.sequential_figure_from_text
                        mapping_name_to_id
                        mapping_id_to_name
                        "D;"
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
                    built.Figure.sequential_figure_from_text
                        mapping_name_to_id
                        mapping_id_to_name
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
                existing =
                    ","
                    |>built.Figure.sequential_figure_from_text
                        mapping_name_to_id
                        mapping_id_to_name
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
                existing =
                    ","
                    |>built.Figure.sequential_figure_from_text
                        mapping_name_to_id
                        mapping_id_to_name
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
        
    let history =
        "D0,1,2,3,4,5,6,7,8,9;"
        |>built.Figure.sequential_figure_from_text
            mapping_name_to_id
            mapping_id_to_name
       
    
    let find_incarnations_of_digit target =
        target
        |>results_of_stencil_application digit_declaration_stencil
        |>Seq.collect (Algorithm.apply_parallel_functions finding_digits_between_commas)
        
    [<Fact>]
    let ``find incarnations of digit-concept``()=
        "D0,1,2,3,4,5,6,7,8,9;"
        |>built.Figure.sequential_figure_from_text
            figure_name_to_id
            figure_id_to_name
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
            |>built.Figure.sequential_figure_from_text
                figure_name_to_id
                figure_id_to_name

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
        
    [<Fact>]
    let ``mathematical primer``()=
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
        