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
            Mapping(
                dict [
                    "a","a1";
                    "b","b1";
                    "c","c1";
                ]
            )
     