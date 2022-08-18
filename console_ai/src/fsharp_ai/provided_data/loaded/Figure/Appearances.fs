module rvinowise.ai.loaded.figure.Appearances

open System.Collections.Generic

open rvinowise
open rvinowise.ai


let of_figure figure_id = 
    database.Read.appearances_of figure_id