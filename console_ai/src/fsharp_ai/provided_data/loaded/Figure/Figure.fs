namespace rvinowise.ai.loaded.figure

open System.Collections.Generic

open rvinowise
open rvinowise.ai

type Figure(id: string) = 
    member this.appearances =
        printfn $"figure %s{id} appearances requested"
        loaded.figure.Appearances.of_figure id
    
    member this.edges =
        printfn $"figure %s{id} edges requested"
        loaded.figure.Edges.of_figure id
      
    member this.exists =
        printfn $"figure %s{id} exists requested"
        not(database.Read.body_of id |>  Seq.isEmpty)
    member this.id = id

  

//let with_id id: ai.figure.Figure option =
//    let appearances 
//        = loaded.figure.Appearances.of_figure id
//
//    if Seq.isEmpty appearances then None
//    else Some {
//        id=id
//        appearances = appearances
//        edges=loaded.figure.Edges.of_figure id
//
//    }
