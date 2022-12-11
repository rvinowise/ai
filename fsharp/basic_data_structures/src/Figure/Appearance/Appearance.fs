namespace rvinowise.ai.figure

    open rvinowise.ai

    type Appearance(figure: Figure_id, interval: Interval) = 

        member this.figure = figure
        member this.interval = interval
        
        member this.head=this.interval.head
        member this.tail=this.interval.tail

        new (figure: Figure_id, moment: Moment) =
            Appearance(figure, {head= moment; tail= moment})
        new (figure: Figure_id, head, tail) =
            Appearance(figure, {head= head; tail= tail})
