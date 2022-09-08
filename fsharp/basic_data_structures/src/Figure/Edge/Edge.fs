namespace rvinowise.ai.figure

(* 
fsharp implementation heavily relies on the database, so, 
instead of the references to other objects in memory,
it uses their identifiers in the database (e.g. string id)
*)

type Edge(head: string, tail: string) = 
    member _.head = head
    member _.tail = tail

    new (parent: string, head, tail) =
        Edge(head, tail)

