namespace rvinowise.ai.stencil

open System.Collections.Generic
open rvinowise.ai


type Stencil(
    id, 
    edges
) =
    member this.id:Figure_id = id
    member this.edges: Edge seq = edges
    
    new (id) =
        Stencil(id,[])

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Stencil =
    ()
        

