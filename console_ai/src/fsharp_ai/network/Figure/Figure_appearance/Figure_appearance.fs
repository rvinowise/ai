namespace rvinowise.ai


type Figure_appearance(head: int64, tail: int64) = 
    member _.head = head
    member _.tail = tail

    new (figure: string, head, tail) =
        Figure_appearance(head, tail)

