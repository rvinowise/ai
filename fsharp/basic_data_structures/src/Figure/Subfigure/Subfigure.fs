namespace rvinowise.ai.figure



type Subfigure(parent: string, referenced: string) = 
    member _.parent = parent
    member _.referenced = referenced

    new (parent, referenced) =
        Subfigure(parent, referenced)

