module rvinowise.ai.example.Figure_history
    open Xunit
    open FsUnit
    open rvinowise.ai

    let short_history_with_some_repetitions=
        built.Figure_history.from_tuples "a" [
                10,15;
                11,16;
                15,17;
                20,20
            ]
    
    let another_history_for_combining_togetner=
        built.Figure_history.from_tuples "b" [
                11,12;
                12,14;
                13,17;
                20,24
            ]

        

    