namespace rvinowise.ai.created.figure

open rvinowise.ai


module Appearance=

    let new_input figure_id = 
        let appearance = 
            Interval.moment loaded.figure.Appearances.current_moment
        
        database.Write.figure_appearance figure_id appearance