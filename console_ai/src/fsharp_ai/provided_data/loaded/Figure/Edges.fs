module rvinowise.ai.loaded.figure.Edges

open System.Collections.Generic

open rvinowise
open rvinowise.ai


let of_figure figure_id = 
    database.Read.edges_of figure_id