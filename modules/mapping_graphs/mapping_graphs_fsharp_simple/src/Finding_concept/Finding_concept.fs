namespace rvinowise.ai

module Finding_concept = 
 
    open rvinowise.ai
    open rvinowise.extensions

    open Xunit
    open FsUnit


    let rec occurances 
        (place: Figure)
        (concept: Concept)
        =
        match concept with
        |Leaf stencil->
            stencil
            |>Applying_Stencil.results_of_stencil_application place
            |>Seq.singleton
        |Or children->
            children
            |>Seq.collect (occurances place)
        |And children->
            children
            |>Seq.map (occurances place)
            |>Seq.map Set.ofSeq
            |>Set.intersectMany


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
        let last_digit_stencil =
            built.Stencil.simple_with_separator [
                "N","out";
                ",","out";
                "out",";";
            ]
        let digit_concept = 
            [
                Concept.leaf first_digit_stencil;
                Concept.leaf middle_digit_stencil;
                Concept.leaf last_digit_stencil
            ]|>Concept.or 
        

        let history_as_figure =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequence_from_text

        digit_concept
        |>occurances history_as_figure
        |>Set.ofSeq
        |>should equal (
            "0123456789"
            |>Seq.map string
            |>Seq.map built.Figure.signal
            |>Set.ofSeq
        )