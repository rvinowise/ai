
namespace rvinowise.ai.example
open rvinowise.ai

module Stencil =

    let empty =
        built.Stencil.simple_without_separator []

    let a_fitting_stencil =
        built.Stencil.simple_without_separator
            [
                "b","out1";
                "out1","f";
                "h","f";
            ]

    let a_stencil_with_huge_beginning =
        built.Stencil.simple_without_separator
            [
                "a","out1";
                "b","out1";
                "c","out1";
                "d","out1";
                "e","out1";
                "g","out1";
                "h","out1";
                "i","out1";
                "j","out1";
                "k","out1";
                "l","out1";
                "m","out1";
                "n","out1";
                "o","out1";
                "p","out1";
                "q","out1";

                "out1","f";

                "a","f";
                "b","f";
                "c","f";
                "d","f";
                "e","f";
                "g","f";
                "h","f";
                "i","f";
                "j","f";
                "k","f";
                "l","f";
                "m","f";
                "n","f";
                "o","f";
                "p","f";
                "q","f";
            ]