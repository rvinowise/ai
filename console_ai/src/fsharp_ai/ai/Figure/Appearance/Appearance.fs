namespace rvinowise.ai.figure


type Appearance(figure: string, head: int64, tail: int64) = 

    member _.figure = figure
    member _.head = head
    member _.tail = tail

    new (figure, moment) =
        Appearance(figure, moment, moment)

