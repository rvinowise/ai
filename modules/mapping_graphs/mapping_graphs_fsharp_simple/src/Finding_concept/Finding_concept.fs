namespace rvinowise.ai

module Finding_concept = 
 
    open rvinowise.ai
    open rvinowise.extensions

    open Xunit
    open FsUnit


    let rec concrete_instances 
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
            |>Seq.collect (concrete_instances place)
            |>Set.ofSeq
        |And children ->
            children
            |>Seq.map (concrete_instances place>>Set.ofSeq)
            |>Set.intersectMany


    let digit_concept =
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
        |>List.map Concept.Leaf
        |>Set.ofList
        |>Concept.Or 

    [<Fact>]
    let ``try digit concept``()=
        let history_as_figure =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequential_figure_from_text

        digit_concept
        |>concrete_instances history_as_figure
        |>Set.ofSeq
        |>should equal (
            "0123456789"
            |>Seq.map string
            |>Seq.map built.Figure.signal
            |>Set.ofSeq
        )

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
                        |>Seq.collect id 
                        |>Set.ofSeq
            }
        let second_digit_of_primer =
            [
                "1";"+";"out";"="
            ]|>built.Stencil.sequential_stencil_from_sequence
        let math_equation_digit_addition_ =
            [
                "[digit]";"+";"[digit]";"=";"[digit]"
            ]|>built.Figure.sequential_figure_from_sequence
        let math_equation_addition_of_1 =
            [
                "1";"+";"[digit]";"=";"[digit]";";"
            ]|>built.Figure.sequential_figure_from_sequence
        // let next_digit = 
        //     [
        //         "reference";",";"out"
        //     ]|>built.Stencil.simple_with_separator
        
        ()