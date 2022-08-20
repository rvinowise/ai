namespace rvinowise.ai.figure

open System.Collections.Generic


type Figure(id, appearances, edges) =
    member this.id:string =  id
    member this.appearances: Appearance seq = appearances
    member this.edges: Edge seq = edges
    
    new (id) =
        Figure(id, [],[])