namespace rvinowise.ai.figure

open rvinowise.ai

(* 
fsharp implementation heavily relies on the database, so, 
instead of the references to other objects in memory,
it uses their identifiers in the database (e.g. string id)
*)

type Edge(head: Subfigure_id, tail: Subfigure_id) = 
    member _.head = head
    member _.tail = tail

    new (parent: Figure_id, head, tail) =
        Edge(head, tail)

