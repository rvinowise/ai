namespace rvinowise.ai.figure


type Appearance(head: int64, tail: int64) = 
    member _.head = head
    member _.tail = tail

    new (figure: string, head, tail) =
        Appearance(head, tail)

