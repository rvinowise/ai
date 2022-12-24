namespace rvinowise.ai

    open System.Collections.Generic
    open System.Text
    open rvinowise.ai.stencil
    open rvinowise.extensions


    type Stencil = {
        id: Figure_id
        edges: Edge seq
    }
    with 
        override this.ToString() =
            let result = StringBuilder()
            result 
            += $"Stencil_{this.id}( "
            this.edges
            |>Seq.iter(fun edge ->
                result 
                ++ edge.tail.id
                ++"->"
                ++ edge.head.id
                +=" "
            )
            result+=")"
            result.ToString()




