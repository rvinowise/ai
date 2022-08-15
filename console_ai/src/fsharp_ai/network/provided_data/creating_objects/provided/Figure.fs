module rvinowise.ai.provided.Figure

open System.Collections.Generic

open rvinowise
open rvinowise.ai


let empty id: ai.Figure = {
    id = id
    appearances=[]
    edges=[]
}
    
let with_id
    (id:string)
    (loaded_figures:Dictionary<string, Figure>) :
    Figure
    =
    let is_found, full_figure = loaded_figures.TryGetValue id 
    match is_found with
    | true -> full_figure
    | false ->
        empty id