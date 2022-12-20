namespace rvinowise.ai.test

open Xunit

open rvinowise
open rvinowise.ai
open rvinowise.ai.stencil
        
module ``the mapping type``=
    open FsUnit
    open rvinowise.ai.stencil
    open rvinowise.ai
    open System.Collections.Generic
    

        

    type ``comparing mappings with each other``()=
        
        [<Fact>]
        member this.``comparing equal mappings``()=    
            let mapping1 = Mapping(
                dict [
                    "a","a1";
                    "b","b1";
                    "c","c1";
                ]
            )
            let mapping2 = Mapping(
                dict [
                    "a","a1";
                    "b","b1";
                    "c","c1";
                ]
            )
            mapping1=mapping2
            |>should be True
            
        [<Fact>]
        member this.``comparing different mappings``()=    
            let mapping1 = Mapping(
                dict [
                    "a","a1";
                    "b","b1";
                    "c","c1";
                ]
            )
            let mapping2 = Mapping(
                dict [
                    "a","a2";
                    "b","b1";
                    "c","c1";
                ]
            )
            mapping1=mapping2
            |>should be False
     
        [<Fact>]
        member this.``adding different mappings to a set``()=    
            
            [
                Mapping(
                    dict [
                        "a","a1";
                        "b","b1";
                        "c","c1";
                    ]
                )
                Mapping(
                    dict [
                        "a","a2";
                        "b","b1";
                        "c","c1";
                    ]
                )
            ]
            |>Set.ofSeq
            |>should haveCount 2
           
        [<Fact>]
        member this.``adding same mappings to a set``()=    
            [
                Mapping(
                    dict [
                        "a","a1";
                        "b","b1";
                        "c","c1";
                    ]
                )
                Mapping(
                    dict [
                        "a","a1";
                        "b","b1";
                        "c","c1";
                    ]
                )
            ]
            |>Set.ofSeq
            |>should haveCount 1
            
        
        type ``creating mappings``()=
            
            [<Fact>]
            member this.``several mutable mappings can coexist without sharing data``()=
                let mapping1 = Mapping.empty()
                mapping1["a"] <- "a1"
                let mapping2 = Mapping.empty()
                mapping2["a"] <- "a2"
                
                mapping2
                |>should haveCount 1
                
            [<Fact>]
            member this.``a copy of another mapping has same structure``()=
                let original = Mapping [
                    "a","a0";
                    "b","b0"
                ]
                
                let copy = Mapping.copy original 
                
                (copy=original)|>should be True
                copy|>should haveCount 2
                
                copy["b1"] <- "b1"
                copy|>should haveCount 3
                original|>should haveCount 2