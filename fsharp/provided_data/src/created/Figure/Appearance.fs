module rvinowise.ai.created.figure.Appearance

open rvinowise
open rvinowise.ai


let new_input figure_id = 
    let appearance = 
        Interval.moment loaded.figure.Appearances.current_moment
    
    database.Write.figure_appearance figure_id appearance