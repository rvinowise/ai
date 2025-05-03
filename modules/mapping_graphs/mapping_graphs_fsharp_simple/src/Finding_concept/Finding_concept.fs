namespace rvinowise.ai

open rvinowise.ai

open Xunit
open FsUnit


module Finding_concept = 

    let rec incarnations_of_concept 
        (place: Figure)
        (concept: Concept)
        =
        match concept with
        |Leaf stencil->
            stencil
            |>Applying_stencil.results_of_stencil_application place
            |>Set.ofSeq
        |Or children->
            children
            |>Seq.collect (incarnations_of_concept place)
            |>Set.ofSeq
        |And children ->
            children
            |>Seq.map (incarnations_of_concept place>>Set.ofSeq)
            |>Set.intersectMany


    let complex_digit_concept =
        let first_digit_stencil =
            built.Stencil.simple_with_separator [
                "N","out";
                "out",",";
                "out",";";
            ] 
        let middle_digit_stencil =
            built.Stencil.simple_with_separator [
                "N","out";
                ",#1","out";
                "out",",#2";
                "out",";";
            ] 
        let last_digit_stencil = //{
            built.Stencil.simple_with_separator [
                "N","out";
                ",","out";
                "out",";";
            ]
        
        [
            first_digit_stencil;
            middle_digit_stencil;
            last_digit_stencil
        ]
        |>List.map (fun stencil->
            {stencil with 
                output_without=
                    ","|>built.Figure.signal|>Set.singleton})
        |>List.map Leaf
        |>Set.ofList
        |>Or 

    let digit_concept =
        {
            (
                built.Stencil.simple_with_separator [
                    "N","out";
                    "out",";";
                ]
            ) with 
                output_without=
                    ","|>built.Figure.signal|>Set.singleton
        }|>Leaf

    let digit_concept_between_commas =
        {
            (
                built.Stencil.simple_with_separator [
                    ",","out";
                    "out",",";
                ]
            ) with 
                output_without=
                    ","|>built.Figure.signal|>Set.singleton
        }|>Leaf

    [<Fact>]
    let ``try incarnations of digit concept``()=
        let history_as_figure =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequential_figure_from_text

        digit_concept
        |>incarnations_of_concept history_as_figure
        |>should equal (
            "0123456789"
            |>Seq.map string
            |>Seq.map built.Figure.signal
            |>Set.ofSeq
        )

    [<Fact>]
    let ``try incarnations of concept in several places of incarnation``()=
        (*TODO "N0" and "y" are mistakenly considered number incarnations *)
        let history_as_figure =
            "N0,1;x,y;z,N0,2;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequential_figure_from_text

        let incarnations = 
            complex_digit_concept
            |>incarnations_of_concept history_as_figure
        
        incarnations
        |>Set.filter(fun figure->
            figure
            |>Figure.is_signal "0"
        )|>should haveCount 2
    
        incarnations
        |>should equal (
            "012"
            |>Seq.map string
            |>Seq.map built.Figure.signal
            |>Set.ofSeq
        )

    let appearances_of_concept_incarnations
        (history: Figure)
        concept
        =
        let incarnations =         
            concept
            |>incarnations_of_concept history
        
        let vertices_of_incarnations = 
            incarnations
            |>Seq.map (fun figure->
                figure.subfigures
                |>Map.keys
                |>Set.ofSeq
            )
            |>Set.ofSeq

        incarnations
        |>Seq.map (
            Mapping_graph_with_immutable_mapping.map_figure_onto_target
                history
        )|>Seq.concat
        |>Seq.filter (fun (appearance)->
            let appearance_vertices = 
                appearance.Keys
                |>Set.ofSeq
            
            vertices_of_incarnations
            |>Set.exists (fun incarnation_vertices->
                incarnation_vertices = appearance_vertices
            ) 
            |>not
        )

    [<Fact>]
    let ``try appearances of incarnations of digit concept``()=
        let history_as_figure =
            @"N0,1,2,3,4,5,6,7,8,9;
            N0,x;
            1+1=2;ok;"
    //mom:    0123456789¹123456789²
            |>History_from_text.sequential_figure_from_text
                (History_from_text.mood_changes_as_words_and_numbers "no" "ok")

        appearances_of_concept_incarnations
            history_as_figure
            digit_concept
        

    [<Fact>]
    let ``mathematical concepts``()=
        let math_operations = 
            ["+";"-"]
            |>List.map built.Figure.signal
        let equation = "="|>built.Figure.signal
        let math_operators =
            equation::math_operations
        let control_flow = 
            [",";";";"ok";"no"]
            |>List.map built.Figure.signal
        let number =
            [
                "[digit]#1","[digit]#2"
            ]|>built.Stencil.simple_with_separator
            |>fun stencil->{
                stencil with 
                    output_without=
                        [math_operators;control_flow]
                        |>Seq.concat
                        |>Set.ofSeq
            }
        let second_digit_of_primer =
            [
                "1";"+";"out";"="
            ]|>built.Stencil.sequential_stencil_from_sequence
        let math_equation_digit_addition_ =
            [
                "[digit]";"+";"[digit]";"=";"[digit]"
            ]|>built.Figure.sequential_figure_from_sequence_of_figures
        let math_equation_addition_of_1 =
            [
                "1";"+";"[digit]";"=";"[digit]";";"
            ]|>built.Figure.sequential_figure_from_sequence_of_figures
        // let next_digit = 
        //     [
        //         "reference";",";"out"
        //     ]|>built.Stencil.simple_with_separator
        
        ()