module rvinowise.ai.provided.Figure

open System.Collections.Generic

open rvinowise
open rvinowise.ai
open rvinowise.ai.figure


let empty id: ai.figure.Figure = Figure(id)
    
let with_id
    (id:string)
    (loaded_figures:Dictionary<string, ai.figure.Figure>) :
    ai.figure.Figure
    =
    let is_found, full_figure = loaded_figures.TryGetValue id 
    match is_found with
    | true -> full_figure
    | false ->
        empty id