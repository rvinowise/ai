namespace rvinowise.ai

    open System.Collections.Generic
    open rvinowise.ai


    type Stencil(
        id, 
        edges
    ) =
        member this.id:Figure_id = id
        member this.edges: stencil.Edge seq = edges
        
        new (id) =
            Stencil(id,[])



