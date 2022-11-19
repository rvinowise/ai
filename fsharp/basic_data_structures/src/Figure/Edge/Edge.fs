namespace rvinowise.ai.figure

open rvinowise.ai

(* 
fsharp implementation heavily relies on the database, so, 
instead of the references to other objects in memory,
it uses their identifiers in the database (e.g. string id)
*)

type Edge = 
    struct
        val head: Subfigure
        val tail: Subfigure
    end

