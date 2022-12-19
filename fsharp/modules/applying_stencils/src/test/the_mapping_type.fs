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