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
