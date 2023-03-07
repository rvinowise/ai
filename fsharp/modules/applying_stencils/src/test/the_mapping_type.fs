namespace rvinowise.ai.test

open Xunit

        
module ``the mapping type``=
    open FsUnit
    open rvinowise.ai.stencil
    open rvinowise.ai
    open System.Collections.Generic
    

        
    [<Fact>]
    let ``comparing equal mappings``()=    
        let mapping1 = Mapping.ofStringPairs [
            "a","a1";
            "b","b1";
            "c","c1";
        ]
        let mapping2 = Mapping.ofStringPairs [
            "a","a1";
            "b","b1";
            "c","c1";
        ]
        mapping1=mapping2
        |>should be True
        
    [<Fact>]
    let ``comparing different mappings``()=    
        let mapping1 = Mapping.ofStringPairs [
            "a","a1";
            "b","b1";
            "c","c1";
        ]
        let mapping2 = Mapping.ofStringPairs [
            "a","a2";
            "b","b1";
            "c","c1";
        ]
        mapping1=mapping2
        |>should be False
    
    [<Fact>]
    let ``adding different mappings to a set``()=    
        
        [
            Mapping.ofStringPairs [
                "a","a1";
                "b","b1";
                "c","c1";
            ]
            Mapping.ofStringPairs [
                "a","a2";
                "b","b1";
                "c","c1";
            ]
        ]
        |>Set.ofSeq
        |>should haveCount 2
        
    [<Fact>]
    let ``adding same mappings to a set``()=    
        [
            Mapping.ofStringPairs [
                "a","a1";
                "b","b1";
                "c","c1";
            ]
            Mapping.ofStringPairs [
                "a","a1";
                "b","b1";
                "c","c1";
            ]
        ]
        |>Set.ofSeq
        |>should haveCount 1
            
        
            
    [<Fact>]
    let ``several mutable mappings can coexist without sharing data``()=
        let mapping1 = Mapping.empty()
        mapping1[Vertex_id "a"] <- Vertex_id "a1"
        let mapping2 = Mapping.empty()
        mapping2[Vertex_id "a"] <- Vertex_id "a2"
        
        mapping2
        |>should haveCount 1
        
    [<Fact>]
    let ``a copy of another mapping has same structure``()=
        let original = Mapping.ofStringPairs [
            "a","a0";
            "b","b0"
        ]
        
        let copy = Mapping.copy original 
        
        (copy=original)|>should be True
        copy|>should haveCount 2
        
        copy[Vertex_id "b1"] <- Vertex_id "b1"
        copy|>should haveCount 3
        original|>should haveCount 2

    