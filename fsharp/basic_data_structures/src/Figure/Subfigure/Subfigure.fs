namespace rvinowise.ai.figure

open rvinowise.ai


type Subfigure(id, parent: Figure_id, referenced: Figure_id) = 
    member _.id: Subfigure_id = id
    member _.parent = parent
    member _.referenced = referenced


