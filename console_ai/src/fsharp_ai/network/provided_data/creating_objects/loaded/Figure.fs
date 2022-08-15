module rvinowise.ai.loaded.Figure

open System.Collections.Generic

open rvinowise
open rvinowise.ai


let with_id id: ai.Figure option =
    let appearances = 
        database.Read.appearances_of id database.postgres.sql_uri 

    match appearances.Length with
    | 0 -> None
    | _ -> Some {
        id=id
        appearances = appearances
        edges=database.Read.internal_structure_of 
            id database.postgres.sql_uri (Dictionary<string, Figure>())

    }
