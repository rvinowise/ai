module rvinowise.ai.created.figure.Appearance

open System.Collections.Generic

open rvinowise
open rvinowise.ai


let new_input figure_id = 
    let appearance: ai.figure.Appearance = new ai.figure.Appearance(
        figure_id,
        database.Read.current_moment
    )
    database.Write.figure_appearance figure_id appearance