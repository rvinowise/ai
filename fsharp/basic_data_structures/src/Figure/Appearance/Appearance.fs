namespace rvinowise.ai.figure

open rvinowise.ai

type Appearance(figure: string, interval: Interval) = 

    member this.figure = figure
    member this.interval = interval
    
    member this.head=this.interval.head
    member this.tail=this.interval.tail

    new (figure, moment: int64) =
        Appearance(figure, {head= moment; tail= moment})
    new (figure, head, tail) =
        Appearance(figure, {head= head; tail= tail})
