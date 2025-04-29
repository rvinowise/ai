namespace rvinowise.ai.example
open rvinowise.ai

module Figure =

    let a_high_level_relatively_simple_figure = 
        built.Figure.simple 
            [
                "b0","c";
                "b0","d";
                "c","b1";
                "d","e";
                "d","f0";
                "e","f1";
                "h","f1";
                "b2","h"
            ]

    let a_figure_with_huge_beginning = 
        built.Figure.simple 
            [
                "a0","x0";
                "b0","x0";
                "c0","x0";
                "d0","x0";
                "e0","x0";
                "g0","x0";
                "h0","x0";
                "i0","x0";
                "j0","x0";
                "k0","x0";
                "l0","x0";
                "m0","x0";
                "n0","x0";
                "o0","x0";
                "p0","x0";
                "q0","x0";
               
                "x0","y0";
                "y0","z0";
                "z0","f0";

                "a1","f0";
                "b1","f0";
                "c1","f0";
                "d1","f0";
                "e1","f0";
                "g1","f0";
                "h1","f0";
                "i1","f0";
                "j1","f0";
                "k1","x0";
                "l1","x0";
                "m1","x0";
                "n1","x0";
                "o1","x0";
                "p1","x0";
                "q1","x0";
            ]

    let create_a_bad_figure_with_cycle() = 
        built.Figure.simple 
            [
                "b0","x0";
                "f0","x0";
                "h0","x0";

                "x0","y0";
                "y0","z0";
                "z0","f0";

                "b1","f0";
                "f1","f0";
                "h1","f0";
            ]
    
    let fitting_stencil_as_figure =
        built.Figure.simple
            [
                "b","f";
                "h","f";
            ]

    let empty = built.Figure.from_tuples []
    
    
    let a_long_figure = 
        built.Figure.simple 
            [
                "b1","c1";
                "b1","d1";
                "c1","b3";
                "d1","e1";
                "d1","f1";
                "e1","f2";
                "b2","h1";
                "h1","f2";
                
                "f1","x1";
                "x1","y1";
                "y1","z1";
                "z1","p1";
                "p1","r1";
                "r1","s1";
                "s1","f3";
                "f3","t1"
                
                "f2","m1"
                "f2","k1"
                "m1","n1"
                "k1","l1"
                "n1","o1"
                "l1","o1"
                "o1","p1"
                "o1","q1"
                "q1","r1"
            ]