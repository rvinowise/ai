namespace rvinowise.ai.figure

open rvinowise.ai


type Subfigure(parent: Figure_id, referenced: Figure_id) = 
    member _.parent = parent
    member _.referenced = referenced

    new (parent, referenced) =
        Subfigure(parent, referenced)

