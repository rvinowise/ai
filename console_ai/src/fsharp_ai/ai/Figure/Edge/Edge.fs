namespace rvinowise.ai.figure


type Edge(head: string, tail: string) = 
    member _.head = head
    member _.tail = tail

    new (parent: string, head, tail) =
        Edge(head, tail)

