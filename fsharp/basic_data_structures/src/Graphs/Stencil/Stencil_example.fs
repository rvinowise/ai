
module rvinowise.ai.example.Stencil
    open rvinowise.ai

    let a_fitting_stencil =
        built.Stencil.simple_without_separator
            [
                "b","out1";
                "out1","f";
                "h","f";
            ]