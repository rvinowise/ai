namespace rvinowise.ai

open System.Diagnostics
open BenchmarkDotNet.Engines
open Xunit
open FsUnit
open rvinowise.extensions


(* a concept (e.g. a Digit) can be represented by a consecutive application of stencils,
so that following stencils are applied to the result of previous stencil applications. it's an algorithm *)

    
module Digit_concept =
    
    
    let digit_declaration_stencil = {
        Conditional_stencil.figure =
            {
                existing = built.Figure.sequential_figure_from_text "D;"
                impossibles =
                    [
                        ["D#1";"D#2";";#1"]
                        |>built.Figure.sequential_figure_from_sequence_of_vertices String.remove_number_with_hash
                        ["D#1";";#2";";#1"]
                        |>built.Figure.sequential_figure_from_sequence_of_vertices String.remove_number_with_hash
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
                existing = built.Figure.sequential_figure_from_text ",,"
                impossibles =
                    [
                        [",#1";",#3";",#2"]
                        |>built.Figure.sequential_figure_from_sequence_of_vertices String.remove_number_with_hash
                        ["D#1";";#2";";#1"]
                        |>built.Figure.sequential_figure_from_sequence_of_vertices String.remove_number_with_hash
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
                impossibles =
                    [",#2";",#1"]
                    |>built.Figure.sequential_figure_from_sequence_of_vertices String.remove_number_with_hash
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
                impossibles =
                    [",#1";",#2"]
                    |>built.Figure.sequential_figure_from_sequence_of_vertices String.remove_number_with_hash
                    |>built.Conditional_figure.from_figure_without_impossibles
                    |>Set.singleton
            }
        output_border = {
            before = ",#1" |>Vertex_id|>Set.singleton
            after =  Set.empty
        }
    }
        
    let finding_digits_between_commas =
        [
            Applying_stencil.results_of_conditional_stencil_application between_commas_stencil
            Applying_stencil.results_of_conditional_stencil_application before_commas_stencil
            Applying_stencil.results_of_conditional_stencil_application after_commas_stencil
        ]
        
    let history =
        "D0,1,2,3,4,5,6,7,8,9;"
        |>built.Figure.sequential_figure_from_text
       
    
    let find_incarnations_of_digit target =
        target
        |>Applying_stencil.results_of_conditional_stencil_application digit_declaration_stencil
        |>Seq.map (Algorithm.apply_parallel_functions finding_digits_between_commas)
        |>Seq.collect id
        
    [<Fact>]
    let ``find incarnations of digit-concept``()=
        "D0,1,2,3,4,5,6,7,8,9;"
        |>built.Figure.sequential_figure_from_text
        |>find_incarnations_of_digit
        |>Set.ofSeq
        |>should equal (
            "0123456789"
            |>Seq.map string
            |>Seq.map built.Figure.signal
            |>Set.ofSeq
        )

    [<Fact>]
    let ``find incarnations of concept in several places of incarnation``()=
        let history_as_figure =
            "D0,1;x,y;z,D0,2;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequential_figure_from_text

        let incarnations = 
            find_incarnations_of_digit history_as_figure
        
        incarnations
        |>Seq.filter(fun figure->
            figure
            |>Figure.is_signal "0"
        )|>Seq.length
        |>should equal 2
    
        incarnations
        |>Seq.map Renaming_figures.rename_vertices_to_standard_names
        |>Set.ofSeq
        |>should equal (
            "012"
            |>Seq.map string
            |>Seq.map built.Figure.signal
            |>Set.ofSeq
        )
        
    [<Fact>]
    let ``mathematical primer``()=
        ()
        
module Number_concept =
    
    let not_digit_subfigure = {
        Subfigure.name = Figure_id "[not_digit]"
        is_mappable =
            //Digit_concept.find_incarnations_of_digit
            built.Subfigure.does_subfigure_reference_needed_figure
    }
    let number_concept = {
        Conditional_stencil.figure = {
            existing =
                [
                    not_digit_subfigure,1
                    not_digit_subfigure,2
                ]
                |>built.Figure.sequential_figure_from_sequence_of_subfigures
            impossibles =
                ["[not_digit]#1";"[not_digit]#3";"[not_digit]#2"]
                |>built.Figure.sequential_figure_from_sequence_of_vertices String.remove_number_with_hash
                |>built.Conditional_figure.from_figure_without_impossibles
                |>Set.singleton
        }
        output_border = {
            before = "[not_digit]#1"|>Vertex_id|>Set.singleton
            after = "[not_digit]#2"|>Vertex_id|>Set.singleton
        } 
    }
    
    let find_instances_of_number target =()
        