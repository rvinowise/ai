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


    [<Fact>]
    let ``concept of number``()=
        let math_operator = 
            ["+";"=";"-"]
            |>List.map built.Figure.signal
        let control_flow = 
            [",";";";"ok";"no"]
            |>List.map built.Figure.signal
        [
            "[digit]#1","[digit]#2"
        ]|>built.Stencil.simple_with_separator
        |>fun stencil->{
            stencil with 
                output_without=
                    [math_operator;control_flow]
                    |>Seq.collect id 
                    |>Set.ofSeq
        }

    [<Fact>]
    let ``concept of digit``()=
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
        let digit_concept = 
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
        

        let history_as_figure =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequence_from_text

        digit_concept
        |>concrete_instances history_as_figure
        |>Set.ofSeq
        |>should equal (
            "0123456789"
            |>Seq.map string
            |>Seq.map built.Figure.signal
            |>Set.ofSeq
        )