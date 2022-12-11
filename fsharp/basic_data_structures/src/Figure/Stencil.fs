namespace rvinowise.ai

open System.Collections.Generic
open rvinowise.ai


type Stencil(
    id, 
    edges
) =
    member this.id:Figure_id = id
    member this.edges: stencil.Edge seq = edges
    
    new (id) =
        Stencil(id,[])

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Stencil =
    
    let first_subfigures (stencil:Stencil) =
        stencil.edges
        |>rvinowise.ai.stencil.Edges.first_nodes 
        |>Nodes.only_subfigures

