
module rvinowise.ai.example.Stencil
    open rvinowise.ai

    let a_fitting_stencil =
        built.Stencil.simple
            "S"
            [
                "b","out1";
                "out1","f";
                "h","f";
            ]