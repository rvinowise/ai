namespace rvinowise.ai.figure

open System.Collections.Generic
open rvinowise.ai

type Figure(
    id, 
    appearances, 
    edges
) =
    member this.id:Figure_id = id
    member this.appearances: Appearance seq = appearances
    member this.edges: Edge seq = edges
    
    new (id) =
        Figure(id, [],[])