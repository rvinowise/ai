module rvinowise.ai.built.fusing_sequences
    
    open Xunit
    open FsUnit
    
    open rvinowise.ai
    open rvinowise.extensions
    open rvinowise


    let sequential_pair 
        (a_sequence: Figure_id array)
        (b_sequence: Figure_id array)
        =
        Array.append a_sequence b_sequence

    [<Fact>]
    let ``try sequential_pair``()=
        let a_sequence = [|"a";"ab";"b"|]
        let b_sequence = [|"a";"bb";"e"|]
          
        let real_ab_sequence = 
            sequential_pair
                a_sequence
                b_sequence
        
        [|"a";"ab";"b";"a";"bb";"e"|]
        |>should equal real_ab_sequence
            
        

        