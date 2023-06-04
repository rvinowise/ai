module rvinowise.ai.example.Apperances
    open Xunit
    open FsUnit
    open rvinowise.ai

    let short_history_with_some_repetitions=
        built.Appearances.from_tuples [
                10,15;
                11,16;
                15,17;
                20,20
            ]
    
    let another_history_for_combining_togetner=
        built.Appearances.from_tuples [
                11,12;
                12,14;
                13,17;
                20,24
            ]

        

    