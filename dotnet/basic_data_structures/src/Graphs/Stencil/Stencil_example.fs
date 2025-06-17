namespace rvinowise.ai.example
open rvinowise.ai

module Stencil =

    let empty =
        built.Stencil.simple_without_separator []

    let a_fitting_stencil =
        built.Stencil.simple_without_separator
            [
                "b","out";
                "out","f";
                "h","f";
            ]

    let a_stencil_with_huge_beginning =
        built.Stencil.simple_without_separator
            [
                "a","out";
                "b","out";
                "c","out";
                "d","out";
                "e","out";
                "g","out";
                "h","out";
                "i","out";
                "j","out";
                "k","out";
                "l","out";
                "m","out";
                "n","out";
                "o","out";
                "p","out";
                "q","out";

                "out","f";

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
            
    let a_long_stencil =
        built.Stencil.simple_without_separator
            [
                "b1","y1"
                "b1","f1"
                "h1","f1"
                "y1","s1"
                "f1","out"
                "out","o1"
                "o1","r1"
                "r1","s1"
                "r1","f2"
                "s1","t1"
                "f2","t1"
            ]