module rvinowise.ai.example.Figure
    open rvinowise.ai

    let a_high_level_relatively_simple_figure = 
        built.Figure.simple 
            [
                ("b0","c");
                ("b0","d");
                ("c","b1");
                ("d","e");
                ("d","f0");
                ("e","f1");
                ("h","f1");
                ("b2","h")
            ]

    let a_figure_with_huge_beginning = 
        built.Figure.simple 
            [
                ("a0","x0");
                ("b0","x0");
                ("c0","x0");
                ("d0","x0");
                ("e0","x0");
                ("f0","x0");
                ("g0","x0");
                ("h0","x0");
                ("i0","x0");
                ("j0","x0");
               
                ("x0","y0");
                ("y0","z0");
                ("z0","f0");

                ("a1","f0");
                ("b1","f0");
                ("c1","f0");
                ("d1","f0");
                ("e1","f0");
                ("f1","f0");
                ("g1","f0");
                ("h1","f0");
                ("i1","f0");
                ("j1","f0");
            ]