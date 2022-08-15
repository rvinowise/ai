module rvinowise.ai.loaded.Figure

open System.Collections.Generic

open rvinowise
open rvinowise.ai


let with_id id: ai.Figure option =
    let appearances = 
        database.read.appearances_of id database.postgres.sql_uri 

    match appearances.Length with
    | 0 -> None
    | _ -> Some {
        id=id
        appearances = appearances
        edges=database.read.internal_structure_of 
            id database.postgres.sql_uri (Dictionary<string, Figure>())

    }
